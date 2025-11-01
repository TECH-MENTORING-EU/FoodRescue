# GitHub Copilot & Generative AI Instructions
*(Optimized for GPT-5, Gemini 2.5 Pro, and Copilot Chat in Visual Studio Enterprise — aligned with Blazor Server BFF using cookie-based authentication)*

---

## 1. Meta-Instructions for AI Assistants

### 1.1 General Principles
- Be **concise and specific** — no boilerplate or filler.  
- **Code first**, explanation second.  
- Preserve formatting and indentation exactly.  

### 1.2 Context Awareness
- Architecture = **Clean Architecture + Backend-for-Frontend (BFF)**  
  `UI (Blazor Server) → Application → Infrastructure → External APIs`
- Browser ↔ Server = **secure cookie sessions only** — no tokens in browser.  
- Blazor components never call HTTP or use `fetch()`/JS XHR.  
- All authentication & authorization live **server-side**.

### 1.3 Collaboration Discipline
- **Suggest, don’t apply:** list extra ideas as bullets after code.  
- ≤ 2 paragraphs explanation.  
- The developer is the final authority.

### 1.4 Model-Specific Behavior
**Gemini 2.5 Pro:** ≤ 20 lines explanation.  
**GPT-5:** may include one-sentence rationale before code.  
**Copilot Chat (VS):**
- Target **.NET 9 Blazor Server Interactive**.  
- Keep namespaces aligned to folders.  
- Include only essential `using` statements.  
- Prefer small, context-aware snippets over scaffolds.

---

## 2. Project Overview & Core Principles

You are an expert AI programming assistant.  
Your primary goal → help build a **high-performance, secure, maintainable Blazor Server BFF application**.

### 🔒 Security First
- Auth & authorization = **entirely server-side**.  
- Browser → secure cookies only; tokens never exposed.  
- No local/session storage or client OIDC logic.  
- Always use **Authorization Code Flow + PKCE** with cookie sessions.

### ⚡ Performance by Default
- Use async/await correctly; avoid blocking calls.  
- Employ **Dapper** for parameterized SQL.  
- Enable ADO.NET connection pooling.  
- Keep components small and scoped.

### 🧩 Maintainability & Readability
- Follow **Clean Architecture + SOLID**.  
- Layers: `UI → Application → Infrastructure → Domain`.  
- Keep each method focused; avoid over-abstraction.  
- Prefer explicit, self-documenting code.

### ⚙️ Consistency
- Rules are **absolute**.  
- Always align with **BFF** and cookie-auth model.  
- When unsure → ask, don’t guess.

---

## 3. Security & Authentication (BFF Model)

### 3.1 Authentication Flow
- Pattern: **Backend-for-Frontend (BFF)** using  
  `Microsoft.AspNetCore.Authentication.Cookies` + `OpenIdConnect`.  
- Server issues **secure cookies**; no client-side tokens.  
- Access / refresh tokens → server-side only (Duende).  
- Windows Authentication is allowed for intranet scenarios or App Service Easy Auth
- Tokens never visible in Blazor or JS.

### 3.2 OIDC Configuration
- Use **Authorization Code + PKCE**.  
- Configure in `Program.cs` via OIDC middleware.  
- API calls → `IHttpClientFactory` + Duende token management.  
- Never manually attach `Authorization` headers.

### 3.3 Authorization
- Checks = server-side.  
- `<AuthorizeView>` = UI visibility only.  
- Use `[Authorize(Policy = "...")]` for enforcement.

---

## 4. Blazor Architecture & Safety

### 4.1 Render Mode
- Global: `InteractiveServer`.  
- Never use `InteractiveAuto`, `WebAssembly`, or SSR.

### 4.2 Component Design
- Always use `@key` in `@foreach`.  
- Split markup / logic (`.razor` + `.razor.cs`).  
- Use `EventCallback<T>` for events.  
- Never perform HTTP calls in components.

