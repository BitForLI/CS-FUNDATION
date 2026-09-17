# Learning path / 学习路径

## Stage 1 — Programming model / 编程模型

Learn values, control flow, functions, types, collections, errors, and complexity. Use these whenever you translate a requirement into deterministic logic.

学习变量、控制流、函数、类型、集合、错误和复杂度。任何把需求翻译成确定性逻辑的工作都会使用它们。

Paths:

- `examples/csharp/01-language-basics/`
- `examples/csharp/02-data-structures/`
- `examples/csharp/08-interview-algorithms/`

Completion check: explain why a dictionary lookup is normally preferable to scanning a list by ID.

## Stage 2 — Machine and runtime boundaries / 机器与运行时边界

Async code does not automatically mean parallel code. Files are byte streams. Network messages also become bytes and require framing. Learn these before debugging web-service latency.

异步不等于并行；文件本质是字节流；网络消息也需要编码和分帧。这些是排查 Web 服务延迟的前置知识。

Paths:

- `examples/csharp/03-async-concurrency/`
- `examples/csharp/04-file-io-json/`
- `examples/csharp/05-networking/`
- `examples/csharp/09-runtime-semantics/`

Completion check: identify where cancellation is observed and why two concurrent increments can lose updates.

## Stage 3 — Data and security / 数据与安全

Use relational constraints for invariants that must survive every application instance. Store salted password verifiers rather than passwords. Treat browser input as untrusted.

必须跨进程、跨实例成立的不变量应交给数据库约束；密码只保存带盐验证值；浏览器输入永远不可信。

Paths:

- `examples/sql/`
- `examples/csharp/06-security/`

Completion check: explain primary keys, foreign keys, unique constraints, transactions, salts, and constant-time comparison.

## Stage 4 — Web platform and layers / Web 平台与分层

First run the browser example, then trace Task Board from HTTP endpoint to service and repository. The endpoint handles transport; the service owns business rules; the repository owns persistence access.

先运行原生浏览器示例，再沿 Task Board 请求追踪 Endpoint、Service 和 Repository。Endpoint 管传输，Service 管业务规则，Repository 管持久化访问。

Paths:

- `examples/browser/`
- `apps/task-board/server/`
- `apps/task-board/client/`

Completion check: add a validation rule without placing it in React or the repository.

## Stage 5 — Reliability and delivery / 可靠性与交付

Retries can duplicate work, caches can become stale, and remote calls can partially fail. Add idempotency, timeouts, bounded retries, circuit breakers, health checks, tests, CI, and containers for specific failure modes.

重试可能造成重复执行，缓存可能过期，远程调用会部分失败。针对具体失败模式使用幂等、超时、有界重试、熔断、健康检查、测试、CI 和容器。

Paths:

- `examples/csharp/07-distributed-patterns/`
- `tests/TaskBoard.Tests/`
- `.github/workflows/ci.yml`
- `apps/task-board/server/Dockerfile`

Completion check: explain why retrying a non-idempotent request can create two orders.

## Interview recap / 面试复盘

Read [`INTERVIEW_FOUNDATIONS.md`](INTERVIEW_FOUNDATIONS.md) and answer each question using definition, mechanism, code path, trade-off and failure case. Run `npm run event-loop` in `examples/typescript/` when reviewing browser scheduling. / 按定义、原理、代码路径、取舍、失败场景五步回答；复习事件循环时运行 TypeScript 示例。
