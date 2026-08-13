# TICKET-0004

**Type**: Feature
**Status**: Open
**Created**: 2026-08-13
**Category**: Features
**Parent**: TICKET-0001
**Dependencies**: TICKET-0002

---

## Description

Build data ingestion system that handles CSV file uploads, automatically detects schema, infers data types, and provides metadata about the dataset (column types, statistics, quality metrics).

---

## Implementation Plan

### 1. Data Models (Pydantic)
**data/schemas.py**:
```python
from pydantic import BaseModel
from typing import List, Dict, Optional, Literal
from datetime import datetime

class ColumnSchema(BaseModel):
    name: str
    type: Literal["numeric", "categorical", "datetime", "text", "boolean"]
    dtype: str  # pandas dtype
    missing_count: int
    missing_percentage: float
    unique_count: int
    stats: Optional[Dict]  # numeric: mean/std/min/max, categorical: distribution

class DataQuality(BaseModel):
    total_rows: int
    total_columns: int
    missing_percentage: float
    duplicate_rows: int
    outliers_detected: Optional[int]

class DatasetSchema(BaseModel):
    columns: List[ColumnSchema]
    row_count: int
    quality: DataQuality
    detected_at: datetime
```

### 2. Data Type Detection
**data/type_detector.py**:
```python
import pandas as pd
import numpy as np

class TypeDetector:
    @staticmethod
    def detect_column_type(series: pd.Series) -> str:
        """Detect semantic type of a column"""
        # Drop NaN for analysis
        clean_series = series.dropna()
        
        # Empty column
        if len(clean_series) == 0:
            return "unknown"
        
        # Boolean
        if clean_series.nunique() == 2:
            return "boolean"
        
        # Numeric
        if pd.api.types.is_numeric_dtype(series):
            return "numeric"
        
        # Datetime
        try:
            pd.to_datetime(clean_series)
            return "datetime"
        except:
            pass
        
        # Categorical vs Text
        unique_ratio = clean_series.nunique() / len(clean_series)
        if unique_ratio < 0.05 or clean_series.nunique() < 20:
            return "categorical"
        
        return "text"
    
    @staticmethod
    def get_column_stats(series: pd.Series, col_type: str) -> Dict:
        """Get statistics based on column type"""
        if col_type == "numeric":
            return {
                "mean": float(series.mean()),
                "std": float(series.std()),
                "min": float(series.min()),
                "max": float(series.max()),
                "quartiles": series.quantile([0.25, 0.5, 0.75]).tolist()
            }
        elif col_type == "categorical":
            value_counts = series.value_counts().head(10)
            return {
                "distribution": value_counts.to_dict(),
                "top_values": value_counts.index.tolist()
            }
        elif col_type == "datetime":
            dt_series = pd.to_datetime(series)
            return {
                "min": dt_series.min().isoformat(),
                "max": dt_series.max().isoformat()
            }
        return {}
```

### 3. Schema Analyzer
**data/analyzer.py**:
```python
import pandas as pd
from .schemas import DatasetSchema, ColumnSchema, DataQuality
from .type_detector import TypeDetector

class DataAnalyzer:
    def __init__(self):
        self.detector = TypeDetector()
    
    def analyze_dataframe(self, df: pd.DataFrame) -> DatasetSchema:
        """Analyze pandas DataFrame and return schema"""
        columns = []
        
        for col_name in df.columns:
            series = df[col_name]
            col_type = self.detector.detect_column_type(series)
            
            column_schema = ColumnSchema(
                name=col_name,
                type=col_type,
                dtype=str(series.dtype),
                missing_count=int(series.isna().sum()),
                missing_percentage=float(series.isna().sum() / len(df) * 100),
                unique_count=int(series.nunique()),
                stats=self.detector.get_column_stats(series, col_type)
            )
            columns.append(column_schema)
        
        # Quality metrics
        quality = DataQuality(
            total_rows=len(df),
            total_columns=len(df.columns),
            missing_percentage=float(df.isna().sum().sum() / (len(df) * len(df.columns)) * 100),
            duplicate_rows=int(df.duplicated().sum()),
            outliers_detected=self._detect_outliers(df)
        )
        
        return DatasetSchema(
            columns=columns,
            row_count=len(df),
            quality=quality,
            detected_at=datetime.now()
        )
    
    def _detect_outliers(self, df: pd.DataFrame) -> int:
        """Detect outliers using IQR method on numeric columns"""
        numeric_cols = df.select_dtypes(include=[np.number]).columns
        outlier_count = 0
        
        for col in numeric_cols:
            Q1 = df[col].quantile(0.25)
            Q3 = df[col].quantile(0.75)
            IQR = Q3 - Q1
            lower_bound = Q1 - 1.5 * IQR
            upper_bound = Q3 + 1.5 * IQR
            outliers = df[(df[col] < lower_bound) | (df[col] > upper_bound)]
            outlier_count += len(outliers)
        
        return outlier_count
```