### 4.3 State Management
- Shared state → Scoped DI services.  
- Subscribe in `OnInitialized`; unsubscribe in `Dispose()`.  
- Call `StateHasChanged()` on state updates.

### 4.4 Circuit Safety
- Avoid static mutable state; prefer scoped services for per-user state.
- Handle disconnects/reconnects gracefully; keep per-user memory bounded.
- Use `ErrorBoundary` around pages; route to `Error.razor` on unhandled errors.

---

## 5. C# and Backend Directives

- PascalCase for public symbols; `_camelCase` for privates.  
- Use explicit types unless obvious.  
- Always use braces.  
- Methods must follow SRP.

### Async Discipline
- `async Task` / `Task<T>` only.  
- No `async void`, `.Result`, or `.Wait()`.

### Dependency Injection
- Constructor DI only.  
- Scoped → repositories/services.  
- Transient → DB connections.  
- Singleton → config/cache.

### Logging & Validation
- Inject `ILogger<T>`.  
- Use FluentValidation.  
- Catch exceptions in services, not UI.

---

## 6. Data Access & Migrations

### 6.1 Dapper
- All queries must be parameterized. No string interpolation or concatenation.
- Use async Dapper methods (`QueryAsync`, `ExecuteAsync`, etc.).  
- Register `IDbConnection` in DI (Transient).  
- Repositories receive connections via DI.  
- Use multi-mapping with `splitOn`.

### 6.2 Migrations (FluentMigrator)
- FluentMigrator C# classes prioritization —  Raw SQL as last resort.  
- Run migrations on startup (`.Migrate()`) in controlled environments.
- Prefer fluent API (`Create.Table()`, `Alter.Column()`).
- A version table is required.
- Prefer migrations for seed data over ad-hoc code.

### 6.3 Data Guardrails
- Dapper: always parameterized, async, and with `CancellationToken`.
- Connection pooling: set `MaxPoolSize`, `MinPoolSize`, and `Pooling=true` in connection strings.
- Transactions are explicit at service boundaries; keep them short-lived. No generic UoW.

---

## 7. Frontend & Accessibility

### 7.1 HTML (Semantic + Secure)
- Use semantic HTML5 elements: `<main>`, `<nav>`, `<header>`, `<footer>`, `<section>`, `<article>`.  
- Use `<button>` for actions — never `<div @onclick>` or fake links.  
- One `<h1>` per page; don’t skip levels.  
- `<div>` / `<span>` → layout only.  
- No `<form action="">`; Blazor handles submissions via events.  
- No hidden fields / manual form tokens — cookies manage session.

### 7.2 CSS Flexbox & Tailwind Layouts
- Flexbox for 1-D; Grid only for 2-D.  
- **Patterns:**  
  - Center → `flex items-center justify-center`  
  - Nav → `flex justify-between items-center`  
  - Cards → `flex flex-wrap gap-4`  
- Use Tailwind responsive prefixes (`sm: md: lg:`); avoid manual media queries.

### 7.3 JavaScript (for Blazor Interop ONLY)
- All JS → `/wwwroot/js/`; no inline `<script>`.  
- Start each file with `'use strict';`.  
- Invoke via `IJSRuntime`.  
- Use `let`/`const`; no `var`.  
- Arrow functions only.  
- **Forbidden:** `fetch()`, XHR, HTTP calls, cookie/token access.  
- JS may only enhance UI (e.g., scroll, focus, clipboard).

### 7.4 Tailwind CSS (Design System)
- `tailwind.config.js` = single source of truth.  
- No magic numbers (`w-[123px]`, `text-[#aabbcc]`).  
- Reference tokens from `theme`; extend with semantic names.  
- Sort classes via `prettier-plugin-tailwindcss`.  
- `content` array must scan `.razor` + `.html`.  
- Prefer utilities over custom CSS.  
- Use class-based dark mode (`dark:`).  
- No inline `style=""`.

