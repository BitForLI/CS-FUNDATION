# Interview foundations / 面试基础知识

Use this as a **question → short answer → when/why → code path** guide. Say the mechanism first, then give a concrete example and a trade-off. The runnable examples are teaching models, not production implementations.

按“**问题 → 简答 → 何时使用/作用 → 代码路径**”复习。先讲机制，再举项目实例和取舍。可运行小例子用于教学，不等于生产实现。

## 1. Language, OOP and algorithms / 语言、面向对象与算法

### 1. Value type versus reference type? / 值类型和引用类型有什么区别？

- **Answer / 回答：** A value variable contains its value; assigning it copies that value. A reference variable contains a reference to an object; copying the variable makes another reference to the same object. C# `struct`/`int` are value types, `class` is a reference type. Heap/stack placement is an implementation detail, not the definition. / 值类型赋值复制值；引用类型赋值复制引用，因此可能指向同一个对象。不要把“值类型一定在栈、引用类型一定在堆”当作定义。
- **When/why / 场景与作用：** Explain aliasing and mutation bugs in shared state. / 排查共享状态被意外修改。
- **Run / 路径：** `dotnet run --project examples/csharp/09-runtime-semantics`; `examples/csharp/09-runtime-semantics/Program.cs`.

### 2. `==`, equality and hash codes? / `==`、相等性与哈希值？

- **Answer / 回答：** Equality semantics depend on the type. Records supply value-style equality; mutable classes normally use reference identity unless equality is implemented. Equal keys must have equal hash codes, and dictionary keys should not change while stored. / 相等性由类型决定；record 默认按值比较；普通可变 class 通常按身份比较。相等的键必须有相同哈希值，入字典后不应改变影响哈希的字段。
- **When/why / 场景与作用：** Cache keys, sets and deduplication. / 缓存键、集合去重。
- **Run / 路径：** `examples/csharp/09-runtime-semantics/Program.cs`; `examples/csharp/02-data-structures/Program.cs`.

### 3. Interface, inheritance and dependency inversion? / 接口、继承与依赖倒置？

- **Answer / 回答：** An interface describes a capability; inheritance shares or specializes behavior. High-level business rules should depend on an abstraction such as `ITodoRepository`, not on a database class. The application chooses the concrete adapter at startup. / 接口描述能力，继承用于共享或特化行为。业务层依赖 `ITodoRepository` 抽象，启动入口再选择具体存储实现。
- **When/why / 场景与作用：** Replace a database adapter or inject a fake in tests. Do not create interfaces for every trivial class. / 替换存储或测试替身；不要给每个简单类机械套接口。
- **Path / 路径：** `apps/task-board/server/Application/ITodoRepository.cs`, `Infrastructure/InMemoryTodoRepository.cs`, `Program.cs`, `tests/TaskBoard.Tests/TodoServiceTests.cs`.

### 4. What does SOLID mean here? / SOLID 在此项目中如何落地？

- **Answer / 回答：** Single responsibility keeps HTTP transport in `Program.cs`, rules in `TodoItem`/`TodoService`, and storage in a repository. Open/closed and dependency inversion allow a new repository adapter without rewriting the service. Interface segregation keeps the repository small. Liskov substitution means any adapter must honor the same contract. / 单一职责分开 HTTP、规则和存储；开闭与依赖倒置允许换适配器；接口隔离保持契约小；里氏替换要求新适配器遵守同一语义。
- **When/why / 场景与作用：** Use when changes in one layer repeatedly break another; avoid over-engineering a tiny script. / 跨层修改频繁互相影响时有用，小脚本不必过度设计。
- **Path / 路径：** `apps/task-board/server/{Program.cs,Domain/,Application/,Infrastructure/}`.

### 5. Big-O, array/list versus dictionary? / 复杂度与 List、Dictionary 怎么选？

