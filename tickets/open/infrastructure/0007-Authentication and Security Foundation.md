# TICKET-0007

**Type**: Feature
**Status**: Open
**Created**: 2026-08-13
**Category**: Infrastructure
**Parent**: TICKET-0001
**Dependencies**: TICKET-0002

---

## Description

Implement API key authentication system, rate limiting, and basic security measures to protect the API from unauthorized access and abuse.

---

## Implementation Plan

### 1. API Key System
**Database Schema**:
```sql
CREATE TABLE api_keys (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    key TEXT UNIQUE NOT NULL,
    name TEXT NOT NULL,
    user_email TEXT,
    is_active BOOLEAN DEFAULT 1,
    rate_limit_per_minute INTEGER DEFAULT 60,
    rate_limit_per_hour INTEGER DEFAULT 1000,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_used_at TIMESTAMP,
    expires_at TIMESTAMP,
    metadata JSON
);

CREATE INDEX idx_api_keys_key ON api_keys(key);
CREATE INDEX idx_api_keys_active ON api_keys(is_active);
```

### 2. API Key Generation
**core/auth.py**:
```python
import secrets
import hashlib
import sqlite3
from datetime import datetime, timedelta
from typing import Optional, Dict

class APIKeyManager:
    def __init__(self, db_path: str = "data/api_keys.db"):
        self.db_path = db_path
        self._init_db()
    
    def _init_db(self):
        """Initialize API keys database"""
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS api_keys (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                key TEXT UNIQUE NOT NULL,
                name TEXT NOT NULL,
                user_email TEXT,
                is_active BOOLEAN DEFAULT 1,
                rate_limit_per_minute INTEGER DEFAULT 60,
                rate_limit_per_hour INTEGER DEFAULT 1000,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                last_used_at TIMESTAMP,
                expires_at TIMESTAMP,
                metadata TEXT
            )
        """)
        cursor.execute("CREATE INDEX IF NOT EXISTS idx_api_keys_key ON api_keys(key)")
        conn.commit()
        conn.close()
    
    def generate_key(
        self,
        name: str,
        user_email: Optional[str] = None,
        expires_days: Optional[int] = None
    ) -> str:
        """
        Generate new API key.
        
        Format: gpuml_<32_random_chars>
        """
        # Generate secure random key
        random_part = secrets.token_urlsafe(24)  # 32 chars after encoding
        api_key = f"gpuml_{random_part}"
        
        # Calculate expiration
        expires_at = None
        if expires_days:
            expires_at = datetime.now() + timedelta(days=expires_days)
        
        # Store in database
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        cursor.execute("""
            INSERT INTO api_keys (key, name, user_email, expires_at)
            VALUES (?, ?, ?, ?)
        """, (api_key, name, user_email, expires_at))
        conn.commit()
        conn.close()
        
        return api_key
    
    def validate_key(self, api_key: str) -> Optional[Dict]:
        """
        Validate API key and return key info if valid.
        
        Returns None if invalid/expired.
        """
        conn = sqlite3.connect(self.db_path)
        conn.row_factory = sqlite3.Row
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT * FROM api_keys
            WHERE key = ? AND is_active = 1
        """, (api_key,))
        
        row = cursor.fetchone()
        
        if row:
            # Check expiration
            if row['expires_at']:
                expires_at = datetime.fromisoformat(row['expires_at'])
                if datetime.now() > expires_at:
                    conn.close()
                    return None
            
            # Update last used
            cursor.execute("""
                UPDATE api_keys SET last_used_at = ?
                WHERE key = ?
            """, (datetime.now(), api_key))
            conn.commit()
            
            result = dict(row)
            conn.close()
            return result
        
        conn.close()
        return None
    
    def revoke_key(self, api_key: str):
        """Revoke (deactivate) an API key"""
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        cursor.execute("UPDATE api_keys SET is_active = 0 WHERE key = ?", (api_key,))
        conn.commit()
        conn.close()
    
    def list_keys(self, active_only: bool = True) -> list:
        """List all API keys"""
        conn = sqlite3.connect(self.db_path)
        conn.row_factory = sqlite3.Row
        cursor = conn.cursor()
        
        if active_only:
            cursor.execute("SELECT * FROM api_keys WHERE is_active = 1 ORDER BY created_at DESC")
        else:
            cursor.execute("SELECT * FROM api_keys ORDER BY created_at DESC")
        
        keys = [dict(row) for row in cursor.fetchall()]
        conn.close()
        return keys
```

