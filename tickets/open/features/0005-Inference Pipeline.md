# TICKET-0005

**Type**: Feature
**Status**: Open
**Created**: 2026-08-13
**Category**: Features
**Parent**: TICKET-0001
**Dependencies**: TICKET-0003, TICKET-0004

---

## Description

Build the core inference pipeline that takes user-selected features/target, preprocesses data automatically, runs model prediction, and returns results with metrics.

This is where the "magic" happens - user picks features, we handle the rest.

---

## Implementation Plan

### 1. Preprocessing Pipeline
**inference/preprocessing.py**:
```python
import pandas as pd
import numpy as np
from sklearn.preprocessing import (
    StandardScaler, MinMaxScaler, LabelEncoder, OneHotEncoder
)
from sklearn.impute import SimpleImputer
from typing import Dict, List, Optional

class PreprocessingPipeline:
    def __init__(self):
        self.scalers = {}
        self.encoders = {}
        self.imputers = {}
        self.feature_names = []
    
    def fit(self, df: pd.DataFrame, feature_cols: List[str], 
            target_col: str, schema: dict):
        """
        Fit preprocessing transformers based on schema.
        
        schema: {col_name: {'type': 'numeric'|'categorical'|...}}
        """
        X = df[feature_cols].copy()
        self.feature_names = feature_cols
        
        for col in feature_cols:
            col_type = schema[col]['type']
            
            # Handle missing values
            if df[col].isna().any():
                if col_type == 'numeric':
                    self.imputers[col] = SimpleImputer(strategy='mean')
                else:
                    self.imputers[col] = SimpleImputer(strategy='most_frequent')
                
                X[col] = self.imputers[col].fit_transform(X[[col]]).ravel()
            
            # Encoding for categorical
            if col_type == 'categorical':
                self.encoders[col] = LabelEncoder()
                X[col] = self.encoders[col].fit_transform(X[col])
            
            # Scaling for numeric
            elif col_type == 'numeric':
                self.scalers[col] = StandardScaler()
                X[col] = self.scalers[col].fit_transform(X[[col]]).ravel()
        
        return X
    
    def transform(self, df: pd.DataFrame) -> np.ndarray:
        """Transform new data using fitted transformers"""
        X = df[self.feature_names].copy()
        
        for col in self.feature_names:
            # Impute
            if col in self.imputers:
                X[col] = self.imputers[col].transform(X[[col]]).ravel()
            
            # Encode
            if col in self.encoders:
                X[col] = self.encoders[col].transform(X[col])
            
            # Scale
            if col in self.scalers:
                X[col] = self.scalers[col].transform(X[[col]]).ravel()
        
        return X.values
    
    def fit_transform(self, df: pd.DataFrame, feature_cols: List[str],
                     target_col: str, schema: dict) -> np.ndarray:
        """Fit and transform in one step"""
        X = self.fit(df, feature_cols, target_col, schema)
        return X.values
```

### 2. Inference Engine
**inference/engine.py**:
```python
from typing import Dict, List, Optional, Tuple
import pandas as pd
import numpy as np
from models.manager import ModelManager
from .preprocessing import PreprocessingPipeline
from sklearn.metrics import accuracy_score, f1_score, confusion_matrix

class InferenceEngine:
    def __init__(self, model_manager: ModelManager):
        self.model_manager = model_manager
    
    def predict(
        self,
        model_id: str,
        df: pd.DataFrame,
        feature_cols: List[str],
        target_col: Optional[str],
        schema: Dict,
        return_probabilities: bool = True
    ) -> Dict:
        """
        Run inference on dataset.
        
        Returns:
            {
                'predictions': [...],
                'probabilities': [...] (if classification),
                'metrics': {...} (if target_col provided),
                'feature_importance': {...}
            }
        """
        # Get model
        model = self.model_manager.get_model(model_id)
        
        # Preprocess
        pipeline = PreprocessingPipeline()
        
        if target_col and target_col in df.columns:
            # Training/evaluation mode
            y_true = df[target_col].values
            X = pipeline.fit_transform(df, feature_cols, target_col, schema)
        else:
            # Pure prediction mode
            y_true = None
            X = pipeline.fit_transform(df, feature_cols, None, schema)
        
        # Predict
        predictions = model.predict(X)
        
        result = {
            'predictions': predictions.tolist(),
            'preprocessing': {
                'scaled_features': [col for col in feature_cols if col in pipeline.scalers],
                'encoded_features': [col for col in feature_cols if col in pipeline.encoders],
                'imputed_features': [col for col in feature_cols if col in pipeline.imputers]
            }
        }
        
        # Probabilities (if classification)
        if return_probabilities:
            probas = model.predict_proba(X)
            if probas is not None:
                result['probabilities'] = probas.tolist()
        
        # Metrics (if ground truth available)
        if y_true is not None:
            result['metrics'] = self._calculate_metrics(
                y_true, predictions, model.metadata.get('type')
            )
        
        # Feature importance (if model supports it)
        if hasattr(model.model, 'feature_importances_'):
            importance = model.model.feature_importances_
            result['feature_importance'] = dict(zip(feature_cols, importance.tolist()))
        
        return result
    
    def _calculate_metrics(self, y_true, y_pred, model_type: str) -> Dict:
        """Calculate performance metrics"""
        if model_type == 'classification':
            return {
                'accuracy': float(accuracy_score(y_true, y_pred)),
                'f1_score': float(f1_score(y_true, y_pred, average='weighted')),
                'confusion_matrix': confusion_matrix(y_true, y_pred).tolist()
            }
        elif model_type == 'regression':
            from sklearn.metrics import mean_squared_error, r2_score
            return {
                'mse': float(mean_squared_error(y_true, y_pred)),
                'rmse': float(np.sqrt(mean_squared_error(y_true, y_pred))),
                'r2_score': float(r2_score(y_true, y_pred))
            }
        return {}
```