- **Answer / 回答：** Scanning a list of `n` IDs is O(n); dictionary lookup is expected O(1), with extra memory and hash overhead. An array/list offers O(1) indexed access and efficient contiguous iteration; insertion in its middle is O(n). Worst-case dictionary behavior is not guaranteed O(1). / List 按 ID 扫描为 O(n)，字典查找平均 O(1)但占额外空间；列表按下标读取 O(1)，中间插入 O(n)。字典最坏情况不能保证 O(1)。
- **When/why / 场景与作用：** Pick by the dominant access pattern and actual data size. / 根据主要访问模式和数据规模选择。
- **Run / 路径：** `dotnet run --project examples/csharp/02-data-structures`.

### 6. What do the four common coding patterns solve? / 四种常见算法套路解决什么？

- **Answer / 回答：** Hash lookup trades memory for one-pass matching; sliding window maintains a valid contiguous range; a stack matches nested structure; binary search halves a monotonic search space. / 哈希表以空间换一次遍历；滑动窗口维护连续合法区间；栈处理嵌套结构；二分搜索利用单调性不断减半。
- **When/why / 场景与作用：** Two Sum, longest unique substring, brackets, first element not less than target. Always state invariant, complexity and edge cases. / 两数之和、最长不重复子串、括号匹配、二分边界；回答时说清不变量、复杂度和边界。
- **Run / 路径：** `dotnet run --project examples/csharp/08-interview-algorithms`.

### 7. BFS versus DFS? / 广度优先与深度优先区别？

- **Answer / 回答：** BFS uses a queue and finds a shortest path by edge count in an unweighted graph; DFS uses recursion or a stack and is useful for reachability or exhaustive traversal. Neither directly solves weighted shortest paths. / BFS 用队列，可找无权图最少边路径；DFS 用递归或栈，适合遍历和可达性；有权最短路不能直接套用。
- **When/why / 场景与作用：** Dependency traversal, navigation, graph interview questions. / 依赖遍历、路径与图题。
- **Run / 路径：** `examples/csharp/02-data-structures/Program.cs` implements BFS.

## 2. Runtime, operating systems and concurrency / 运行时、操作系统与并发

### 8. Process versus thread? / 进程与线程区别？

- **Answer / 回答：** Processes have separate virtual address spaces and resource handles; threads within a process share its address space but have independent execution stacks. Isolation is stronger across processes; sharing is cheaper but requires synchronization across threads. / 进程有各自虚拟地址空间与资源句柄；同一进程的线程共享地址空间但各有执行栈。进程隔离强，线程共享方便但必须同步。
- **When/why / 场景与作用：** Choose process isolation for services; use concurrent tasks/threads inside a service for work. / 服务间隔离与服务内并发的设计选择。
- **Path / 路径：** `examples/csharp/03-async-concurrency/Program.cs` demonstrates in-process tasks; process isolation is a conceptual OS boundary.

### 9. Async versus parallelism versus concurrency? / 异步、并发、并行区别？

- **Answer / 回答：** Async avoids blocking while waiting, concurrency interleaves multiple operations, and parallelism executes work at the same time on multiple cores. `await Task.Delay` is not CPU parallelism; `Task.Run` schedules CPU work on a thread pool. / 异步是等待时不阻塞，并发是任务交错推进，并行是多核同一时刻执行。`await Task.Delay` 不是 CPU 并行。
- **When/why / 场景与作用：** Async for I/O-bound web requests; bounded parallel work for CPU-bound computation. / I/O 请求用异步，CPU 计算考虑有界并行。
- **Run / 路径：** `examples/csharp/03-async-concurrency/Program.cs`, `examples/csharp/09-runtime-semantics/Program.cs`.

### 10. Race condition, lock and deadlock? / 竞态、锁与死锁？