### 3. Authentication Middleware
**api/middleware/auth.py**:
```python
from fastapi import Request, HTTPException, status
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from core.auth import APIKeyManager

security = HTTPBearer()
api_key_manager = APIKeyManager()

async def verify_api_key(request: Request):
    """
    Verify API key from header.
    
    Expects: Authorization: Bearer <api_key>
    Or: X-API-Key: <api_key>
    """
    # Try Authorization header first
    auth_header = request.headers.get("Authorization")
    if auth_header and auth_header.startswith("Bearer "):
        api_key = auth_header.replace("Bearer ", "")
    else:
        # Try X-API-Key header
        api_key = request.headers.get("X-API-Key")
    
    if not api_key:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="API key required. Provide via 'Authorization: Bearer <key>' or 'X-API-Key: <key>' header."
        )
    
    # Validate key
    key_info = api_key_manager.validate_key(api_key)
    
    if not key_info:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid or expired API key"
        )
    
    # Attach key info to request state
    request.state.api_key_info = key_info
    
    return key_info
```

### 4. Rate Limiting
**api/middleware/rate_limit.py**:
```python
from fastapi import Request, HTTPException, status
import redis.asyncio as redis
from datetime import datetime, timedelta
import json

class RateLimiter:
    def __init__(self, redis_url: str = "redis://localhost:6379"):
        self.redis_client = redis.from_url(redis_url, decode_responses=True)
    
    async def check_rate_limit(
        self,
        api_key: str,
        limit_per_minute: int,
        limit_per_hour: int
    ) -> bool:
        """
        Check if API key is within rate limits.
        
        Uses sliding window algorithm.
        """
        now = datetime.now()
        
        # Minute window
        minute_key = f"ratelimit:{api_key}:minute:{now.strftime('%Y%m%d%H%M')}"
        minute_count = await self.redis_client.incr(minute_key)
        
        if minute_count == 1:
            await self.redis_client.expire(minute_key, 60)
        
        if minute_count > limit_per_minute:
            return False
        
        # Hour window
        hour_key = f"ratelimit:{api_key}:hour:{now.strftime('%Y%m%d%H')}"
        hour_count = await self.redis_client.incr(hour_key)
        
        if hour_count == 1:
            await self.redis_client.expire(hour_key, 3600)
        
        if hour_count > limit_per_hour:
            return False
        
        return True
    
    async def get_remaining(self, api_key: str, limit_per_minute: int) -> int:
        """Get remaining requests in current minute"""
        now = datetime.now()
        minute_key = f"ratelimit:{api_key}:minute:{now.strftime('%Y%m%d%H%M')}"
        count = await self.redis_client.get(minute_key)
        return limit_per_minute - int(count or 0)

# Dependency
async def check_rate_limit(request: Request):
    """Rate limit middleware"""
    rate_limiter = request.app.state.rate_limiter
    key_info = request.state.api_key_info
    
    allowed = await rate_limiter.check_rate_limit(
        api_key=key_info['key'],
        limit_per_minute=key_info['rate_limit_per_minute'],
        limit_per_hour=key_info['rate_limit_per_hour']
    )
    
    if not allowed:
        raise HTTPException(
            status_code=status.HTTP_429_TOO_MANY_REQUESTS,
            detail="Rate limit exceeded"
        )
    
    # Add rate limit headers
    remaining = await rate_limiter.get_remaining(
        key_info['key'],
        key_info['rate_limit_per_minute']
    )
    
    # Attach to response (done in middleware)
    request.state.rate_limit_remaining = remaining
```

