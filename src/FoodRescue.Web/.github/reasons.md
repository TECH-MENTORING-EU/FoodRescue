Check the documentation for the reasons behind the copilot-instructions.md file.
https://docs.google.com/document/d/1eZ9jrDWmLfBeKpzPJ6W3WVyquGIPQP5EP8f6CyiQn6g/edit?usp=sharing

## below sumarized set of rules

> The following guidance defines how Copilot, GPT-5, and Gemini 2.5 Pro must operate inside this repository.

### General Behavior
- Prioritize **concise, secure, high-performance** code generation for a **Blazor Server BFF (.NET 9)** app.  
- Focus on correctness, security, and maintainability — no boilerplate or filler.  
- Always produce **diff-style edits** or minimal snippets.  
- When the intent or context is unclear → **ask before coding**.  
- Keep output within ~50 lines unless explicitly expanded.

### Context Awareness
- Architecture: **Blazor Server Backend-for-Frontend (BFF)**  
- Auth: **Cookie-based OIDC (Authorization Code + PKCE)** using **Duende.AccessTokenManagement**.  
- Data: **SQL Server via Dapper**, migrations via **FluentMigrator**.  
- Frontend: **TailwindCSS**, semantic HTML5, no inline scripts or client-side HTTP.  
- Validation: **FluentValidation**; Logging: `ILogger<T>` through DI.

### Coding Discipline
- Use `async/await`; no blocking calls (`.Result`, `.Wait()`, `async void`).  
- Keep formatting and indentation unchanged.  
- Follow Clean Architecture strictly; no cross-layer logic.  
- Use constructor DI; no static service logic.  
- Always parameterize SQL; forbid EF Core usage.  
- JS interop via `IJSRuntime` only; never embed `<script>` in `.razor` or `.html`.

### Output & Tone
- **Code first**, short rationale after if needed.  
- Responses should be **technical, direct, and concise**.  
- Include file path or layer context when modifying code.  
- Preserve whitespace and indentation.  
- Use Markdown code fences with proper language tags.

### Frontend / Tailwind
- Enforce Tailwind’s standard class order (Prettier plugin).  
- No magic numeric values — use theme tokens from `tailwind.config.js`.  
- Prefer responsive prefixes (`sm: md: lg:`).  
- Use dark-mode via `class` strategy.  
- Never inline `style=""`.

### Safety Rules
- Never expose or manipulate tokens client-side.  
- Never use `fetch()` or XHR from browser code.  
- Never store anything in `localStorage` or `sessionStorage`.  
- Never include hidden form fields for auth.  
- Never access cookies from JS.

### Session Behavior
- Retain project context for ~10 chat turns; re-state architecture after resets.  
- Remind the **Blazor Server BFF architecture** every few prompts.  
- Auto-reassert these rules if context appears lost or conversation reconnects.

---

> TL;DR — Think in **Clean Architecture**, act as a **secure Blazor Server BFF backend expert**, and never generate unsafe or client-side auth logic.
