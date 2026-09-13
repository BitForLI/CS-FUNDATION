// Layer: frontend language. Run: npm install && npm start

interface User {
  readonly id: number
  name: string
  role: 'customer' | 'staff' | 'admin'
}

type ApiResult<T> =
  | { ok: true; value: T }
  | { ok: false; error: string }

const users: User[] = [
  { id: 1, name: 'Ada', role: 'admin' },
  { id: 2, name: 'Linus', role: 'staff' },
]

function findById<T extends { id: number }>(values: readonly T[], id: number): ApiResult<T> {
  const value = values.find(item => item.id === id)
  return value ? { ok: true, value } : { ok: false, error: `ID ${id} was not found` }
}

function describe(result: ApiResult<User>): string {
  return result.ok ? `${result.value.name}: ${result.value.role}` : result.error
}

async function mapAsync<T, R>(values: readonly T[], project: (value: T) => Promise<R>): Promise<R[]> {
  return Promise.all(values.map(project))
}

console.log(describe(findById(users, 2)))
console.log(await mapAsync(users, async user => user.name.toUpperCase()))