### 4. File Upload Handler
**data/upload.py**:
```python
from fastapi import UploadFile
import pandas as pd
import io
from typing import Optional

class FileUploader:
    SUPPORTED_FORMATS = ['.csv', '.xlsx', '.json', '.parquet']
    MAX_FILE_SIZE_MB = 100
    
    @staticmethod
    async def read_uploaded_file(file: UploadFile) -> pd.DataFrame:
        """Read uploaded file into pandas DataFrame"""
        # Validate file size
        contents = await file.read()
        size_mb = len(contents) / (1024 * 1024)
        
        if size_mb > FileUploader.MAX_FILE_SIZE_MB:
            raise ValueError(f"File too large: {size_mb:.2f}MB (max {FileUploader.MAX_FILE_SIZE_MB}MB)")
        
        # Detect format and read
        if file.filename.endswith('.csv'):
            df = pd.read_csv(io.BytesIO(contents))
        elif file.filename.endswith('.xlsx'):
            df = pd.read_excel(io.BytesIO(contents))
        elif file.filename.endswith('.json'):
            df = pd.read_json(io.BytesIO(contents))
        elif file.filename.endswith('.parquet'):
            df = pd.read_parquet(io.BytesIO(contents))
        else:
            raise ValueError(f"Unsupported file format: {file.filename}")
        
        return df
    
    @staticmethod
    def read_csv_from_string(csv_content: str) -> pd.DataFrame:
        """Read CSV from string (for API requests)"""
        return pd.read_csv(io.StringIO(csv_content))
```

### 5. API Endpoints
**api/routes/data.py**:
```python
from fastapi import APIRouter, UploadFile, File, HTTPException
from data.upload import FileUploader
from data.analyzer import DataAnalyzer
from data.schemas import DatasetSchema

router = APIRouter(prefix="/api/v1/data", tags=["data"])

@router.post("/analyze", response_model=DatasetSchema)
async def analyze_data(file: UploadFile = File(...)):
    """
    Upload a dataset and get schema/metadata.
    
    Supports: CSV, Excel, JSON, Parquet
    Max size: 100MB
    """
    try:
        # Upload and read file
        uploader = FileUploader()
        df = await uploader.read_uploaded_file(file)
        
        # Analyze schema
        analyzer = DataAnalyzer()
        schema = analyzer.analyze_dataframe(df)
        
        return schema
    
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Analysis failed: {str(e)}")

@router.post("/preview")
async def preview_data(file: UploadFile = File(...), rows: int = 10):
    """
    Preview first N rows of uploaded dataset.
    """
    try:
        uploader = FileUploader()
        df = await uploader.read_uploaded_file(file)
        
        preview = df.head(rows).to_dict(orient='records')
        
        return {
            "total_rows": len(df),
            "preview_rows": len(preview),
            "columns": list(df.columns),
            "data": preview
        }
    
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
```

### 6. Data Storage (Temporary)
**data/storage.py**:
```python
import hashlib
import pandas as pd
from pathlib import Path
from typing import Optional

class DataStorage:
    def __init__(self, storage_path: str = "data/uploads"):
        self.storage_path = Path(storage_path)
        self.storage_path.mkdir(parents=True, exist_ok=True)
    
    def store_dataframe(self, df: pd.DataFrame, filename: str) -> str:
        """Store DataFrame and return unique ID"""
        # Generate unique ID from content hash
        content_hash = hashlib.md5(df.to_csv(index=False).encode()).hexdigest()
        data_id = f"{content_hash}_{filename}"
        
        # Save as parquet (efficient, preserves types)
        file_path = self.storage_path / f"{data_id}.parquet"
        df.to_parquet(file_path, index=False)
        
        return data_id
    
    def load_dataframe(self, data_id: str) -> Optional[pd.DataFrame]:
        """Load DataFrame by ID"""
        file_path = self.storage_path / f"{data_id}.parquet"
        if file_path.exists():
            return pd.read_parquet(file_path)
        return None
```

---

## Testing Checklist

- [ ] CSV upload works
- [ ] Schema detection identifies all column types correctly
- [ ] Numeric columns get statistics (mean, std, quartiles)
- [ ] Categorical columns get value distributions
- [ ] Missing value counts are accurate
- [ ] Duplicate row detection works
- [ ] Outlier detection works (IQR method)
- [ ] File size validation works (rejects >100MB)
- [ ] Unsupported formats are rejected
- [ ] Preview endpoint returns first N rows
- [ ] Data storage/retrieval works

---

## Success Criteria

- [ ] Can upload CSV files via API
- [ ] Schema is automatically detected and returned
- [ ] Column types are correctly identified (numeric, categorical, datetime, text, boolean)
- [ ] Statistics are calculated for each column type
- [ ] Data quality metrics are computed
- [ ] Preview endpoint works
- [ ] Uploaded data can be stored and retrieved
- [ ] Documentation is complete

---

## Notes

**Supported Formats** (Phase 1): CSV, Excel, JSON, Parquet

**Max File Size**: 100MB (configurable)

**Storage**: Temporary local storage (Phase 2 adds MinIO/S3)

**Future**: Database connectors (TICKET-0009), streaming data (Phase 2)

---

## Token Usage

(Track via /log-cost when completed)
