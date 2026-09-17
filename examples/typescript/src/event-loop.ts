// Run: npm run event-loop
// Synchronous code runs first; a Promise callback is a microtask; a timer is a task.
const events: string[] = []
events.push('sync')
setTimeout(() => events.push('timer'), 0)
Promise.resolve().then(() => events.push('promise'))

setTimeout(() => {
  const actual = events.join(' -> ')
  const expected = 'sync -> promise -> timer'
  if (actual !== expected) throw new Error(`Expected ${expected}, got ${actual}`)
  console.log(actual)
}, 10)
