# GitHub tutorial survey / GitHub 教程调研

No single surveyed repository combines a complete CS map, modern full-stack implementation, minimal runnable examples, and an explicit layer/use-case index. `CS-FUNDATION` combines the strongest teaching patterns while keeping its code original.

| Repository | Strong point / 优点 | Missing for this goal / 不足 | What we borrow / 借鉴方式 |
|---|---|---|---|
| [Microsoft Web Dev for Beginners](https://github.com/microsoft/Web-Dev-For-Beginners) | Short project-based lessons / 短小、项目驱动 | Focuses mainly on browser fundamentals / 主要覆盖浏览器基础 | Small independent lessons / 独立小课程 |
| [OSSU Computer Science](https://github.com/ossu/computer-science) | Broad university-level knowledge map / 大学级完整知识地图 | Mostly links to courses, not one runnable codebase / 以课程链接为主 | Topic ordering and prerequisite thinking / 知识顺序与前置关系 |
| [Full Stack Open](https://github.com/fullstack-hy2020/fullstack-hy2020.github.io) | Modern React, APIs, testing and delivery / 现代 React、API、测试和交付 | Less emphasis on low-level systems / 底层系统知识较少 | Incremental full-stack progression / 渐进式全栈路径 |
| [RealWorld](https://github.com/realworld-apps/realworld) | One product contract implemented by many stacks / 同一产品契约对应多技术栈 | Implementations are larger than minimal examples / 例子并不最小 | Stable API boundary / 稳定 API 边界 |
| [.NET eShop](https://github.com/dotnet/eShop) | Production-oriented .NET architecture / 生产型 .NET 架构 | Too large for a first runnable example / 初学者难以逐行理解 | Domain/application/infrastructure separation / 分层思想 |
| [System Design Primer](https://github.com/donnemartin/system-design-primer) | Explains scale and distributed trade-offs / 解释规模化和分布式取舍 | Mostly conceptual / 主要是概念 | Scenario-first explanations / 从场景解释概念 |
| [TheAlgorithms/C-Sharp](https://github.com/TheAlgorithms/C-Sharp) | Many C# algorithm implementations / 大量 C# 算法实现 | Not connected to a full-stack application / 未与全栈应用连接 | Small executable algorithm demonstrations / 小型算法演示 |
| [Project Based Learning](https://github.com/practical-tutorials/project-based-learning) | Large curated tutorial index / 大型项目教程索引 | Curation rather than a coherent curriculum / 是索引而非统一课程 | Further-study links / 延伸学习链接 |

## Design decision / 设计决定

The foundation uses C#/.NET 8 for systems and backend examples, TypeScript/React for the browser, SQL for relational concepts, and one Task Board application to show how layers connect. Every topic must have a path, a command, and an observable result.

