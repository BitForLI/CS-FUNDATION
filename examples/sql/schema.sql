PRAGMA foreign_keys = ON;

CREATE TABLE users (
    id INTEGER PRIMARY KEY,
    email TEXT NOT NULL UNIQUE,
    display_name TEXT NOT NULL CHECK (length(display_name) BETWEEN 1 AND 100)
);

CREATE TABLE tasks (
    id INTEGER PRIMARY KEY,
    owner_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    title TEXT NOT NULL CHECK (length(title) BETWEEN 1 AND 200),
    is_done INTEGER NOT NULL DEFAULT 0 CHECK (is_done IN (0, 1)),
    created_utc TEXT NOT NULL
);

CREATE INDEX ix_tasks_owner_done ON tasks(owner_id, is_done);

BEGIN TRANSACTION;
INSERT INTO users(id, email, display_name) VALUES (1, 'ada@example.com', 'Ada');
INSERT INTO users(id, email, display_name) VALUES (2, 'linus@example.com', 'Linus');
INSERT INTO tasks(id, owner_id, title, is_done, created_utc)
VALUES
    (1, 1, 'Learn primary keys', 1, '2026-01-01T00:00:00Z'),
    (2, 1, 'Learn joins', 0, '2026-01-02T00:00:00Z'),
    (3, 2, 'Learn transactions', 0, '2026-01-03T00:00:00Z');
COMMIT;