- **Answer / 回答：** A race means the result depends on interleaving; `counter++` is a read-modify-write sequence. A lock protects a critical section. Deadlock occurs when tasks wait cyclically for resources; consistent lock order and small critical sections reduce risk. / 竞态使结果依赖执行交错；`counter++` 并非原子操作。锁保护临界区；循环等待可能死锁，固定加锁顺序和短临界区可降低风险。
- **When/why / 场景与作用：** Shared mutable in-memory state; a process-local lock does not coordinate multiple server instances. / 进程内共享状态；本地锁不能跨服务器实例。
- **Run / 路径：** `examples/csharp/03-async-concurrency/Program.cs`.

### 11. `Channel`, queue and backpressure? / Channel、队列与背压？

- **Answer / 回答：** Producers enqueue work and consumers process it. A bounded channel makes producers wait when full, preventing unbounded memory growth. An in-process channel loses queued work if the process dies; a durable broker has different guarantees. / 生产者入队、消费者处理；有界队列满时生产者等待，防止内存无限增长。进程内队列会随进程故障丢失，不能当持久消息队列。
- **When/why / 场景与作用：** Decouple short in-process pipelines with controlled capacity. / 进程内解耦与限流。
- **Run / 路径：** `examples/csharp/03-async-concurrency/Program.cs`.

### 12. Garbage collection versus `Dispose`? / GC 与 `Dispose` 区别？

- **Answer / 回答：** GC reclaims managed memory when it decides; `Dispose` releases external resources such as files and sockets at a known point. Use `using`/`await using` for disposable resources; a GC collection is not a substitute for closing a file. / GC 在不确定时间回收托管内存；`Dispose` 在确定时间释放文件、套接字等外部资源。
- **When/why / 场景与作用：** File locks, network connections, streams. / 文件锁、网络连接和流。
- **Run / 路径：** `examples/csharp/04-file-io-json/Program.cs`, `examples/csharp/09-runtime-semantics/Program.cs`.

### 13. Virtual memory, stack and heap? / 虚拟内存、栈和堆？

- **Answer / 回答：** Virtual memory gives each process an address-space abstraction. A thread stack contains call frames; the managed heap stores many runtime-managed objects. Language value/reference semantics do not by themselves determine one physical location. / 虚拟内存给进程独立地址空间抽象；线程栈容纳调用帧；托管堆容纳许多受 GC 管理对象。不要用“值/引用类型”简单推断物理位置。
- **When/why / 场景与作用：** Explain stack overflow, heap allocation and memory pressure. / 排查递归栈溢出、对象分配和内存压力。
- **Path / 路径：** `examples/csharp/09-runtime-semantics/Program.cs` demonstrates semantics, not OS page tables.

## 3. Networking and HTTP / 网络与 HTTP

### 14. What happens when a URL is entered? / 输入 URL 后发生什么？

- **Answer / 回答：** The browser parses the URL, resolves the host via DNS (possibly cache), establishes a connection, negotiates TLS for HTTPS, sends HTTP, receives response bytes, parses HTML/CSS/JS, then renders. HTTP/2 and HTTP/3 differ in transport details. / 浏览器解析 URL、通过 DNS 找地址、建立连接、HTTPS 协商 TLS、发送 HTTP、接收响应，再解析和渲染。HTTP/2 与 HTTP/3 的传输细节不同。
- **When/why / 场景与作用：** Diagnose DNS, TLS, server, transfer and rendering latency separately. / 分层定位 DNS、TLS、服务端、传输和渲染耗时。
- **Path / 路径：** `examples/csharp/05-networking/Program.cs` shows TCP bytes; `apps/task-board/server/Program.cs` shows HTTP; `apps/task-board/client/src/api.ts` sends requests.

### 15. TCP stream versus HTTP message? / TCP 字节流与 HTTP 消息？

- **Answer / 回答：** TCP provides ordered bytes, not application message boundaries. A protocol needs framing, such as a length prefix; HTTP defines its own framing, methods, headers and status codes. One `Read` need not return one whole application message. / TCP 提供有序字节流，不提供业务消息边界；应用协议需要分帧。一次 `Read` 不保证读完一个消息。
- **When/why / 场景与作用：** Design a custom protocol or debug partial reads. / 设计协议和排查粘包、半包。
- **Run / 路径：** `dotnet run --project examples/csharp/05-networking`.

