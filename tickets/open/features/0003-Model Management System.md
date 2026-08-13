# TICKET-0003

**Type**: Feature
**Status**: Open
**Created**: 2026-08-13
**Category**: Features
**Parent**: TICKET-0001
**Dependencies**: TICKET-0002

---

## Description

Build the model management system that handles model registration, loading, unloading, and metadata tracking. Supports basic sklearn models initially (can be extended later).

---

## Implementation Plan

### 1. Model Registry Database
**SQLite initially** (lightweight, file-based):
```sql
CREATE TABLE models (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL,
    type TEXT NOT NULL,  -- 'classification', 'regression'
    version TEXT NOT NULL,
    description TEXT,
    framework TEXT,  -- 'sklearn', 'pytorch', 'xgboost'
    file_path TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT 1,
    metadata JSON  -- performance metrics, hyperparameters, etc.
);
```

### 2. Model Storage Structure
```
models/
├── registry.db           # SQLite database
├── sklearn/
│   ├── random_forest_clf_v1.pkl
│   ├── logistic_regression_v1.pkl
│   └── xgboost_clf_v1.pkl
└── metadata/
    ├── random_forest_clf_v1.json
    └── logistic_regression_v1.json
```

### 3. Model Classes
**models/base.py**:
```python
from abc import ABC, abstractmethod
from typing import Any, Dict, List
import numpy as np

class BaseModel(ABC):
    def __init__(self, model_id: str, metadata: Dict[str, Any]):
        self.model_id = model_id
        self.metadata = metadata
        self.model = None
    
    @abstractmethod
    def load(self):
        """Load model into memory"""
        pass
    
    @abstractmethod
    def predict(self, X: np.ndarray) -> np.ndarray:
        """Run inference"""
        pass
    
    @abstractmethod
    def predict_proba(self, X: np.ndarray) -> np.ndarray:
        """Get prediction probabilities (if applicable)"""
        pass
    
    def unload(self):
        """Free memory"""
        self.model = None
```

**models/sklearn_model.py**:
```python
import joblib
import torch
from .base import BaseModel

class SklearnModel(BaseModel):
    def load(self):
        self.model = joblib.load(self.metadata['file_path'])
        # Move to GPU if supported (e.g., XGBoost with GPU)
        if hasattr(self.model, 'set_params'):
            if 'tree_method' in self.model.get_params():
                self.model.set_params(tree_method='gpu_hist')
    
    def predict(self, X):
        return self.model.predict(X)
    
    def predict_proba(self, X):
        if hasattr(self.model, 'predict_proba'):
            return self.model.predict_proba(X)
        return None
```

### 4. Model Manager
**models/manager.py**:
```python
from typing import Dict, List, Optional
import sqlite3
from .base import BaseModel
from .sklearn_model import SklearnModel

class ModelManager:
    def __init__(self, db_path: str = "models/registry.db"):
        self.db_path = db_path
        self.loaded_models: Dict[str, BaseModel] = {}
        self._init_db()
    
    def _init_db(self):
        """Initialize database schema"""
        pass
    
    def register_model(self, model_data: dict) -> str:
        """Register new model in registry"""
        pass
    
    def get_model(self, model_id: str) -> BaseModel:
        """Get model (load if not in memory)"""
        pass
    
    def list_models(self, active_only: bool = True) -> List[dict]:
        """List all registered models"""
        pass
    
    def unload_model(self, model_id: str):
        """Unload model from memory"""
        pass
    
    def delete_model(self, model_id: str):
        """Mark model as inactive"""
        pass
```