### 3. Request/Response Models
**inference/schemas.py**:
```python
from pydantic import BaseModel
from typing import List, Dict, Optional

class InferenceRequest(BaseModel):
    model_id: str
    data_id: str  # From data upload
    features: List[str]
    target: Optional[str] = None
    options: Optional[Dict] = {
        'return_probabilities': True,
        'calculate_metrics': True,
        'return_feature_importance': True
    }

class InferenceResponse(BaseModel):
    predictions: List
    probabilities: Optional[List[List[float]]] = None
    metrics: Optional[Dict] = None
    feature_importance: Optional[Dict[str, float]] = None
    preprocessing: Dict
    compute_time_seconds: float
    gpu_memory_used_mb: Optional[float] = None
```

### 4. API Endpoints
**api/routes/inference.py**:
```python
from fastapi import APIRouter, HTTPException
from inference.engine import InferenceEngine
from inference.schemas import InferenceRequest, InferenceResponse
from data.storage import DataStorage
from data.analyzer import DataAnalyzer
import time
import torch

router = APIRouter(prefix="/api/v1/inference", tags=["inference"])

# Initialize components
model_manager = None  # Injected from main.py
data_storage = DataStorage()
inference_engine = None  # Injected

@router.post("/predict", response_model=InferenceResponse)
async def predict(request: InferenceRequest):
    """
    Run inference on a dataset.
    
    Steps:
    1. Load data by data_id
    2. Get schema (or use cached)
    3. Preprocess data
    4. Run model prediction
    5. Calculate metrics if target provided
    6. Return results
    """
    start_time = time.time()
    
    try:
        # Load data
        df = data_storage.load_dataframe(request.data_id)
        if df is None:
            raise HTTPException(status_code=404, detail="Data not found")
        
        # Analyze schema
        analyzer = DataAnalyzer()
        schema_obj = analyzer.analyze_dataframe(df)
        schema = {col.name: {'type': col.type} for col in schema_obj.columns}
        
        # Validate features exist
        missing_features = set(request.features) - set(df.columns)
        if missing_features:
            raise HTTPException(
                status_code=400,
                detail=f"Features not found: {missing_features}"
            )
        
        # Run inference
        gpu_mem_before = torch.cuda.memory_allocated() / 1024**2 if torch.cuda.is_available() else None
        
        result = inference_engine.predict(
            model_id=request.model_id,
            df=df,
            feature_cols=request.features,
            target_col=request.target,
            schema=schema,
            return_probabilities=request.options.get('return_probabilities', True)
        )
        
        gpu_mem_after = torch.cuda.memory_allocated() / 1024**2 if torch.cuda.is_available() else None
        
        # Build response
        compute_time = time.time() - start_time
        
        return InferenceResponse(
            predictions=result['predictions'],
            probabilities=result.get('probabilities'),
            metrics=result.get('metrics'),
            feature_importance=result.get('feature_importance'),
            preprocessing=result['preprocessing'],
            compute_time_seconds=compute_time,
            gpu_memory_used_mb=gpu_mem_after - gpu_mem_before if gpu_mem_before else None
        )
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
```

### 5. Batch Inference
**inference/batch.py**:
```python
from typing import List, Dict
import pandas as pd

class BatchInference:
    """Handle large datasets with batching"""
    
    def __init__(self, batch_size: int = 1000):
        self.batch_size = batch_size
    
    def predict_batches(self, engine, model_id, df, **kwargs) -> Dict:
        """Process large datasets in batches"""
        n_samples = len(df)
        all_predictions = []
        all_probabilities = []
        
        for i in range(0, n_samples, self.batch_size):
            batch_df = df.iloc[i:i+self.batch_size]
            result = engine.predict(model_id, batch_df, **kwargs)
            all_predictions.extend(result['predictions'])
            if result.get('probabilities'):
                all_probabilities.extend(result['probabilities'])
        
        return {
            'predictions': all_predictions,
            'probabilities': all_probabilities if all_probabilities else None
        }
```

---

## Testing Checklist

- [ ] Can preprocess numeric features (scaling)
- [ ] Can preprocess categorical features (encoding)
- [ ] Can handle missing values (imputation)
- [ ] Can run prediction on preprocessed data
- [ ] Returns predictions in correct format
- [ ] Returns probabilities for classification
- [ ] Calculates metrics when target is provided
- [ ] Returns feature importance
- [ ] Tracks compute time
- [ ] Tracks GPU memory usage
- [ ] Batch inference works for large datasets
- [ ] Error handling works (missing features, etc.)

---

## Success Criteria

- [ ] Complete inference pipeline works end-to-end
- [ ] Automatic preprocessing based on data types
- [ ] Predictions are returned correctly
- [ ] Metrics are calculated when ground truth available
- [ ] Feature importance is returned (when supported)
- [ ] GPU memory tracking works
- [ ] Batch processing works for large datasets
- [ ] API endpoints are functional
- [ ] Documentation is complete

---

## Notes

**Preprocessing**: Automatic based on detected column types

**Supported**:
- Scaling: StandardScaler (configurable later)
- Encoding: LabelEncoder (OneHot in Phase 2)
- Imputation: Mean (numeric), Most Frequent (categorical)

**Batch Size**: 1000 samples (configurable)

**Future**: Custom preprocessing pipelines (Phase 2), feature engineering (Phase 3)

---

## Token Usage

(Track via /log-cost when completed)