### 7.5 Accessibility & UX
- All inputs require `<label for="">`; announce async state changes via live regions.
- Manage focus on navigation and major UI updates.
- Buttons default to `type="button"` unless submitting via Blazor handlers.

### 7.6 AI & Copilot Frontend Rules
- Never generate client-side auth or token logic.  
- Preserve Tailwind order and spacing.  
- Suggest adding tokens to config if missing.  
- Output must be accessible, semantic, responsive.  
- Respect folder structure: `/src/UI/Pages`, `/src/UI/Shared`.

---

## 8. Visual Studio & Tooling

### 8.1 Copilot Chat Integration
- Default context: Blazor Server BFF (.NET 9).  
- Output ≤ 50 lines unless requested.  
- Include file path and layer context.  
- Ask before cross-layer code.  
- Prefer incremental edits.  
- Maintain Tailwind order and HTML semantics.

### 8.2 IDE & Build Configuration
- Enforce analyzers: enable .NET analyzers, nullable, and treat warnings as errors in CI.
- Tailwind: ensure `prettier-plugin-tailwindcss` sorts classes; add `npm run watch` and integrate with __Task Runner Explorer__.

---

## 9. Repository Layout
. -> src/FoodRescue.Web/
  /Authentication → OIDC + Cookie Auth setup, auth services, auth policies, auth attributes
  /Components → Blazor components (.razor, .razor.cs), Blazor Server BFF
  /Models → Entities, Value Objects
  /Repositories → Dapper Repositories, SQL
  /Services → Services, DTOs, Validators
  /Styles → global styles, Tailwind related styles
  /Migrations -> all Migrations (FluentMigrator), FluentMigrator configuration
tests/FoodRescue.Web.Tests/ → unit and integration tests for the Web project
database/ → database setup scripts, seed data, 

---
## 10. Configuration & Environment

### 10.1 Environment & Auth Modes (Repo-Specific)
- Dev: use `DevAuth` (local only). Never ship or enable outside Development.
- Non-dev: current repo uses Windows Authentication (`Negotiate`). If/when moving to OIDC, follow §3 strictly and remove `Negotiate`.
- Do not inject `HttpClient` into `.razor` files. Only repositories/services may call external APIs.
- Seed data must be dev-only and idempotent. Replace startup seeding in `Program.cs` with a dev-only `IHostedService` behind a config flag.

### 10.2 Secrets & Configuration
- Use __Project > Manage User Secrets__ for local secrets. Never commit secrets.
- Configuration precedence: `appsettings.json` → `appsettings.{Environment}.json` → env vars → user secrets.
- Feature flags for dev-only behaviors (e.g., seeding): `FeatureFlags:EnableDevSeeding`.

---

## 11. Outbound HTTP & Resilience

- Use `IHttpClientFactory` with typed clients; configure:
  - Timeouts (≤ 10s), automatic decompression, default headers.
  - Polly policies: retry-with-jitter for transient 5xx/408, circuit breaker, and timeout.
- Attach auth only server-side. Never set `Authorization` headers in UI code.
- Always pass `CancellationToken` from caller → service → HTTP/Dapper.

---

## 12. Error Handling, Logging, Telemetry

- Services map exceptions to domain-specific results; UI never catches.
- Use structured logging (`ILogger<T>`), no PII. Include correlation id.
- Consider Serilog sinks; minimum level Information in prod, Debug in dev.
- Validate inputs with FluentValidation in Application/Service layer only.

---

## 13. Testing Policy

- Unit: services with fakes; avoid over-mocking. Repo tests hit a temp DB/container; do not mock Dapper.
- Integration: `WebApplicationFactory` with a fake auth scheme (no real IdP). Verify auth policies and routes.
- Components: bUnit for `.razor` tests; no HTTP. Use `AuthorizeView` test patterns. Ensure accessibility assertions (labels, roles, tab order).
