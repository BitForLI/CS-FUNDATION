import { FormEvent, useEffect, useMemo, useState } from 'react'
import { TodoItem, todoApi } from './api'

export default function App() {
  const [items, setItems] = useState<TodoItem[]>([])
  const [title, setTitle] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const controller = new AbortController()
    todoApi.list(controller.signal)
      .then(setItems)
      .catch(reason => {
        if (reason instanceof DOMException && reason.name === 'AbortError') return
        setError(String(reason))
      })
      .finally(() => setLoading(false))
    return () => controller.abort()
  }, [])

  const remaining = useMemo(() => items.filter(item => !item.isDone).length, [items])

  async function submit(event: FormEvent) {
    event.preventDefault()
    const normalized = title.trim()
    if (!normalized) return
    try {
      const created = await todoApi.create(normalized)
      setItems(current => [...current, created])
      setTitle('')
      setError('')
    } catch (reason) {
      setError(String(reason))
    }
  }

  async function toggle(item: TodoItem) {
    try {
      const changed = await todoApi.toggle(item.id)
      setItems(current => current.map(value => value.id === changed.id ? changed : value))
    } catch (reason) {
      setError(String(reason))
    }
  }

  async function remove(item: TodoItem) {
    try {
      await todoApi.remove(item.id)
      setItems(current => current.filter(value => value.id !== item.id))
    } catch (reason) {
      setError(String(reason))
    }
  }

  return (
    <main className="shell">
      <p className="eyebrow">CS FUNDATION · FULL STACK TRACE</p>
      <h1>Task Board</h1>
      <p className="intro">One small request crossing browser, HTTP, application, domain, and infrastructure layers.</p>

      <form onSubmit={submit}>
        <label htmlFor="title">New task / 新任务</label>
        <div className="inputRow">
          <input id="title" value={title} maxLength={200} onChange={event => setTitle(event.target.value)} />
          <button type="submit">Add</button>
        </div>
      </form>

      <div className="summary"><span>{items.length} total</span><span>{remaining} remaining</span></div>
      {loading && <p role="status">Loading…</p>}
      {error && <p className="error" role="alert">{error}</p>}
      <ul>
        {items.map(item => (
          <li key={item.id} className={item.isDone ? 'done' : ''}>
            <button className="titleButton" onClick={() => toggle(item)}>{item.title}</button>
            <button className="remove" aria-label={`Remove ${item.title}`} onClick={() => remove(item)}>×</button>
          </li>
        ))}
      </ul>
    </main>
  )
}