### 16. HTTP methods and idempotency? / HTTP 方法与幂等？

- **Answer / 回答：** GET reads, POST usually creates or invokes a command, PUT replaces a resource, PATCH partially changes it, DELETE removes it. Idempotent means repeating the same request has the same intended effect, not necessarily the same response. A toggle action is not idempotent even when exposed via PATCH. / GET 读取、POST 通常创建或执行命令、PUT 替换、PATCH 局部修改、DELETE 删除。幂等是重复执行后的预期状态相同，不代表响应完全相同；toggle 操作本身不幂等。
- **When/why / 场景与作用：** Safe retries, API design and client behavior. / 安全重试与接口设计。
- **Path / 路径：** `apps/task-board/server/Program.cs`; `examples/csharp/07-distributed-patterns/Program.cs`.

### 17. Status codes and validation? / 状态码与校验？

- **Answer / 回答：** 200 returns a successful representation, 201 indicates creation and a resource location, 204 succeeds without body, 400/validation problem means invalid input, 404 means missing resource, 401 means unauthenticated and 403 means authenticated but forbidden. The server must validate even if the browser validates. / 200 返回结果，201 创建成功，204 无响应体，400/ValidationProblem 表示输入错误，404 资源不存在，401 未登录，403 已登录但无权限。浏览器校验不能替代服务端校验。
- **When/why / 场景与作用：** Predictable client error handling. / 前端能够可靠处理结果。
- **Run / 路径：** `apps/task-board/server/Program.cs`, `apps/task-board/server/Domain/TodoItem.cs`.

### 18. CORS versus authentication? / CORS 与认证有何区别？

- **Answer / 回答：** CORS is a browser rule controlling whether frontend JavaScript may read a cross-origin response. It is not an authorization check and does not prevent non-browser clients from calling an API. Authentication verifies identity; authorization decides permission. / CORS 是浏览器跨域读取规则，不是权限保护，也阻止不了非浏览器客户端。认证回答“你是谁”，授权回答“能做什么”。
- **When/why / 场景与作用：** Separate browser configuration from API security. / 避免把跨域配置当作安全边界。
- **Path / 路径：** `apps/task-board/server/Program.cs` configures CORS; this minimal Task Board intentionally has no auth.

### 19. HTTPS and TLS? / HTTPS 与 TLS 的作用？

- **Answer / 回答：** TLS authenticates the server certificate and protects transport confidentiality and integrity. It does not make unsafe application logic safe. Use HTTPS for credentials and personal data. / TLS 验证服务器证书并保护传输机密性、完整性；它不能修复应用逻辑漏洞。
- **When/why / 场景与作用：** Public web traffic and API calls. / 公网 Web 与 API 调用。
- **Path / 路径：** `examples/csharp/05-networking/Program.cs` is plain loopback TCP for teaching; configure TLS at a real deployment boundary.

## 4. Databases and transactions / 数据库与事务

### 20. Primary key, foreign key, unique and check? / 主键、外键、唯一、检查约束？

- **Answer / 回答：** A primary key identifies a row; a foreign key prevents orphan references; unique prevents duplicate values; check enforces a row-level rule. Database constraints protect every writer, not only one application's validation code. / 主键标识行、外键防孤儿引用、唯一约束防重复、检查约束限制行值；数据库约束覆盖所有写入者。
- **When/why / 场景与作用：** Enforce durable invariants across multiple instances. / 跨实例保持数据不变量。
- **Path / 路径：** `examples/sql/schema.sql`.

### 21. What is an index and when does it hurt? / 索引是什么，什么时候有代价？

