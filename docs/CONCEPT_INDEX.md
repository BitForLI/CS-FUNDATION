# Concept index / 知识点索引

This index tells you exactly where a concept is implemented and what observable result proves it.

| Concept / 概念 | Layer / 层 | Path / 路径 | Use when / 何时使用 | Observable result / 可观察结果 |
|---|---|---|---|---|
| Value and reference types | Language | `examples/csharp/01-language-basics/Program.cs` | Model data with explicit semantics / 明确数据语义 | Printed immutable order summary |
| Interface and dependency inversion | Language/Application | `01-language-basics`, `apps/task-board/server/Application/` | Swap implementations without changing callers / 替换实现而不改调用方 | Fake clock/repository can be injected |
| Generics | Language | `01-language-basics/Program.cs` | Reuse type-safe algorithms / 复用类型安全算法 | Generic `Last<T>` works for strings and numbers |
| LINQ | Language/Data | `01-language-basics/Program.cs` | Filter/project in-memory sequences / 查询内存集合 | Active items are filtered and summed |
| Union narrowing and promises | Frontend language | `examples/typescript/src/index.ts` | Model success/failure and asynchronous values / 表达成功失败与异步值 | TypeScript example prints narrowed results |
| Big-O | Algorithm | `02-data-structures/Program.cs` | Compare growth as input grows / 比较输入扩大后的成本 | Linear and logarithmic operation counts |
| Stack/Queue/Dictionary | Algorithm | `02-data-structures/Program.cs` | LIFO/FIFO/key lookup / 后进先出、先进先出、键查找 | Deterministic traversal output |
| BFS | Algorithm | `02-data-structures/Program.cs` | Shortest path in an unweighted graph / 无权图最短路径 | Route `A -> B -> D` |
| Async and cancellation | Runtime | `03-async-concurrency/Program.cs` | Wait without blocking a thread / 非阻塞等待 | Cancellation stops work cleanly |
| Race condition and lock | Runtime | `03-async-concurrency/Program.cs` | Protect shared mutable state / 保护共享可变状态 | Locked counter reaches expected value |
| Channel | Runtime/Messaging | `03-async-concurrency/Program.cs` | Producer-consumer pipelines / 生产消费管线 | Consumer drains queued jobs |
| File/stream/JSON | OS boundary | `04-file-io-json/Program.cs` | Small durable local state and interchange / 小型本地状态与数据交换 | JSON round-trip equality |
| TCP and framing | Network | `05-networking/Program.cs` | Understand connection-oriented byte streams / 理解面向连接字节流 | Local echo response |
| PBKDF2 and salt | Security | `06-security/Program.cs` | Store password verifiers / 保存密码验证值 | Correct password true, wrong false |
| Schema and constraints | Database | `examples/sql/schema.sql` | Enforce durable invariants / 强制持久不变量 | Duplicate email and orphan rows rejected |
| Join and aggregation | Database | `examples/sql/queries.sql` | Combine and summarize related rows / 关联与汇总 | Per-user task counts |
| Semantic HTML and DOM | Browser | `examples/browser/` | Accessible no-framework UI / 无框架可访问界面 | Add/toggle/remove tasks in browser |
| REST endpoint | Transport | `apps/task-board/server/Program.cs` | Expose application operations / 暴露应用操作 | GET/POST/PATCH/DELETE return HTTP statuses |
| Domain model | Domain | `apps/task-board/server/Domain/TodoItem.cs` | Represent business state and transitions / 表达业务状态转换 | `Rename` and `Toggle` enforce invariants |
| Application service | Application | `apps/task-board/server/Application/TodoService.cs` | Coordinate use cases / 编排用例 | Validation and not-found behavior tested |
| Repository | Infrastructure | `apps/task-board/server/Infrastructure/` | Isolate persistence mechanism / 隔离持久化机制 | In-memory adapter can be replaced |
| React state/effect/form | Frontend | `apps/task-board/client/src/App.tsx` | Synchronize UI with server state / 同步界面和服务端状态 | CRUD updates without page reload |
| Unit test and fake | Test | `tests/TaskBoard.Tests/` | Prove a rule in isolation / 隔离验证业务规则 | `dotnet test` passes |
| Idempotency | Distributed | `07-distributed-patterns/Program.cs` | Safely retry commands / 安全重试命令 | Duplicate key executes once |
| Timeout/retry/circuit breaker | Distributed | `07-distributed-patterns/Program.cs` | Bound remote dependency failure / 限制远程依赖故障影响 | Failed calls stop after a bound |
| Cache-aside | Distributed/Data | `07-distributed-patterns/Program.cs` | Reduce repeated reads / 减少重复读取 | Second lookup is a cache hit |
| Health check | Operations | `apps/task-board/server/Program.cs` | Separate liveness from business APIs / 区分存活与业务接口 | `/health` returns 200 |
| CI | Delivery | `.github/workflows/ci.yml` | Re-run build/tests on every change / 每次变更重复构建测试 | GitHub Actions passes |
| Container | Delivery | `apps/task-board/server/Dockerfile` | Package runtime and app consistently / 一致打包运行环境 | Container serves `/health` |