### 5. Pre-trained Models
Include sample models for immediate testing:
```python
# scripts/seed_models.py
from sklearn.ensemble import RandomForestClassifier
from sklearn.linear_model import LogisticRegression
from xgboost import XGBClassifier
import joblib

# Train simple models on iris dataset
from sklearn.datasets import load_iris
X, y = load_iris(return_X_y=True)

# Random Forest
rf = RandomForestClassifier(n_estimators=100, random_state=42)
rf.fit(X, y)
joblib.dump(rf, 'models/sklearn/random_forest_clf_v1.pkl')

# Logistic Regression
lr = LogisticRegression(max_iter=200, random_state=42)
lr.fit(X, y)
joblib.dump(lr, 'models/sklearn/logistic_regression_v1.pkl')

# XGBoost (GPU-accelerated)
xgb = XGBClassifier(tree_method='gpu_hist', gpu_id=0, random_state=42)
xgb.fit(X, y)
joblib.dump(xgb, 'models/sklearn/xgboost_clf_v1.pkl')
```

### 6. API Endpoints
**api/routes/models.py**:
```python
from fastapi import APIRouter, HTTPException
from typing import List
from pydantic import BaseModel

router = APIRouter(prefix="/api/v1/models", tags=["models"])

class ModelResponse(BaseModel):
    id: str
    name: str
    type: str
    version: str
    description: str
    framework: str
    created_at: str
    metadata: dict

@router.get("/", response_model=List[ModelResponse])
async def list_models():
    """List all available models"""
    pass

@router.get("/{model_id}", response_model=ModelResponse)
async def get_model(model_id: str):
    """Get model details"""
    pass

@router.post("/{model_id}/load")
async def load_model(model_id: str):
    """Pre-load model into GPU memory"""
    pass

@router.post("/{model_id}/unload")
async def unload_model(model_id: str):
    """Unload model from memory"""
    pass
```

### 7. GPU Memory Management
```python
# core/gpu.py (extend from TICKET-0002)
def get_gpu_memory_info():
    """Get current GPU memory usage"""
    if torch.cuda.is_available():
        return {
            'allocated_mb': torch.cuda.memory_allocated() / 1024**2,
            'reserved_mb': torch.cuda.memory_reserved() / 1024**2,
            'max_allocated_mb': torch.cuda.max_memory_allocated() / 1024**2,
        }
    return None

def clear_gpu_cache():
    """Clear CUDA cache"""
    if torch.cuda.is_available():
        torch.cuda.empty_cache()
```

### 8. Model Metadata Format
**metadata JSON example**:
```json
{
  "id": "random_forest_clf_v1",
  "name": "Random Forest Classifier",
  "type": "classification",
  "version": "1.0.0",
  "description": "100-tree random forest for multi-class classification",
  "framework": "sklearn",
  "file_path": "models/sklearn/random_forest_clf_v1.pkl",
  "supported_features": ["numeric"],
  "num_classes": 3,
  "feature_count": 4,
  "metrics": {
    "accuracy": 0.97,
    "f1_score": 0.96
  },
  "hyperparameters": {
    "n_estimators": 100,
    "max_depth": null,
    "random_state": 42
  }
}
```

---

## Testing Checklist

- [ ] Database initializes correctly
- [ ] Can register new model
- [ ] Can list all models
- [ ] Can load model into memory
- [ ] Can get model details by ID
- [ ] Can unload model from memory
- [ ] GPU memory is tracked correctly
- [ ] XGBoost uses GPU (tree_method='gpu_hist')
- [ ] Models can be marked inactive
- [ ] Seed models script works

---

## Success Criteria

- [ ] Model registry database is functional
- [ ] At least 3 sample models are pre-loaded (RF, LogReg, XGBoost)
- [ ] API endpoints return correct model metadata
- [ ] Models can be loaded/unloaded dynamically
- [ ] GPU memory usage is tracked
- [ ] Documentation is complete

---

## Notes

**Storage**: SQLite for MVP (migrate to PostgreSQL in Phase 2)

**Supported Frameworks** (Phase 1): sklearn, XGBoost

**GPU Acceleration**: XGBoost with `tree_method='gpu_hist'` uses GPU

**Future**: PyTorch models (TICKET-0016), custom uploads (TICKET-0018)

---

## Token Usage

(Track via /log-cost when completed)