- **Answer / 回答：** An index is an auxiliary search structure that can avoid scanning every row for suitable predicates and ordering. It consumes space and must be maintained on writes. The index `(owner_id, is_done)` supports queries starting with `owner_id`; column order matters. / 索引是额外检索结构，可以避免合适查询的全表扫描，但占空间并增加写入维护成本。`(owner_id, is_done)` 对以 `owner_id` 开头的条件更有用，列顺序重要。
- **When/why / 场景与作用：** Add after measuring frequent query patterns, not to every column. / 根据高频查询和执行计划建立，不是每列都加。
- **Path / 路径：** `examples/sql/schema.sql`, `examples/sql/queries.sql`.

### 22. Transaction and ACID? / 事务与 ACID？

- **Answer / 回答：** Atomicity: all-or-nothing; consistency: constraints hold before/after a valid transaction; isolation: concurrent transactions have defined visibility; durability: committed changes survive according to the database's guarantees. `BEGIN ... COMMIT` groups related writes. / 原子性是要么全做要么全不做；一致性是约束保持；隔离性定义并发可见性；持久性定义提交后的保存保证。`BEGIN ... COMMIT` 把相关写入组成整体。
- **When/why / 场景与作用：** Create an order and its lines together. / 同时创建订单及订单行。
- **Path / 路径：** `examples/sql/schema.sql`.

### 23. Isolation levels and anomalies? / 隔离级别与异常现象？

- **Answer / 回答：** Lower isolation can permit phenomena such as non-repeatable reads and phantoms depending on the database; stronger isolation reduces anomalies but may increase blocking or transaction retries. PostgreSQL uses MVCC; exact behavior must be checked for the chosen database and isolation level. / 不同隔离级别可能出现不可重复读、幻读等现象；更强隔离通常增加冲突与重试。PostgreSQL 用 MVCC，具体语义必须按数据库和级别判断。
- **When/why / 场景与作用：** Inventory, balances and competing order updates. / 库存、余额、并发订单更新。
- **Path / 路径：** `examples/sql/schema.sql` introduces a transaction; no isolation-level simulator is included.

### 24. SQL join, N+1 and pagination? / Join、N+1 与分页？

- **Answer / 回答：** A join combines related rows in one query. N+1 occurs when one parent query triggers a separate child query for each row. Pagination bounds data transfer; stable ordering plus cursor pagination handles changing large datasets better than deep offsets. / Join 一次关联相关数据；N+1 是查一批主记录后逐条再查子记录；分页限制数据量，变化快的大数据集常用稳定排序加游标分页。
- **When/why / 场景与作用：** User/order listings and dashboards. / 列表与报表接口。
- **Path / 路径：** `examples/sql/queries.sql` demonstrates join and aggregation.

### 25. SQL injection prevention? / 如何防 SQL 注入？

- **Answer / 回答：** Use parameterized queries or ORM parameter binding for values. Never concatenate untrusted input into SQL. Parameterization does not automatically make dynamic table or column names safe; whitelist identifiers separately. / 对值使用参数化查询或 ORM 参数绑定；不能拼接不可信输入。动态表名、列名需单独白名单校验。
- **When/why / 场景与作用：** Any query using user-supplied values. / 所有含用户输入的查询。
- **Path / 路径：** `examples/sql/queries.sql` includes a parameterized query shape.

## 5. Browser, frontend and security / 浏览器、前端与安全

### 26. Browser event loop? / 浏览器事件循环？

- **Answer / 回答：** Synchronous work runs first; promise callbacks are microtasks; timer callbacks are later tasks. Long synchronous work blocks rendering and interaction. Node and browser have different host details; the example proves only this common ordering. / 同步代码先运行，Promise 回调进入微任务，定时器回调随后作为任务执行。长时间同步计算会阻塞交互与渲染；Node 和浏览器细节并不完全一样。
- **When/why / 场景与作用：** Debug surprising UI ordering and responsiveness. / 排查状态更新时序及卡顿。
- **Run / 路径：** `cd examples/typescript && npm run event-loop`; `src/event-loop.ts`.

### 27. React state, props and effect? / React state、props、effect？

