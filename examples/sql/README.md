# Relational database minimum / 关系数据库最小示例

Use this module when state must survive process restarts and invariants must be shared by every application instance.

当数据必须跨进程重启保存，并且所有应用实例必须遵守同一不变量时，使用关系数据库。

Concepts:

- primary and foreign keys / 主键与外键
- unique and check constraints / 唯一与检查约束
- indexes / 索引
- transaction / 事务
- join and aggregation / 连接与聚合

Run with SQLite:

```bash
sqlite3 learning.db < schema.sql
sqlite3 -header -column learning.db < queries.sql
```

Use PostgreSQL in a production service when you need concurrent writers, operational tooling, access control, and network access. The SQL here intentionally uses a portable subset.

