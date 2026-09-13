-- Projection + filter: return only unfinished work.
SELECT id, title FROM tasks WHERE is_done = 0 ORDER BY created_utc;

-- Join + aggregation: count work per user, including users with zero tasks.
SELECT
    u.display_name,
    COUNT(t.id) AS total_tasks,
    SUM(CASE WHEN t.is_done = 1 THEN 1 ELSE 0 END) AS completed_tasks
FROM users AS u
LEFT JOIN tasks AS t ON t.owner_id = u.id
GROUP BY u.id, u.display_name
ORDER BY u.display_name;

-- Parameterized equivalent in application code:
-- SELECT * FROM tasks WHERE owner_id = @owner_id;
-- Never concatenate untrusted text into SQL.

