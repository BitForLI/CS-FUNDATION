const form = document.querySelector('#task-form');
const input = document.querySelector('#task-title');
const list = document.querySelector('#task-list');
const status = document.querySelector('#status');

/** @type {{ id: string, title: string, done: boolean }[]} */
let tasks = load();
render();

form.addEventListener('submit', (event) => {
  event.preventDefault();
  const title = input.value.trim();
  if (!title) return;
  tasks = [...tasks, { id: crypto.randomUUID(), title, done: false }];
  input.value = '';
  saveAndRender(`Added ${title}`);
});

list.addEventListener('click', (event) => {
  const button = event.target.closest('button[data-action]');
  if (!button) return;
  const { id, action } = button.dataset;
  if (action === 'toggle') tasks = tasks.map(task => task.id === id ? { ...task, done: !task.done } : task);
  if (action === 'remove') tasks = tasks.filter(task => task.id !== id);
  saveAndRender('Task list updated');
});

function load() {
  try { return JSON.parse(localStorage.getItem('browser-tasks') ?? '[]'); }
  catch { return []; }
}

function saveAndRender(message) {
  localStorage.setItem('browser-tasks', JSON.stringify(tasks));
  status.textContent = message;
  render();
}

function render() {
  list.replaceChildren(...tasks.map(task => {
    const row = document.createElement('li');
    row.className = task.done ? 'done' : '';
    const title = document.createElement('span');
    title.textContent = task.title;
    const actions = document.createElement('span');
    actions.innerHTML = `<button data-action="toggle" data-id="${task.id}">Toggle</button> <button data-action="remove" data-id="${task.id}">Remove</button>`;
    row.append(title, actions);
    return row;
  }));
}

