# Project Memory

This file represents the long-term memory of the project.

Update continuously.

---

## Project Vision

Build a production-ready GPU-accelerated ML inference API that makes it trivially easy for consumers to run ML models on datasets.

**Core Value Proposition**: Connect to API, pick model from dropdown, select features/target via checkboxes, submit job, get predictions. Simple as that.

**Target Hardware**: RTX 4080 Super (16GB VRAM)

**Long-term Goal**: Enterprise-grade ML inference platform with AutoML, explainability, multi-GPU support, and comprehensive developer tools.

---

## Current Milestone

**Phase 1: Foundation (MVP)** - Build working inference pipeline

**Target**: Users can upload CSV, select model, choose features/target, and get predictions with metrics.

**Tickets**: TICKET-0002 through TICKET-0007 (6 tickets)

**Current Focus**: Starting TICKET-0002 (Core API Infrastructure Setup)

---

## Active Priorities

1. **Complete Phase 1 MVP** (TICKET-0002 through TICKET-0007)
   - Core API infrastructure with GPU support
   - Model management system
   - Data ingestion and schema detection
   - Inference pipeline with preprocessing
   - Basic web UI
   - Authentication and security

2. **Documentation First**
   - Comprehensive design doc created: `docs/GPU_ML_API_DESIGN.md`
   - All Phase 1 tickets have detailed implementation plans
   - Phase 2-5 tickets created as placeholders

3. **Follow Phased Approach**
   - Phase 1: MVP (immediate)
   - Phase 2: Production readiness (job queues, monitoring)
   - Phase 3: Advanced ML features (AutoML, explainability)
   - Phase 4: Developer experience (SDK, CLI)
   - Phase 5: Scale & optimization (multi-GPU, compliance)

---

## Technical Debt

None yet (greenfield project).

**Future Considerations**:
- Phase 1 uses SQLite (migrate to PostgreSQL in Phase 2)
- Phase 1 uses local file storage (migrate to MinIO/S3 in Phase 2)
- Basic preprocessing only (advanced pipelines in Phase 2)

---

## Known Issues

None yet (project just started).

---

## Future Ideas

**Phase 2-5 Roadmap** (see TICKET-0001 for full breakdown):

- Job queue system with Redis/Celery
- Multiple data sources (SQL, S3, APIs, Google Sheets)
- Advanced preprocessing and feature engineering
- Model versioning and A/B testing
- Prometheus/Grafana monitoring
- AutoML capabilities
- Model explainability (SHAP, LIME)
- Deep learning, NLP, time series models
- Python SDK and CLI tool
- Jupyter integration
- Multi-GPU support
- Cost tracking and billing
- GDPR compliance tools

**Beyond the Roadmap**:
- Model marketplace (share/download models)
- Federated learning support
- Edge deployment (mobile, IoT)
- TPU support
- Differential privacy

---

## Development Patterns

### User Work Style

**Autonomous Implementation Preference**: User prefers minimal clarifying questions for infrastructure/system tasks. When given clear directive ("make a ticket subfolder structure system"), proceed with full implementation using best practices without seeking approval for architectural decisions. Applies to: infrastructure improvements, tooling, system organization, documentation structure.

### Phased Implementation

For large feature sets (10+ items), use phased implementation:

**Phase 1: Foundation** (Implement immediately)
- Highest priority items
- Items other phases depend on
- Core infrastructure
- Quality gates

**Phases 2-5: Roadmap** (Document, implement iteratively)
- Build on Phase 1 foundations
- Each phase has clear theme/purpose
- Dependencies flow downward (Phase N needs Phase N-1)

**Rationale**: 
- Prevents overwhelming scope
- Allows testing foundations before scaling
- Enables learning from early phases
- Follows expert pattern: Context → Connections → Capabilities → Cadence

**Example**: 28 skills → Phase 1 (6 foundation skills) implemented, 22 documented for future phases. Each phase builds quality infrastructure before productivity tools.

---

## Notes

General development notes.
