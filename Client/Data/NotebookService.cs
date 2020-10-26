using System;
using System.Collections.Generic;
using blazoract.Shared;
using Blazored.LocalStorage;
using System.Threading;
using System.Threading.Tasks;

namespace blazoract.Client.Data
{
    public class NotebookService
    {
        public event Action OnChange;

        private ILocalStorageService _storage;

        public NotebookService(ILocalStorageService storage)
        {
            _storage = storage;
            var id = Guid.NewGuid().ToString("N");
            var title = "Default Notebook";
            var notebook = new Notebook(id, title);
            var initialContent = new List<Cell>();
            for (var i = 0; i < 100; i++)
            {
                initialContent.Add(new Cell($"{i} * {i}", i));
            }
            notebook.Cells = initialContent;
            _storage.SetItemAsync("_default_notebook", notebook);
        }
        public async Task<List<Cell>> GetInitialContent()
        {
            return await _storage.GetItemAsync<List<Cell>>("_default_notebook");
        }

        public async Task<List<Cell>> GetById(string id)
        {
            return await _storage.GetItemAsync<List<Cell>>(id);
        }

        public async Task<string> CreateNewNotebook()
        {
            var id = Guid.NewGuid().ToString("N");
            var title = "Just a test";
            var notebook = new Notebook(id, title);
            notebook.Cells = new List<Cell>() { new Cell("", 0) };
            await _storage.SetItemAsync(id, notebook);
            return id;
        }

        public async Task<Notebook> AddCell(string id, string content, CellType type, int position)
        {
            var notebook = await _storage.GetItemAsync<Notebook>(id);
            notebook.Cells.Insert(position, new Cell(content, position, type));
            await _storage.SetItemAsync(id, notebook);
            OnChange.Invoke();
            return notebook;
        }

    }
}