# TICKET-0006

**Type**: Feature
**Status**: Open
**Created**: 2026-08-13
**Category**: Features
**Parent**: TICKET-0001
**Dependencies**: TICKET-0005

---

## Description

Build a clean, modern web UI that allows users to upload data, select models, choose features/target, and view prediction results. Simple and intuitive for non-technical users.

**Goal**: User can run ML inference without writing a single line of code.

---

## Implementation Plan

### 1. Technology Stack
- **Frontend Framework**: Vanilla JavaScript + HTML5 + CSS3 (lightweight, no build step)
- **Alternative**: Vue.js or React (if complexity grows)
- **UI Components**: Bootstrap 5 or Tailwind CSS
- **Charts**: Chart.js for visualizations
- **HTTP Client**: Fetch API

### 2. Project Structure
```
web/
├── static/
│   ├── css/
│   │   └── style.css
│   ├── js/
│   │   ├── api.js           # API client
│   │   ├── upload.js        # File upload handler
│   │   ├── wizard.js        # Multi-step wizard
│   │   └── results.js       # Results visualization
│   └── index.html
├── templates/              # Jinja2 templates (if using)
│   └── index.html
└── app.py                  # Serve static files (or use FastAPI)
```

### 3. Page Layout (Single Page App)
**index.html**:
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>GPU ML API - Inference Platform</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="/static/css/style.css">
</head>
<body>
    <!-- Navigation -->
    <nav class="navbar navbar-dark bg-dark">
        <div class="container">
            <span class="navbar-brand">GPU ML API</span>
            <span class="badge bg-success">GPU: Connected</span>
        </div>
    </nav>

    <!-- Main Container -->
    <div class="container mt-5">
        <!-- Step Wizard -->
        <div id="wizard">
            <!-- Step 1: Upload Data -->
            <div id="step-upload" class="wizard-step active">
                <h2>Step 1: Upload Dataset</h2>
                <div class="upload-zone" id="dropzone">
                    <i class="bi bi-cloud-upload"></i>
                    <p>Drag & drop CSV file or click to browse</p>
                    <input type="file" id="fileInput" accept=".csv,.xlsx,.json" hidden>
                </div>
                <div id="dataPreview" class="mt-4" style="display:none;">
                    <!-- Table preview -->
                </div>
            </div>

            <!-- Step 2: Select Model -->
            <div id="step-model" class="wizard-step" style="display:none;">
                <h2>Step 2: Select Model</h2>
                <div id="modelGrid" class="row">
                    <!-- Model cards will be populated here -->
                </div>
            </div>

            <!-- Step 3: Configure Features -->
            <div id="step-features" class="wizard-step" style="display:none;">
                <h2>Step 3: Select Features & Target</h2>
                <div class="row">
                    <div class="col-md-8">
                        <h4>Features</h4>
                        <div id="featureCheckboxes">
                            <!-- Auto-populated checkboxes -->
                        </div>
                    </div>
                    <div class="col-md-4">
                        <h4>Target Variable</h4>
                        <select id="targetSelect" class="form-select">
                            <option value="">Select target...</option>
                        </select>
                    </div>
                </div>
                <div class="mt-4">
                    <h4>Preprocessing Options</h4>
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" id="autoPreprocess" checked>
                        <label class="form-check-label" for="autoPreprocess">
                            Automatic preprocessing (recommended)
                        </label>
                    </div>
                </div>
            </div>

            <!-- Step 4: Review & Submit -->
            <div id="step-review" class="wizard-step" style="display:none;">
                <h2>Step 4: Review & Submit</h2>
                <div class="card">
                    <div class="card-body">
                        <h5>Configuration Summary</h5>
                        <ul id="reviewSummary"></ul>
                        <button id="submitBtn" class="btn btn-primary btn-lg">
                            Run Inference
                        </button>
                    </div>
                </div>
            </div>

            <!-- Step 5: Results -->
            <div id="step-results" class="wizard-step" style="display:none;">
                <h2>Results</h2>
                
                <!-- Progress Bar -->
                <div id="progressBar" class="progress mb-4" style="display:none;">
                    <div class="progress-bar progress-bar-striped progress-bar-animated" 
                         role="progressbar" style="width: 100%"></div>
                </div>

                <!-- Results Cards -->
                <div class="row" id="resultsContainer">
                    <!-- Metrics Card -->
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">Performance Metrics</div>
                            <div class="card-body" id="metricsContent"></div>
                        </div>
                    </div>

                    <!-- Feature Importance Card -->
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">Feature Importance</div>
                            <div class="card-body">
                                <canvas id="importanceChart"></canvas>
                            </div>
                        </div>
                    </div>

                    <!-- Predictions Table -->
                    <div class="col-12 mt-4">
                        <div class="card">
                            <div class="card-header">
                                Predictions
                                <button id="downloadBtn" class="btn btn-sm btn-success float-end">
                                    Download CSV
                                </button>
                            </div>
                            <div class="card-body">
                                <table id="predictionsTable" class="table table-striped"></table>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Compute Info -->
                <div class="alert alert-info mt-4" id="computeInfo"></div>

                <!-- Start Over Button -->
                <button id="startOverBtn" class="btn btn-outline-primary mt-3">
                    Run Another Model
                </button>
            </div>
        </div>

        <!-- Navigation Buttons -->
        <div class="wizard-nav mt-4">
            <button id="prevBtn" class="btn btn-secondary" style="display:none;">Previous</button>
            <button id="nextBtn" class="btn btn-primary float-end">Next</button>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
    <script src="/static/js/api.js"></script>
    <script src="/static/js/wizard.js"></script>
