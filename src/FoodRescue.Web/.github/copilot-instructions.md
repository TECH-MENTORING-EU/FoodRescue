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

## 4. Blazor Architecture Guidelines

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

## 6. Data Access (Dapper)

- All queries must be parameterized.  
- No string interpolation or concatenation.  
- Use async Dapper methods (`QueryAsync`, `ExecuteAsync`, etc.).  
- Register `IDbConnection` in DI (Transient).  
- Repositories receive connections via DI.  
- Transactions explicit in services (no generic UoW).  
- Use multi-mapping with `splitOn`.

---

## 7. Database Migrations (FluentMigrator)

- FluentMigrator C# classes only — no raw SQL or DbUp.  
- Run migrations on startup (`.Migrate()`).  
- Prefer fluent API (`Create.Table()`, `Alter.Column()`).

---

## 8. Frontend Stack Directives (Blazor Server BFF)

### 8.1 HTML (Semantic + Secure)
- Use semantic HTML5 elements: `<main>`, `<nav>`, `<header>`, `<footer>`, `<section>`, `<article>`.  
- Use `<button>` for actions — never `<div @onclick>` or fake links.  
- One `<h1>` per page; don’t skip levels.  
- `<div>` / `<span>` → layout only.  
- No `<form action="">`; Blazor handles submissions via events.  
- No hidden fields / manual form tokens — cookies manage session.

### 8.2 CSS Flexbox & Tailwind Layouts
- Flexbox for 1-D; Grid only for 2-D.  
- **Patterns:**  
  - Center → `flex items-center justify-center`  
  - Nav → `flex justify-between items-center`  
  - Cards → `flex flex-wrap gap-4`  
- Use Tailwind responsive prefixes (`sm: md: lg:`); avoid manual media queries.

### 8.3 JavaScript (for Blazor Interop ONLY)
- All JS → `/wwwroot/js/`; no inline `<script>`.  
- Start each file with `'use strict';`.  
- Invoke via `IJSRuntime`.  
- Use `let`/`const`; no `var`.  
- Arrow functions only.  
- **Forbidden:** `fetch()`, XHR, HTTP calls, cookie/token access.  
- JS may only enhance UI (e.g., scroll, focus, clipboard).

### 8.4 Tailwind CSS (Design System)
- `tailwind.config.js` = single source of truth.  
- No magic numbers (`w-[123px]`, `text-[#aabbcc]`).  
- Reference tokens from `theme`; extend with semantic names.  
- Sort classes via `prettier-plugin-tailwindcss`.  
  **Example:**  
  - `content` array must scan `.razor` + `.html`.  
- Prefer utilities over custom CSS.  
- Use class-based dark mode (`dark:`).  
- No inline `style=""`.

### 8.5 AI & Copilot Frontend Rules
- Never generate client-side auth or token logic.  
- Preserve Tailwind order and spacing.  
- Suggest adding tokens to config if missing.  
- Output must be accessible, semantic, responsive.  
- Respect folder structure: `/src/UI/Pages`, `/src/UI/Shared`.

---

## 9. Visual Studio Copilot Chat Integration
- Default context: Blazor Server BFF (.NET 9).  
- Output ≤ 50 lines unless requested.  
- Include file path and layer context.  
- Ask before cross-layer code.  
- Prefer incremental edits.  
- Maintain Tailwind order and HTML semantics.

---

## 10. Repository Layout
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
