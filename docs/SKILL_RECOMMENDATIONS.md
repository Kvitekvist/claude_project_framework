# Skill Recommendations for Template Framework

Based on extensive research into AI coding workflows, project automation, and developer productivity patterns in 2026, this document recommends additional skills that would enhance this template for micro solutions and software development.

---

## Research Summary

The research covered six key areas:
1. **AI Coding Workflow Best Practices** - Modern patterns for AI-assisted development
2. **Existing Claude Code Skills** - Community patterns and proven implementations
3. **Project Automation** - Template-based development workflows
4. **Microservices Architecture** - Scaffolding and design patterns
5. **Memory & Context Management** - Persistent knowledge across sessions
6. **CI/CD & Quality Gates** - Automated review, testing, and deployment

---

## Key Findings from Research

### 1. Testing-First Development (2026 Trend)

[Modern AI coding workflows](https://addyosmani.com/blog/ai-coding-workflow/) emphasize that "TDD AI agents only work reliably in codebases that already have functioning test infrastructure." The most successful deployments invest in test infrastructure before unleashing autonomous agents.

**Implication**: Skills that generate tests, scaffold test infrastructure, and verify test coverage are critical.

### 2. Quality Gates Are Essential

[Research shows](https://kilo.ai/articles/beyond-autocomplete) AI-generated PRs wait 4.6x longer in review without governance. Teams winning in 2026 design workflows with execution discipline, not just speed.

**Implication**: Skills for code review automation, PR quality checks, and compliance verification are must-haves.

### 3. Context Management Makes or Breaks AI Assistants

[Industry analysis](https://towardsdatascience.com/why-every-ai-coding-assistant-needs-a-memory-layer/) shows "the difference between a good AI system and a great one often comes down to context management — what you include, what you exclude, how you structure it, when you retrieve it."

**Implication**: Skills that manage project-specific knowledge, API patterns, and architectural decisions are essential.

### 4. Incremental Development Over Big Changes

[Best practices](https://medium.com/@addyosmani/my-llm-coding-workflow-going-into-2026-52fe1681325e) show LLMs perform best with focused, bite-sized tasks. This makes workflows easier to debug and maintain.

**Implication**: Skills that scaffold small, focused components and enforce modular design patterns are valuable.

### 5. Documentation Gaps Block Integration

[ProgrammableWeb surveys](https://buildwithfern.com/post/api-documentation-sdk-generation-tools) found comprehensive documentation is the most influential factor in API selection, surpassing price and performance. Yet only 58% of organizations maintain current docs, creating a 32-point productivity deficit.

**Implication**: Auto-documentation skills that stay synchronized with code are essential.

### 6. Security Scanning Is Non-Negotiable

With [software supply chain attacks on the rise](https://cycode.com/blog/top-enterprise-sca-tools/), teams need automated ways to track and remediate risks in third-party code.

**Implication**: Dependency scanning and vulnerability detection skills should be built-in.

---

## Recommended Skills by Category

### Category 1: Code Quality & Review

#### 1.1 **code-review-ai**
**Purpose**: Automated code review with AI-powered analysis
**Triggers**: "review this code", "check for issues", "PR review"

**What it does**:
- Static analysis for common bugs and anti-patterns
- Security vulnerability detection (SQL injection, XSS, etc.)
- Code style and convention enforcement
- Complexity metrics (cyclomatic complexity, cognitive load)
- Suggests improvements with reasoning

**Integration**: 
- Runs before commits via `definition-of-done` skill
- Can be invoked on specific files or entire PRs
- Outputs markdown report with severity levels

**Why essential**: [Research shows](https://www.augmentcode.com/guides/ai-code-review-ci-cd-pipeline) AI code review in CI/CD pipelines enables early bug detection, improved code quality through standards adherence, and enhanced team collaboration.

**References**:
- [How to Set Up AI Code Review in Your CI/CD Pipeline](https://www.augmentcode.com/guides/ai-code-review-ci-cd-pipeline)
- [Code Review Integration With CI/CD](https://www.meegle.com/en_us/topics/code-review-automation/code-review-integration-with-ci_cd)

---

#### 1.2 **test-generator**
**Purpose**: Automatically generate unit and integration tests
**Triggers**: "generate tests", "add test coverage", "scaffold tests"

**What it does**:
- Analyzes function signatures and generates unit tests
- Creates test fixtures and mock data
- Generates integration tests for API endpoints
- Supports multiple testing frameworks (Jest, pytest, JUnit, etc.)
- Calculates and reports coverage gaps

**Integration**:
- Works with existing code via AST parsing
- Outputs tests in project's existing test directory structure
- Updates test suites when code changes

**Why essential**: [Industry best practices](https://addyosmani.com/blog/ai-coding-workflow/) show testing infrastructure is the foundation for reliable AI-assisted development.

**References**:
- [My LLM coding workflow going into 2026](https://addyosmani.com/blog/ai-coding-workflow/)
- [Best Practices for AI-Assisted Workflow Development](https://medium.com/@sai.kancherla/best-practices-for-ai-assisted-workflow-development-29096fa94d7b)

---

#### 1.3 **refactor-safe**
**Purpose**: Safe refactoring with automated verification
**Triggers**: "refactor this", "extract method", "simplify code"

**What it does**:
- Suggests refactoring opportunities (long methods, duplicated code, etc.)
- Performs common refactorings (extract method, rename, inline, etc.)
- Runs tests before and after to verify behavior unchanged
- Creates atomic commits per refactoring step
- Generates refactoring documentation

**Integration**:
- Integrates with test suite to verify safety
- Creates individual commits per refactoring
- Updates architecture.md if structure changes

**Why essential**: Refactoring is mentioned in PROJECT_RULES.md but lacks automated support.

---

### Category 2: Testing & Quality Assurance

#### 2.1 **test-coverage**
**Purpose**: Analyze and improve test coverage
**Triggers**: "check coverage", "coverage report", "what needs tests"

**What it does**:
- Runs coverage analysis on test suite
- Identifies uncovered code paths
- Generates coverage reports (HTML, JSON, text)
- Suggests which functions need tests most urgently (based on complexity/criticality)
- Tracks coverage trends over time

**Integration**:
- Hooks into `definition-of-done` to block commits with dropping coverage
- Updates project_status.md with current coverage %
- Can fail if coverage drops below threshold

**Why essential**: [Testing best practices](https://testquality.com/best-automation-testing-tools-for-ci-cd-pipelines-your-complete-2025-guide/) emphasize coverage as a key quality metric.

**References**:
- [Best Automation Testing Tools for CI/CD Pipelines](https://testquality.com/best-automation-testing-tools-for-ci-cd-pipelines-your-complete-2025-guide/)
- [CI/CD-Friendly Testing: QA Pipeline in 24 Hours](https://cloudqa.io/ci-cd-testing-automation-guide/)

---

#### 2.2 **smoke-test**
**Purpose**: Quick end-to-end smoke tests
**Triggers**: "smoke test", "quick verify", "sanity check"

**What it does**:
- Runs critical path tests only (under 5 minutes)
- Tests basic functionality (app starts, DB connects, API responds)
- Can run against local, staging, or production
- Reports pass/fail with minimal detail
- Integrates with deployment pipelines

**Integration**:
- Runs automatically before creating releases
- Can be triggered via webhook for deployment verification
- Blocks deployment if smoke tests fail

**Why essential**: [CI/CD patterns](https://graphite.com/guides/role-code-review-ci-cd) show rapid smoke tests before merges are critical.

**References**:
- [The role of code review in CI/CD pipelines](https://graphite.com/guides/role-code-review-ci-cd)

---

### Category 3: Security & Compliance

#### 3.1 **security-scan**
**Purpose**: Dependency vulnerability scanning and security checks
**Triggers**: "security scan", "check vulnerabilities", "audit dependencies"

**What it does**:
- Scans package.json/requirements.txt/go.mod for known CVEs
- Checks for outdated dependencies with security patches
- License compliance verification
- Secret detection in code/commits
- Generates security report with remediation steps

**Integration**:
- Runs on every dependency change
- Blocks commits if critical vulnerabilities found
- Updates project_status.md with security score
- Can auto-create tickets for vulnerabilities

**Why essential**: [Modern security practices](https://cycode.com/blog/top-enterprise-sca-tools/) require automated dependency scanning in all projects.

**References**:
- [Top 21 Enterprise SCA Tools for 2026](https://cycode.com/blog/top-enterprise-sca-tools/)
- [Top Open Source Dependency Scanners in 2026](https://www.aikido.dev/blog/top-open-source-dependency-scanners)
- [How to Handle Dependency Vulnerability Scanning](https://oneuptime.com/blog/post/2026-01-24-dependency-vulnerability-scanning/view)

---

#### 3.2 **secrets-detect**
**Purpose**: Prevent secrets from being committed
**Triggers**: "check for secrets", "scan commits", "pre-commit hook"

**What it does**:
- Scans staged files for API keys, passwords, tokens
- Checks commit history for accidentally committed secrets
- Validates .gitignore covers common secret files
- Suggests using environment variables or secret managers
- Can auto-redact found secrets

**Integration**:
- Runs as pre-commit hook
- Blocks commits containing secrets
- Updates .gitignore automatically
- Integrates with definition-of-done

**Why essential**: Secret leaks are a common security vulnerability that's easily prevented.

---

### Category 4: Documentation & Knowledge

#### 4.1 **api-doc-generator**
**Purpose**: Auto-generate API documentation
**Triggers**: "generate API docs", "update docs", "create OpenAPI spec"

**What it does**:
- Extracts API routes from code (Express, Flask, FastAPI, etc.)
- Generates OpenAPI/Swagger specifications
- Creates markdown documentation from code comments
- Generates interactive API documentation (like Swagger UI)
- Keeps docs synchronized with code

**Integration**:
- Runs on API code changes
- Outputs to docs/api/ directory
- Can deploy to static site
- Updates when routes/parameters change

**Why essential**: [Documentation research](https://buildwithfern.com/post/api-documentation-sdk-generation-tools) shows 73% of API integrations are blocked by documentation gaps.

**References**:
- [API Docs & SDK Generation Tools](https://buildwithfern.com/post/api-documentation-sdk-generation-tools)
- [Auto Generate API Documentation: A Developer's Guide](https://www.docuwriter.ai/posts/auto-generate-api-documentation)
- [15 Tools to Automate API Docs Generations](https://apidog.com/blog/automate-api-docs/)

---

#### 4.2 **architecture-document**
**Purpose**: Generate and update architecture documentation
**Triggers**: "document architecture", "update architecture", "generate diagrams"

**What it does**:
- Analyzes codebase structure and generates architecture diagrams
- Creates component interaction diagrams
- Documents data flow and dependencies
- Generates C4 model diagrams (Context, Container, Component, Code)
- Keeps architecture.md synchronized with actual code

**Integration**:
- Runs when major structural changes detected
- Updates .claude/memory/architecture.md
- Generates diagrams in docs/architecture/
- Can detect architectural violations

**Why essential**: Architecture documentation often lags behind actual code; automation keeps them synchronized.

---

#### 4.3 **readme-generator**
**Purpose**: Generate and maintain README.md
**Triggers**: "generate README", "update README", "improve docs"

**What it does**:
- Analyzes project structure and generates README sections
- Includes installation instructions, usage examples
- Auto-detects tech stack and lists dependencies
- Generates badges (build status, coverage, version)
- Keeps README synchronized with package.json/setup.py

**Integration**:
- Runs when project structure changes
- Updates README.md sections automatically
- Preserves custom content between markers
- Validates all links still work

**Why essential**: README is the first impression; keeping it current improves onboarding.

---

### Category 5: Project Scaffolding & Templates

#### 5.1 **component-scaffold**
**Purpose**: Generate components following project conventions
**Triggers**: "scaffold component", "create new feature", "add module"

**What it does**:
- Generates component files from templates
- Follows project's existing patterns and conventions
- Creates matching test files
- Updates index/exports automatically
- Supports multiple component types (React, Vue, API routes, etc.)

**Integration**:
- Reads coding_conventions.md for style
- Follows architecture.md for structure
- Creates files in correct locations
- Generates initial tests

**Why essential**: [Scaffolding best practices](https://www.opslevel.com/resources/cookiecutter-vs-yeoman-choosing-the-right-scaffolder-for-your-service) show templates reduce dev time and enforce standards.

**References**:
- [Cookiecutter vs. Yeoman: choosing the right scaffolder](https://www.opslevel.com/resources/cookiecutter-vs-yeoman-choosing-the-right-scaffolder-for-your-service)
- [Cookiecutter alternatives](https://safjan.com/cookiecutter-alternatives/)

---

#### 5.2 **microservice-scaffold**
**Purpose**: Generate complete microservice from template
**Triggers**: "create microservice", "new service", "scaffold service"

**What it does**:
- Generates complete microservice structure
- Includes Dockerfile, docker-compose, CI/CD configs
- Creates API routes, database models, tests
- Sets up logging, monitoring, health checks
- Follows 12-factor app principles

**Integration**:
- Uses templates from .claude/templates/microservice/
- Follows project's tech stack
- Updates root documentation with new service
- Can create in separate repo or monorepo

**Why essential**: [Microservices patterns](https://dzone.com/articles/implementing-microservices-the-foundations) emphasize templates for standardization.

**References**:
- [The Principles of Planning and Implementing Microservices](https://dzone.com/articles/implementing-microservices-the-foundations)
- [14 Microservice Best Practices For Your Projects](https://www.simform.com/blog/microservice-best-practices/)

---

### Category 6: Database & Migrations

#### 6.1 **migration-generator**
**Purpose**: Generate safe database migrations
**Triggers**: "create migration", "alter table", "add column"

**What it does**:
- Generates migration files from schema changes
- Creates both up and down migrations
- Validates migrations won't cause data loss
- Checks for common migration pitfalls (missing indexes, etc.)
- Generates test data for migration verification

**Integration**:
- Integrates with project's migration tool (Flyway, Alembic, etc.)
- Creates timestamped migration files
- Updates schema documentation
- Runs migrations in transaction when safe

**Why essential**: [Migration best practices](https://www.liquibase.com/resources/guides/database-schema-migration) emphasize automation and safety checks.

**References**:
- [Database Schema Migration: Understand, Optimize, Automate](https://www.liquibase.com/resources/guides/database-schema-migration)
- [Strategies for Reliable Schema Migrations](https://atlasgo.io/blog/2024/10/09/strategies-for-reliable-migrations)
- [Best Practices for Database Schema Migrations](https://dev.to/jefersoneiji/best-practices-for-database-schema-migrations-in-large-systems-4nl9)

---

#### 6.2 **schema-validator**
**Purpose**: Validate database schema against best practices
**Triggers**: "validate schema", "check database", "schema review"

**What it does**:
- Checks for missing indexes on foreign keys
- Validates naming conventions (tables, columns)
- Detects overly wide columns (VARCHAR(MAX), etc.)
- Identifies unused indexes
- Suggests performance optimizations

**Integration**:
- Runs on migration creation
- Can block merges if violations found
- Generates schema documentation
- Updates architecture.md with database design

**Why essential**: Schema design impacts performance; automated validation prevents common mistakes.

---

### Category 7: Performance & Monitoring

#### 7.1 **performance-profile**
**Purpose**: Profile application performance
**Triggers**: "profile performance", "find bottlenecks", "analyze speed"

**What it does**:
- Runs performance profiling on code
- Identifies slow functions and queries
- Generates flame graphs and reports
- Compares performance across commits
- Suggests optimization opportunities

**Integration**:
- Can run on specific endpoints/functions
- Tracks performance metrics over time
- Alerts on performance regressions
- Updates project_status.md with benchmarks

**Why essential**: [Performance monitoring](https://www.hud.io/blog/top-application-monitoring-tools/) is critical for production applications.

**References**:
- [Top 12 Application Performance Monitoring Tools for 2026](https://www.hud.io/blog/top-application-monitoring-tools/)
- [Top 14 Performance Profiling Tools In 2026](https://startupstash.com/performance-profiling-tools/)

---

#### 7.2 **benchmark-runner**
**Purpose**: Run and track performance benchmarks
**Triggers**: "run benchmarks", "performance test", "speed check"

**What it does**:
- Runs predefined benchmark suites
- Tracks benchmark results over time
- Compares current vs baseline performance
- Fails if performance degrades significantly
- Generates performance reports

**Integration**:
- Runs on every commit or PR
- Stores results in project_status.md
- Can block merges on regressions
- Generates trend graphs

**Why essential**: Continuous performance tracking prevents gradual degradation.

---

### Category 8: CI/CD & Deployment

#### 8.1 **ci-config-generator**
**Purpose**: Generate CI/CD pipeline configurations
**Triggers**: "setup CI", "create pipeline", "configure GitHub Actions"

**What it does**:
- Generates CI config files (GitHub Actions, GitLab CI, etc.)
- Creates workflows for test/build/deploy
- Sets up branch protection rules
- Configures automated releases
- Includes security scans and quality gates

**Integration**:
- Detects project type and tech stack
- Creates .github/workflows/ files
- Follows CI/CD best practices
- Integrates with existing skills (test-coverage, security-scan)

**Why essential**: [CI/CD patterns](https://dev.to/pockit_tools/ai-code-review-in-your-cicd-pipeline-automating-pr-reviews-test-generation-and-bug-detection-56j4) show proper pipeline setup is foundational.

**References**:
- [AI Code Review in Your CI/CD Pipeline](https://dev.to/pockit_tools/ai-code-review-in-your-cicd-pipeline-automating-pr-reviews-test-generation-and-bug-detection-56j4)
- [How to Automate Code Reviews Using GitHub Actions](https://github.com/orgs/community/discussions/178963)

---

#### 8.2 **release-prepare**
**Purpose**: Prepare releases with changelogs and versioning
**Triggers**: "prepare release", "create release", "bump version"

**What it does**:
- Bumps version following semver
- Generates release notes from commits/tickets
- Updates CHANGELOG.md automatically
- Creates git tags and GitHub releases
- Validates release is ready (tests pass, docs updated)

**Integration**:
- Extends existing changelog-append skill
- Uses ticket_memory.md for release notes
- Follows existing version.txt convention
- Can trigger deployment workflows

**Why essential**: Already partially covered by release.md prompt, but automation reduces manual work.

---

### Category 9: Dependency Management

#### 9.1 **dependency-update**
**Purpose**: Smart dependency updates
**Triggers**: "update dependencies", "check outdated", "upgrade packages"

**What it does**:
- Checks for outdated dependencies
- Prioritizes security patches
- Tests updates before committing
- Creates separate PRs per dependency
- Generates compatibility reports

**Integration**:
- Runs weekly via cron/GitHub Actions
- Auto-creates tickets for breaking changes
- Tests updates against test suite
- Updates dependency documentation

**Why essential**: [Dependency management](https://securityboulevard.com/2026/07/dependency-management-tools-key-features-and-6-tools-to-know-in-2026/) is critical for security and stability.

**References**:
- [Dependency management tools: Key features and 6 tools to know in 2026](https://securityboulevard.com/2026/07/dependency-management-tools-key-features-and-6-tools-to-know-in-2026/)

---

#### 9.2 **license-check**
**Purpose**: Verify dependency licenses are compatible
**Triggers**: "check licenses", "license audit", "compliance check"

**What it does**:
- Scans all dependencies for licenses
- Flags incompatible licenses (GPL in commercial project, etc.)
- Generates license report
- Tracks license changes over time
- Suggests alternatives for problematic dependencies

**Integration**:
- Runs on dependency changes
- Can block commits with license violations
- Updates documentation with license info
- Integrates with security-scan

**Why essential**: License compliance is often overlooked until it becomes a legal issue.

---

### Category 10: Project Health & Metrics

#### 10.1 **health-check**
**Purpose**: Comprehensive project health assessment
**Triggers**: "health check", "project status", "how are we doing"

**What it does**:
- Analyzes code quality metrics
- Checks test coverage, documentation coverage
- Reviews dependency freshness
- Assesses technical debt
- Generates project health report card

**Integration**:
- Updates project_status.md automatically
- Runs weekly and on-demand
- Tracks trends over time
- Can create tickets for issues found

**Why essential**: Provides high-level view of project quality and areas needing attention.

---

#### 10.2 **technical-debt-tracker**
**Purpose**: Identify and track technical debt
**Triggers**: "find tech debt", "debt analysis", "code smells"

**What it does**:
- Scans for TODO/FIXME/HACK comments
- Identifies code smells (long functions, high complexity)
- Tracks debt over time
- Estimates effort to resolve
- Creates tickets for debt items

**Integration**:
- Updates project_memory.md Technical Debt section
- Runs on every commit
- Generates debt reports
- Prioritizes debt by impact

**Why essential**: Technical debt is mentioned in project_memory.md but lacks automated tracking.

---

## Priority Implementation Roadmap

### Phase 1: Foundation (Immediate)
**Rationale**: Build quality gates and testing infrastructure first

1. **test-generator** - Testing is the foundation for AI-assisted development
2. **security-scan** - Security cannot be an afterthought
3. **code-review-ai** - Early quality feedback prevents issues
4. **definition-of-done** enhancement - Already exists, enhance with new skills

**Estimated effort**: 2-3 weeks
**Impact**: High - Enables safe autonomous development

---

### Phase 2: Documentation & Knowledge (Next)
**Rationale**: Keep documentation synchronized with code

5. **api-doc-generator** - Documentation gaps block integration
6. **readme-generator** - Improve onboarding
7. **architecture-document** - Keep architecture current

**Estimated effort**: 1-2 weeks
**Impact**: Medium-High - Reduces onboarding friction

---

### Phase 3: Scaffolding & Productivity (Then)
**Rationale**: Speed up common development tasks

8. **component-scaffold** - Enforce conventions automatically
9. **migration-generator** - Make DB changes safer
10. **ci-config-generator** - Standardize pipelines

**Estimated effort**: 2 weeks
**Impact**: Medium - Accelerates development

---

### Phase 4: Advanced Quality (Later)
**Rationale**: Deeper quality and performance insights

11. **test-coverage** - Visibility into quality
12. **performance-profile** - Prevent performance regressions
13. **refactor-safe** - Safe code improvement

**Estimated effort**: 2-3 weeks
**Impact**: Medium - Improves code quality

---

### Phase 5: Maintenance & Health (Ongoing)
**Rationale**: Long-term project sustainability

14. **dependency-update** - Keep dependencies current
15. **health-check** - High-level project visibility
16. **technical-debt-tracker** - Prevent debt accumulation

**Estimated effort**: 1-2 weeks
**Impact**: Low-Medium - Sustains project health

---

## Integration with Existing Skills

The new skills should integrate seamlessly with the second brain system:

### With `context-load`
- Skills can read project-specific patterns from memory
- Skills can update memory with findings (security issues, debt, etc.)

### With `definition-of-done`
- New quality checks hook into pre-commit verification
- Tests must pass, security scans clear, coverage maintained

### With `new-ticket`
- Skills can auto-create tickets for found issues
- Security vulnerabilities → Bug tickets
- Tech debt → Enhancement tickets

### With `log-cost`
- Track token costs of skill executions
- Optimize expensive operations

### With `changelog-append`
- Auto-document skill-driven improvements
- Link changes to tickets

---

## Implementation Guidelines

### Skill Structure Template

```markdown
---
name: skill-name
description: One-line description of what this skill does
version: 1.0.0
triggers: "keyword1", "keyword2", "phrase to trigger"
---

# Skill Purpose

Clear explanation of what problem this solves and when to use it.

## How It Works

1. Step-by-step workflow
2. What it analyzes
3. What it outputs
4. How it integrates

## Usage Examples

```bash
/skill-name [options]
```

## Configuration

Optional project-specific settings in project_config.md

## Integration Points

- Reads: architecture.md, coding_conventions.md
- Updates: project_status.md, ticket_memory.md
- Triggers: Other skills that depend on this

## Output Format

What the skill produces (reports, code, configs, etc.)
```

---

## Best Practices for Skill Development

1. **Single Responsibility**: Each skill does one thing well
2. **Composable**: Skills work together (test-generator + test-coverage)
3. **Idempotent**: Running twice produces same result
4. **Fast Feedback**: Quick results, detailed analysis optional
5. **Configurable**: Project-specific settings in project_config.md
6. **Memory-Aware**: Uses second brain memory for context
7. **Token-Efficient**: Targeted analysis, not whole-codebase scans
8. **Well-Documented**: Clear usage examples and integration points

---

## Metrics for Success

Track these metrics to measure skill effectiveness:

- **Time saved**: Compare manual vs automated task time
- **Quality improved**: Bugs caught, test coverage increase
- **Developer satisfaction**: Survey team on skill usefulness
- **Adoption rate**: How often skills are used
- **Token efficiency**: Cost per skill execution
- **False positive rate**: Accuracy of automated checks

---

## Community Skills to Consider

Based on [existing Claude Code skills](https://github.com/alirezarezvani/claude-skills) (345 skills) and [community patterns](https://suhasbhairav.com/ai-skills), consider adopting proven patterns:

### From Antigravity Awesome Skills
- `/review` - Code review with context
- `/debug` - Intelligent debugging assistance
- `/perf` - Performance analysis
- `/security` - Security audit

### From Community Libraries
- Architecture brainstorming
- API design patterns
- PR description generation
- Documentation style enforcement

**Recommendation**: Study existing implementations before building from scratch.

**References**:
- [AI Skills Library](https://suhasbhairav.com/ai-skills)
- [345 Claude Code skills & agent skills](https://github.com/alirezarezvani/claude-skills)
- [240+ Claude Code skills converted from Cursor rules](https://github.com/Mindrally/skills)

---

## Conclusion

This research identified **22 high-value skills** across 10 categories that would significantly enhance this template for micro solutions and software development:

**Immediate priorities** (Phase 1):
- test-generator
- security-scan  
- code-review-ai

These three skills provide the foundation for safe, high-quality AI-assisted development and should be implemented first.

The phased roadmap ensures the template evolves systematically, building quality infrastructure before productivity accelerators, and maintaining a balance between automation and control.

---

## Sources

### AI Coding Workflows
- [My LLM coding workflow going into 2026](https://addyosmani.com/blog/ai-coding-workflow/)
- [Building Reliable AI Coding Workflows](https://techcommunity.microsoft.com/blog/educatordeveloperblog/building-reliable-ai-coding-workflows-using-modular-ai-agent-optimization/4523252)
- [Beyond Autocomplete: Best Agentic Coding Workflow in 2026](https://kilo.ai/articles/beyond-autocomplete)
- [Best Practices for AI-Assisted Workflow Development](https://medium.com/@sai.kancherla/best-practices-for-ai-assisted-workflow-development-29096fa94d7b)

### Existing Skills & Templates
- [AI Skills Library](https://suhasbhairav.com/ai-skills)
- [10 Must-Have Skills for Claude](https://medium.com/@unicodeveloper/10-must-have-skills-for-claude-and-any-coding-agent-in-2026-b5451b013051)
- [345 Claude Code skills](https://github.com/alirezarezvani/claude-skills)
- [Claude Skills Guide](https://design.dev/guides/claude-skills/)

### Security & Dependencies
- [Top 21 Enterprise SCA Tools for 2026](https://cycode.com/blog/top-enterprise-sca-tools/)
- [Top Open Source Dependency Scanners](https://www.aikido.dev/blog/top-open-source-dependency-scanners)
- [How to Handle Dependency Vulnerability Scanning](https://oneuptime.com/blog/post/2026-01-24-dependency-vulnerability-scanning/view)
- [Dependency management tools: 6 tools to know in 2026](https://securityboulevard.com/2026/07/dependency-management-tools-key-features-and-6-tools-to-know-in-2026/)

### Documentation
- [API Docs & SDK Generation Tools](https://buildwithfern.com/post/api-documentation-sdk-generation-tools)
- [Auto Generate API Documentation](https://www.docuwriter.ai/posts/auto-generate-api-documentation)
- [15 Tools to Automate API Docs](https://apidog.com/blog/automate-api-docs/)

### Testing & CI/CD
- [Best Automation Testing Tools for CI/CD Pipelines](https://testquality.com/best-automation-testing-tools-for-ci-cd-pipelines-your-complete-2025-guide/)
- [How to Set Up AI Code Review in Your CI/CD Pipeline](https://www.augmentcode.com/guides/ai-code-review-ci-cd-pipeline)
- [AI Code Review in Your CI/CD Pipeline](https://dev.to/pockit_tools/ai-code-review-in-your-cicd-pipeline-automating-pr-reviews-test-generation-and-bug-detection-56j4)
- [The role of code review in CI/CD pipelines](https://graphite.com/guides/role-code-review-ci-cd)

### Database Migrations
- [Database Schema Migration](https://www.liquibase.com/resources/guides/database-schema-migration)
- [Strategies for Reliable Schema Migrations](https://atlasgo.io/blog/2024/10/09/strategies-for-reliable-migrations)
- [Best Practices for Database Schema Migrations](https://dev.to/jefersoneiji/best-practices-for-database-schema-migrations-in-large-systems-4nl9)

### Microservices & Scaffolding
- [The Principles of Planning and Implementing Microservices](https://dzone.com/articles/implementing-microservices-the-foundations)
- [14 Microservice Best Practices](https://www.simform.com/blog/microservice-best-practices/)
- [Cookiecutter vs. Yeoman](https://www.opslevel.com/resources/cookiecutter-vs-yeoman-choosing-the-right-scaffolder-for-your-service)
- [Cookiecutter alternatives](https://safjan.com/cookiecutter-alternatives/)

### Performance & Monitoring
- [Top 12 Application Performance Monitoring Tools](https://www.hud.io/blog/top-application-monitoring-tools/)
- [Top 14 Performance Profiling Tools](https://startupstash.com/performance-profiling-tools/)

### Memory & Context Management
- [Why Every AI Coding Assistant Needs a Memory Layer](https://towardsdatascience.com/why-every-ai-coding-assistant-needs-a-memory-layer/)
- [AI agent memory: types, architecture & implementation](https://redis.io/blog/ai-agent-memory-stateful-systems/)
- [5 Architectural Patterns for Persistent Memory](https://machinelearningmastery.com/5-architectural-patterns-for-persistent-memory-and-state-in-ai-agents/)
- [Context Management for Agentic AI](https://medium.com/@hungry.soul/context-management-a-practical-guide-for-agentic-ai-74562a33b2a5)