</body>
</html>
```

### 4. API Client (api.js)
```javascript
class GPUMLAPIClient {
    constructor(baseURL = 'http://localhost:8000') {
        this.baseURL = baseURL;
        this.dataId = null;
        this.schema = null;
    }

    async uploadData(file) {
        const formData = new FormData();
        formData.append('file', file);
        
        const response = await fetch(`${this.baseURL}/api/v1/data/analyze`, {
            method: 'POST',
            body: formData
        });
        
        if (!response.ok) throw new Error('Upload failed');
        
        this.schema = await response.json();
        return this.schema;
    }

    async listModels() {
        const response = await fetch(`${this.baseURL}/api/v1/models`);
        if (!response.ok) throw new Error('Failed to fetch models');
        return await response.json();
    }

    async predict(modelId, dataId, features, target) {
        const response = await fetch(`${this.baseURL}/api/v1/inference/predict`, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                model_id: modelId,
                data_id: dataId,
                features: features,
                target: target
            })
        });
        
        if (!response.ok) throw new Error('Prediction failed');
        return await response.json();
    }

    async checkHealth() {
        const response = await fetch(`${this.baseURL}/health/gpu`);
        return await response.json();
    }
}
```

### 5. Wizard Logic (wizard.js)
```javascript
class InferenceWizard {
    constructor() {
        this.currentStep = 0;
        this.steps = ['upload', 'model', 'features', 'review', 'results'];
        this.api = new GPUMLAPIClient();
        this.selectedModel = null;
        this.selectedFeatures = [];
        this.targetVariable = null;
        
        this.initEventListeners();
    }

    initEventListeners() {
        // File upload
        const dropzone = document.getElementById('dropzone');
        const fileInput = document.getElementById('fileInput');
        
        dropzone.addEventListener('click', () => fileInput.click());
        fileInput.addEventListener('change', (e) => this.handleFileUpload(e.target.files[0]));
        
        // Navigation
        document.getElementById('nextBtn').addEventListener('click', () => this.nextStep());
        document.getElementById('prevBtn').addEventListener('click', () => this.prevStep());
        
        // Submit
        document.getElementById('submitBtn').addEventListener('click', () => this.runInference());
    }

    async handleFileUpload(file) {
        try {
            const schema = await this.api.uploadData(file);
            this.displayDataPreview(schema);
            this.enableNext();
        } catch (error) {
            alert('Upload failed: ' + error.message);
        }
    }

    displayDataPreview(schema) {
        // Show preview table
        const preview = document.getElementById('dataPreview');
        preview.style.display = 'block';
        
        let html = `<h4>Dataset Preview</h4>`;
        html += `<p>${schema.row_count} rows, ${schema.columns.length} columns</p>`;
        html += `<table class="table table-sm"><thead><tr>`;
        
        schema.columns.forEach(col => {
            html += `<th>${col.name} <small>(${col.type})</small></th>`;
        });
        
        html += `</tr></thead></table>`;
        preview.innerHTML = html;
    }

    async loadModels() {
        const models = await this.api.listModels();
        const grid = document.getElementById('modelGrid');
        
        grid.innerHTML = models.map(model => `
            <div class="col-md-4 mb-3">
                <div class="card model-card" data-model-id="${model.id}">
                    <div class="card-body">
                        <h5>${model.name}</h5>
                        <p class="text-muted">${model.description}</p>
                        <span class="badge bg-primary">${model.type}</span>
                        <span class="badge bg-success">${model.framework}</span>
                    </div>
                </div>
            </div>
        `).join('');
        
        // Click handlers
        document.querySelectorAll('.model-card').forEach(card => {
            card.addEventListener('click', (e) => this.selectModel(e.currentTarget.dataset.modelId));
        });
    }

    populateFeatureCheckboxes() {
        const container = document.getElementById('featureCheckboxes');
        const targetSelect = document.getElementById('targetSelect');
        
        this.api.schema.columns.forEach(col => {
            // Feature checkboxes
            container.innerHTML += `
                <div class="form-check">
                    <input class="form-check-input feature-checkbox" type="checkbox" 
                           value="${col.name}" id="feature_${col.name}" checked>
                    <label class="form-check-label" for="feature_${col.name}">
                        ${col.name} <small class="text-muted">(${col.type})</small>
                    </label>
                </div>
            `;
            
            // Target dropdown
            targetSelect.innerHTML += `<option value="${col.name}">${col.name}</option>`;
        });
    }