- **Answer / 回答：** Props pass data into a component; state stores data that changes its render; an effect synchronizes with an external system such as an API. Do not use an effect for a value derivable from existing state; compute it during render or memoize only when useful. / props 传入数据，state 驱动重渲染，effect 同步外部系统；可从现有状态计算的值不必另设 effect。
- **When/why / 场景与作用：** Build UI that reflects server updates predictably. / 构建与服务端数据同步的界面。
- **Path / 路径：** `apps/task-board/client/src/App.tsx` uses state/effect and derives remaining count.

### 28. Controlled input and accessibility? / 受控输入与可访问性？

- **Answer / 回答：** A controlled input reads its value from state and updates state on input events. Semantic elements, associated labels, buttons and live regions help keyboard and assistive-technology users. / 受控输入的值来自 state，并由事件更新；语义标签、关联 label、button 和实时状态区域支持键盘与辅助技术。
- **When/why / 场景与作用：** Forms and interactive controls. / 表单和交互组件。
- **Path / 路径：** `apps/task-board/client/src/App.tsx`, `examples/browser/index.html`.

### 29. XSS, CSRF and password storage? / XSS、CSRF、密码存储？

- **Answer / 回答：** XSS executes attacker-controlled script in a trusted page; prefer text insertion over unsafe HTML, then apply output encoding and policy appropriate to context. CSRF makes a browser send an authenticated request using ambient credentials; cookie-auth applications need CSRF defenses such as SameSite plus anti-forgery tokens where appropriate. Store salted, slow password hashes rather than plaintext or a fast unsalted hash. / XSS 是攻击者脚本进入可信页面；优先按文本插入。CSRF 借浏览器自动携带的凭证发请求；Cookie 认证要考虑 SameSite 与防伪令牌。密码应存带盐慢哈希。
- **When/why / 场景与作用：** Any public form, authentication or personalized page. / 公共输入、登录和个性化页面。
- **Path / 路径：** `examples/browser/app.js` uses `textContent` for task titles; `examples/csharp/06-security/Program.cs` shows PBKDF2. The Task Board has no authentication, so it is not a CSRF demonstration.

### 30. Authentication versus authorization; JWT versus session? / 认证、授权；JWT 与 Session？

- **Answer / 回答：** Authentication verifies identity; authorization checks permission. A server session stores state server-side and sends an opaque session ID; a signed JWT carries claims verifiable by the API. JWTs still need expiry, key rotation and a revocation strategy; neither format alone provides access control. / 认证确认身份、授权检查权限。Session 在服务端存状态，客户端拿不透明 ID；JWT 携带签名声明。JWT 仍需要过期、密钥轮换和吊销方案，令牌格式本身不等于权限设计。
- **When/why / 场景与作用：** Protected APIs and user-specific data. / 受保护接口和个人数据。
- **Path / 路径：** `apps/task-board/server/Program.cs` is intentionally unauthenticated; `examples/csharp/06-security/Program.cs` covers password verification only. Do not claim JWT implementation from this repository.

## 6. Distributed systems, testing and delivery / 分布式、测试与交付

### 31. Retry, timeout and idempotency? / 重试、超时与幂等？

- **Answer / 回答：** A timeout bounds how long a caller waits; a retry repeats a failed operation and can amplify load or duplicate side effects; an idempotency key lets a server recognize a repeated command. Retry only suitable failures with a finite budget and preferably backoff/jitter. / 超时限制等待；重试会放大负载或重复副作用；幂等键识别同一命令。只对合适错误有限重试，并考虑退避与随机抖动。
- **When/why / 场景与作用：** Payment, order creation and remote dependencies. / 支付、创建订单与远程依赖。
- **Run / 路径：** `examples/csharp/07-distributed-patterns/Program.cs` is a single-process teaching model; it does not provide durable, cross-instance idempotency or a production timeout policy.

### 32. Cache-aside and invalidation? / 旁路缓存与失效？

