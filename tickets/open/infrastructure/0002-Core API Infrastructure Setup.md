# TICKET-0002

**Type**: Feature
**Status**: Open
**Created**: 2026-08-13
**Category**: Infrastructure
**Parent**: TICKET-0001
**Dependencies**: None

---

## Description

Set up the foundational FastAPI application infrastructure with GPU detection, health checks, logging, and basic project structure.

This is the **first ticket in Phase 1** - everything else builds on this foundation.

---

## Implementation Plan

### 1. Project Structure Setup
```
gpu_api/
├── api/
│   ├── __init__.py
│   ├── main.py              # FastAPI app entry point
│   ├── config.py            # Configuration management
│   └── routes/
│       ├── __init__.py
│       └── health.py        # Health check endpoints
├── core/
│   ├── __init__.py
│   ├── gpu.py               # GPU detection and management
│   └── logging.py           # Structured logging setup
├── models/                  # (created in TICKET-0003)
├── data/                    # (created in TICKET-0004)
├── inference/               # (created in TICKET-0005)
├── tests/
│   ├── __init__.py
│   └── test_health.py
├── requirements.txt
├── setup.py
└── README.md
```

### 2. FastAPI Application
- Create FastAPI app with CORS middleware
- Configure uvicorn server
- Add automatic OpenAPI/Swagger documentation
- Set up exception handlers (global error handling)
- Add request/response logging middleware

### 3. GPU Detection & Initialization
- Detect NVIDIA GPU (RTX 4080 Super)
- Check CUDA availability via PyTorch
- Log GPU specs:
  - Name, compute capability
  - Total memory, available memory
  - CUDA version, cuDNN version
- Set default device (cuda:0)
- Create GPU health check function

### 4. Configuration Management
- Environment-based config (dev, prod)
- Settings via Pydantic BaseSettings:
  - API host, port
  - CORS origins
  - Log level
  - GPU device ID
  - Max concurrent requests
- Support .env file loading

### 5. Structured Logging
- Set up structlog for JSON logging
- Log levels: DEBUG, INFO, WARNING, ERROR
- Include request IDs for tracing
- Log to stdout (for Docker) and file
- GPU metrics logging

### 6. Health Check Endpoints
```python
GET /health
{
  "status": "healthy",
  "timestamp": "2026-08-13T14:30:00Z",
  "version": "0.1.0"
}

GET /health/gpu
{
  "status": "healthy",
  "gpu": {
    "available": true,
    "name": "NVIDIA GeForce RTX 4080 SUPER",
    "memory_total_mb": 16384,
    "memory_allocated_mb": 0,
    "memory_reserved_mb": 0,
    "cuda_version": "12.1",
    "device_count": 1
  }
}
```

### 7. Dependencies Installation
**requirements.txt**:
```
fastapi==0.115.0
uvicorn[standard]==0.30.0
pydantic==2.9.0
pydantic-settings==2.5.0
python-dotenv==1.0.1
structlog==24.4.0
torch==2.5.0+cu121
```

### 8. Development Scripts
**scripts/dev_server.bat**:
```batch
@echo off
uvicorn api.main:app --reload --host 0.0.0.0 --port 8000
```

### 9. Docker Setup (Optional)
- Dockerfile with CUDA base image
- docker-compose.yml for local development
- GPU passthrough configuration

### 10. Documentation
- Update README with setup instructions
- Document API endpoints (health checks)
- Add GPU requirements section
- Include troubleshooting guide

---

## Testing Checklist

- [ ] FastAPI app starts without errors
- [ ] Health endpoint returns 200 OK
- [ ] GPU health endpoint detects RTX 4080 Super
- [ ] CUDA is properly initialized
- [ ] Logs are structured and readable
- [ ] CORS is configured correctly
- [ ] OpenAPI docs accessible at /docs
- [ ] Environment variables are loaded
- [ ] Exception handling works (test with invalid route)

---

## Success Criteria

- [ ] FastAPI application runs and serves requests
- [ ] GPU is detected and CUDA is available
- [ ] Health endpoints return correct information
- [ ] Structured logging is working
- [ ] Configuration management is functional
- [ ] Documentation is complete
- [ ] Can run locally with `scripts/dev_server.bat`

---

## Notes

**Critical Path**: This must be completed before any other Phase 1 tickets.

**GPU Requirements**: NVIDIA GPU with CUDA support (RTX 4080 Super)

**Port**: Default 8000 (configurable)

---

## Token Usage

(Track via /log-cost when completed)