    async runInference() {
        document.getElementById('progressBar').style.display = 'block';
        
        try {
            const result = await this.api.predict(
                this.selectedModel,
                this.api.dataId,
                this.selectedFeatures,
                this.targetVariable
            );
            
            this.displayResults(result);
            this.nextStep();
        } catch (error) {
            alert('Inference failed: ' + error.message);
        } finally {
            document.getElementById('progressBar').style.display = 'none';
        }
    }

    displayResults(result) {
        // Metrics
        if (result.metrics) {
            document.getElementById('metricsContent').innerHTML = `
                <h3>${(result.metrics.accuracy * 100).toFixed(2)}%</h3>
                <p class="text-muted">Accuracy</p>
                <p>F1 Score: ${result.metrics.f1_score.toFixed(3)}</p>
            `;
        }
        
        // Feature Importance Chart
        if (result.feature_importance) {
            this.renderImportanceChart(result.feature_importance);
        }
        
        // Predictions Table (first 100 rows)
        this.renderPredictionsTable(result.predictions.slice(0, 100));
        
        // Compute Info
        document.getElementById('computeInfo').innerHTML = `
            Computed in ${result.compute_time_seconds.toFixed(2)}s using 
            ${result.gpu_memory_used_mb.toFixed(0)}MB GPU memory
        `;
    }

    renderImportanceChart(importance) {
        const ctx = document.getElementById('importanceChart');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: Object.keys(importance),
                datasets: [{
                    label: 'Importance',
                    data: Object.values(importance),
                    backgroundColor: 'rgba(54, 162, 235, 0.5)'
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true
            }
        });
    }

    nextStep() {
        // Validation and step transitions
        this.currentStep++;
        this.updateWizard();
    }

    prevStep() {
        this.currentStep--;
        this.updateWizard();
    }

    updateWizard() {
        // Show/hide steps
        document.querySelectorAll('.wizard-step').forEach((step, idx) => {
            step.style.display = idx === this.currentStep ? 'block' : 'none';
        });
        
        // Navigation buttons
        document.getElementById('prevBtn').style.display = this.currentStep > 0 ? 'inline-block' : 'none';
        document.getElementById('nextBtn').style.display = this.currentStep < 4 ? 'inline-block' : 'none';
    }
}

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    new InferenceWizard();
});
```

### 6. Styling (style.css)
```css
:root {
    --primary-color: #0066cc;
    --success-color: #28a745;
    --bg-light: #f8f9fa;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: var(--bg-light);
}

.upload-zone {
    border: 3px dashed #ccc;
    border-radius: 10px;
    padding: 60px 20px;
    text-align: center;
    cursor: pointer;
    transition: all 0.3s;
}

.upload-zone:hover {
    border-color: var(--primary-color);
    background-color: #f0f8ff;
}

.model-card {
    cursor: pointer;
    transition: transform 0.2s, box-shadow 0.2s;
}

.model-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}

.model-card.selected {
    border: 2px solid var(--primary-color);
    background-color: #f0f8ff;
}

.wizard-step {
    min-height: 400px;
}

#predictionsTable {
    max-height: 500px;
    overflow-y: auto;
}
```

### 7. Serve Static Files (FastAPI)
**api/main.py** (extend):
```python
from fastapi.staticfiles import StaticFiles

app.mount("/static", StaticFiles(directory="web/static"), name="static")

@app.get("/")
async def serve_ui():
    return FileResponse("web/static/index.html")
```

---

## Testing Checklist

- [ ] UI loads without errors
- [ ] File upload works (drag & drop and click)
- [ ] Data preview displays correctly
- [ ] Model cards populate from API
- [ ] Can select a model (visual feedback)
- [ ] Feature checkboxes auto-populate
- [ ] Target dropdown auto-populates
- [ ] Review step shows summary
- [ ] Submit triggers inference
- [ ] Results display correctly
- [ ] Charts render (feature importance)
- [ ] Predictions table shows
- [ ] Download CSV works
- [ ] Navigation (Next/Prev) works
- [ ] Mobile responsive

---

## Success Criteria

- [ ] Complete 5-step wizard UI functional
- [ ] Users can upload CSV files
- [ ] Model selection works
- [ ] Feature/target selection intuitive
- [ ] Results are clearly visualized
- [ ] No JavaScript errors in console
- [ ] Mobile-friendly responsive design
- [ ] Error handling with user-friendly messages
- [ ] Documentation/help text where needed

---

## Notes

**Framework**: Vanilla JS + Bootstrap (Phase 4 may upgrade to React/Vue)

**Features**:
- Drag & drop upload
- Model cards with filtering
- Auto-populated feature checkboxes
- Real-time validation
- Chart.js visualizations
- CSV download

**Future**: WebSocket for real-time updates (Phase 2), dark mode, model comparison view

---

## Token Usage

(Track via /log-cost when completed)