- **Answer / 回答：** On read, check cache then load and populate on miss. On write, update the source of truth and invalidate or update the cache. TTL limits staleness but does not guarantee immediate consistency. / 读时先查缓存，未命中再查数据源并回填；写时更新真相源并使缓存失效或同步。TTL 限制陈旧时间，但不保证实时一致。
- **When/why / 场景与作用：** Repeated expensive reads where bounded staleness is acceptable. / 可容忍有限陈旧且重复查询昂贵的场景。
- **Run / 路径：** `examples/csharp/07-distributed-patterns/Program.cs`.

### 33. What is a circuit breaker? / 熔断器是什么？

- **Answer / 回答：** After enough failures it stops calls temporarily, allowing a dependency to recover and callers to fail fast. A real breaker needs a half-open probe, concurrency-safe state and observation; the minimal example only shows closed/open. / 连续失败后短暂停止调用，使依赖恢复并让请求快速失败。生产实现还需半开探测、并发安全和监控；当前示例仅演示关闭/打开。
- **When/why / 场景与作用：** Remote dependency under repeated failure. / 远程依赖持续故障。
- **Run / 路径：** `examples/csharp/07-distributed-patterns/Program.cs`.

### 34. Strong consistency versus eventual consistency? / 强一致与最终一致？

- **Answer / 回答：** Stronger guarantees make a completed write immediately visible under a defined model; eventual consistency permits temporary disagreement if replicas converge later. The choice affects latency, availability and user expectations. Do not claim a guarantee without naming the store and operation. / 较强一致模型对写后可见性有严格约定；最终一致允许副本短暂不一致并在之后收敛。选择会影响延迟、可用性和体验；必须说明具体存储与操作。
- **When/why / 场景与作用：** Cross-region replicas, cache reads and asynchronous projections. / 跨区域副本、缓存和异步投影。
- **Path / 路径：** `examples/csharp/07-distributed-patterns/Program.cs` illustrates stale cache risk, not a replicated database.

### 35. Unit, integration and end-to-end tests? / 单元、集成与端到端测试？

- **Answer / 回答：** Unit tests isolate one rule with fake dependencies; integration tests exercise real boundaries such as HTTP and storage; E2E follows a user journey across the system. Each catches different defects and costs different time. / 单元测试隔离规则；集成测试覆盖 HTTP/数据库等真实边界；端到端测试模拟完整用户流程。覆盖目标和成本不同。
- **When/why / 场景与作用：** Fast business feedback plus confidence that boundaries connect. / 快速验证规则并证明模块连接。
- **Path / 路径：** `tests/TaskBoard.Tests/TodoServiceTests.cs` is unit testing; run `apps/task-board/server/TaskBoard.Api.http` for a manual HTTP check; CI builds both server and client but does not run browser E2E.

### 36. What is CI/CD and what does a green build prove? / CI/CD 与绿色构建能证明什么？

- **Answer / 回答：** CI repeats builds and automated checks on changes; delivery/deployment moves an artifact to an environment. A green build proves the configured checks passed, not that production behavior or security is perfect. / CI 在变更时重复构建与测试，交付/部署把产物送到环境。绿色构建只证明已配置检查通过，不等于生产无故障。
- **When/why / 场景与作用：** Catch regressions before merging and keep builds reproducible. / 合并前捕获回归并保证可重复构建。
- **Path / 路径：** `.github/workflows/ci.yml`, `apps/task-board/server/Dockerfile`.

## Practice prompt / 自测方法

For each answer, say aloud: **definition → mechanism → example in this repository → trade-off → one failure case**. If the code path does not implement the mechanism (for example JWT or database isolation), say so directly and treat the answer as conceptual knowledge.

每题口述五步：**定义 → 原理 → 本仓库例子 → 取舍 → 一个失败场景**。如果本仓库没有实际实现（例如 JWT 或数据库隔离级别实验），直接说明它只是概念知识。
