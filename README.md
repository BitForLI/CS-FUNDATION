# CS FUNDATION

> A bilingual, runnable map from computer-science fundamentals to a modern full-stack application.  
> 一套中英文对照、可以实际运行的计算机基础与现代全栈学习地图。

The repository name intentionally follows the requested spelling: `CS-FUNDATION`.

## What this repository teaches / 学什么

This is not a collection of disconnected snippets. Each example answers four questions:

1. **Layer / 层次** — where the concept lives in a real system.
2. **Use case / 使用场景** — when it is appropriate.
3. **Effect / 作用** — which problem it solves.
4. **Runnable path / 可运行路径** — the smallest command that proves the idea.

## Prerequisites / 环境

- .NET SDK 8
- Node.js 22.18 or newer and npm (the TypeScript event-loop example uses Node's built-in type stripping)
- A browser
- Optional: Docker for the container lesson

Verify your environment:

```bash
dotnet --version
node --version
npm --version
```

## Knowledge map / 知识地图

| Layer / 层次 | Topic / 知识点 | When and why / 何时使用与作用 | Runnable path / 最小代码路径 |
|---|---|---|---|
| Language | C# values, control flow, functions, records, interfaces, generics, LINQ, exceptions | Build type-safe domain and backend code / 编写类型安全的领域与后端代码 | `examples/csharp/01-language-basics/` |
| Language | TypeScript objects, unions, narrowing, generics, promises | Build type-safe browser code / 编写类型安全的浏览器代码 | `examples/typescript/` |
| Algorithms | Big-O, stack, queue, dictionary, binary search, graph BFS | Choose data structures by access pattern / 根据访问模式选择数据结构 | `examples/csharp/02-data-structures/` |
| Interview algorithms | Hash lookup, sliding window, stack, binary-search boundary | Explain invariant, complexity and edge cases / 说明不变量、复杂度和边界 | `examples/csharp/08-interview-algorithms/` |
| Runtime | Async/await, cancellation, race conditions, locking, channels | Keep services responsive and coordinate concurrent work / 保持服务响应并协调并发任务 | `examples/csharp/03-async-concurrency/` |
| Runtime semantics | Value/reference behavior, equality, resource disposal | Diagnose aliasing and lifecycle bugs / 排查别名与资源生命周期问题 | `examples/csharp/09-runtime-semantics/` |
| OS boundary | Files, streams, JSON, disposal | Persist or exchange small local datasets / 持久化和交换小型本地数据 | `examples/csharp/04-file-io-json/` |
| Network | TCP client/server, framing, bytes, ports | Understand what HTTP runs on top of / 理解 HTTP 下方的网络机制 | `examples/csharp/05-networking/` |
| Security | Salted password hashing and constant-time verification | Store password verifiers, never plaintext passwords / 保存密码验证值而非明文 | `examples/csharp/06-security/` |
| Distributed systems | In-memory idempotency, bounded retry, basic circuit breaker, cache-aside | Understand duplicate work and partial failure / 理解重复请求和局部故障 | `examples/csharp/07-distributed-patterns/` |
| Database | DDL, keys, constraints, indexes, joins, transactions, aggregation | Keep durable relational state consistent / 保持关系数据持久且一致 | `examples/sql/` |
| Browser | Semantic HTML, CSS layout, DOM events, localStorage | Learn the platform before relying on React / 先理解浏览器平台再使用 React | `examples/browser/` |
| Backend | HTTP, REST, validation, dependency injection, layers, status codes | Expose application use cases as an API / 将应用用例暴露为 API | `apps/task-board/server/` |
| Frontend | React components, state, effects, forms, API calls | Build a reactive client around server state / 围绕服务端状态构建响应式客户端 | `apps/task-board/client/` |
| Testing | Unit tests, fake dependencies, boundary cases | Verify business rules without starting the whole system / 无需启动全系统即可验证业务规则 | `tests/TaskBoard.Tests/` |
| Delivery | CI, Docker, health checks, configuration | Make builds repeatable and failures visible / 让构建可重复、失败可见 | `.github/workflows/`, `apps/task-board/server/Dockerfile` |

The detailed concept-to-file index is in [`docs/CONCEPT_INDEX.md`](docs/CONCEPT_INDEX.md).
For common interview questions and bilingual answers, use [`docs/INTERVIEW_FOUNDATIONS.md`](docs/INTERVIEW_FOUNDATIONS.md).

## Run every small C# example / 运行所有 C# 小例子

Run projects independently so each concept stays visible:

```bash
dotnet run --project examples/csharp/01-language-basics
dotnet run --project examples/csharp/02-data-structures
dotnet run --project examples/csharp/03-async-concurrency
dotnet run --project examples/csharp/04-file-io-json
dotnet run --project examples/csharp/05-networking
dotnet run --project examples/csharp/06-security
dotnet run --project examples/csharp/07-distributed-patterns
dotnet run --project examples/csharp/08-interview-algorithms
dotnet run --project examples/csharp/09-runtime-semantics
```

Run the TypeScript language example:

```bash
cd examples/typescript
npm install
npm start
npm run event-loop
```

## Run the full-stack Task Board / 运行全栈 Task Board

Terminal 1 — API:

```bash
dotnet run --project apps/task-board/server
```

Terminal 2 — React client:

```bash
cd apps/task-board/client
npm install
npm run dev
```

Open `http://localhost:5173`. The browser calls `http://localhost:5080/api/todos`.

The application is deliberately small, but its boundaries are realistic:

```text
React UI
  -> HTTP/JSON
API endpoint
  -> application service
domain model
  -> repository interface
in-memory infrastructure adapter
```

Replace only the repository adapter when learning PostgreSQL; the domain and application layers do not need to know which database is used.

## Test / 测试

```bash
dotnet test tests/TaskBoard.Tests
cd apps/task-board/client
npm run build
```

## Recommended order / 推荐顺序

1. Read [`docs/LEARNING_PATH.md`](docs/LEARNING_PATH.md).
2. Run language and data-structure examples.
3. Run async, files, networking, and security examples.
4. Execute the SQL file in SQLite or adapt it to PostgreSQL.
5. Open the plain-browser example.
6. Trace one Task Board request from React to the repository.
7. Change a business rule and write its test first.
8. Study distributed patterns, CI, and Docker after the single-process flow is clear.
9. Use the [interview foundations](docs/INTERVIEW_FOUNDATIONS.md) to explain each mechanism, trade-off and code path aloud.

## Scope / 范围

“All common knowledge” here means the common core needed to understand and build a production-style web application. Specialized topics such as compiler construction, GPU programming, consensus implementation, Kubernetes operators, and model training belong in later repositories after this foundation is mastered.

## References / 参考教程

The repository is original. It links to strong public curricula and sample applications in [`docs/REFERENCES.md`](docs/REFERENCES.md); no third-party source code is copied.
