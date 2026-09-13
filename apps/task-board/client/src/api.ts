export interface TodoItem {
  id: string
  title: string
  isDone: boolean
  createdAtUtc: string
}

const apiBase = import.meta.env.VITE_API_BASE ?? 'http://localhost:5080/api'

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${apiBase}${path}`, {
    ...init,
    headers: { 'Content-Type': 'application/json', ...init?.headers },
  })
  if (!response.ok) {
    const problem = await response.text()
    throw new Error(problem || `HTTP ${response.status}`)
  }
  return response.status === 204 ? (undefined as T) : response.json() as Promise<T>
}

export const todoApi = {
  list: (signal?: AbortSignal) => request<TodoItem[]>('/todos/', { signal }),
  create: (title: string) => request<TodoItem>('/todos/', { method: 'POST', body: JSON.stringify({ title }) }),
  rename: (id: string, title: string) => request<TodoItem>(`/todos/${id}`, { method: 'PUT', body: JSON.stringify({ title }) }),
  toggle: (id: string) => request<TodoItem>(`/todos/${id}/toggle`, { method: 'PATCH' }),
  remove: (id: string) => request<void>(`/todos/${id}`, { method: 'DELETE' }),
}