### 5. API Key Management Endpoints
**api/routes/auth.py**:
```python
from fastapi import APIRouter, HTTPException, Depends
from pydantic import BaseModel, EmailStr
from typing import Optional
from core.auth import APIKeyManager

router = APIRouter(prefix="/api/v1/auth", tags=["authentication"])
api_key_manager = APIKeyManager()

class CreateAPIKeyRequest(BaseModel):
    name: str
    user_email: Optional[EmailStr] = None
    expires_days: Optional[int] = None

class APIKeyResponse(BaseModel):
    api_key: str
    name: str
    created_at: str
    expires_at: Optional[str]

@router.post("/keys", response_model=APIKeyResponse)
async def create_api_key(request: CreateAPIKeyRequest):
    """
    Create new API key.
    
    NOTE: This endpoint should be protected in production!
    """
    api_key = api_key_manager.generate_key(
        name=request.name,
        user_email=request.user_email,
        expires_days=request.expires_days
    )
    
    # Get key info
    key_info = api_key_manager.validate_key(api_key)
    
    return APIKeyResponse(
        api_key=api_key,
        name=key_info['name'],
        created_at=key_info['created_at'],
        expires_at=key_info.get('expires_at')
    )

@router.get("/keys")
async def list_api_keys():
    """List all API keys (excluding the key itself for security)"""
    keys = api_key_manager.list_keys()
    
    # Remove actual key from response
    for key in keys:
        key['key'] = key['key'][:10] + "..." + key['key'][-4:]
    
    return {"keys": keys}

@router.delete("/keys/{key_id}")
async def revoke_api_key(key_id: int):
    """Revoke an API key"""
    # Implementation
    return {"message": "Key revoked"}
```

### 6. Apply Middleware to App
**api/main.py** (extend):
```python
from api.middleware.auth import verify_api_key
from api.middleware.rate_limit import RateLimiter, check_rate_limit
from fastapi import Depends

# Initialize rate limiter
app.state.rate_limiter = RateLimiter()

# Protected routes (require auth + rate limit)
@app.get("/api/v1/models", dependencies=[Depends(verify_api_key), Depends(check_rate_limit)])
async def list_models():
    # ... existing code

@app.post("/api/v1/inference/predict", dependencies=[Depends(verify_api_key), Depends(check_rate_limit)])
async def predict():
    # ... existing code

# Public routes (no auth required)
@app.get("/health")
async def health():
    # ... existing code
```

### 7. Security Headers Middleware
**api/middleware/security.py**:
```python
from fastapi import Request
from starlette.middleware.base import BaseHTTPMiddleware

class SecurityHeadersMiddleware(BaseHTTPMiddleware):
    async def dispatch(self, request: Request, call_next):
        response = await call_next(request)
        
        # Security headers
        response.headers["X-Content-Type-Options"] = "nosniff"
        response.headers["X-Frame-Options"] = "DENY"
        response.headers["X-XSS-Protection"] = "1; mode=block"
        response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains"
        
        # Rate limit headers (if available)
        if hasattr(request.state, 'rate_limit_remaining'):
            response.headers["X-RateLimit-Remaining"] = str(request.state.rate_limit_remaining)
        
        return response

# Add to app
app.add_middleware(SecurityHeadersMiddleware)
```

### 8. Input Validation & Sanitization
- Already handled by Pydantic models
- Add max file size limits
- Validate file types
- SQL injection prevention (parameterized queries)

### 9. Bootstrap Script
**scripts/create_api_key.py**:
```python
from core.auth import APIKeyManager

manager = APIKeyManager()

# Create default API key for development
key = manager.generate_key(
    name="Development Key",
    user_email="dev@example.com",
    expires_days=365
)

print(f"API Key created: {key}")
print("Add this to your requests:")
print(f"  Authorization: Bearer {key}")
print(f"  OR")
print(f"  X-API-Key: {key}")
```

---

## Testing Checklist

- [ ] Can generate API keys
- [ ] API key validation works
- [ ] Invalid keys are rejected (401)
- [ ] Expired keys are rejected
- [ ] Rate limiting works (429 after limit)
- [ ] Rate limit headers are returned
- [ ] Can revoke API keys
- [ ] Security headers are added
- [ ] CORS is configured correctly
- [ ] File upload size limits work
- [ ] SQL injection attempts are blocked
- [ ] Protected endpoints require auth
- [ ] Public endpoints don't require auth

---

## Success Criteria

- [ ] API key system is functional
- [ ] Rate limiting prevents abuse
- [ ] Authentication middleware works
- [ ] Security headers are applied
- [ ] Bootstrap script creates default key
- [ ] Documentation includes auth examples
- [ ] Error messages are clear and helpful

---

## Notes

**Rate Limits** (Default):
- 60 requests/minute
- 1000 requests/hour
(Configurable per API key)

**Redis Required**: For rate limiting (install locally or use Docker)

**Security**: 
- Keys use Bearer token format
- Keys are never logged in plain text
- Use HTTPS in production

**Future**: OAuth2 (Phase 2), user accounts (Phase 2), billing integration (Phase 5)

---

## Token Usage

(Track via /log-cost when completed)
