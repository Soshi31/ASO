using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoBasic.Pages;

public class IndexModel : PageModel
{
    private static readonly List<TodoItem> Items = [];
    private static readonly object FileLock = new();
    private static bool isLoaded;
    private static int nextId = 1;
    private readonly string databasePath;

    public IndexModel(IWebHostEnvironment environment)
    {
        databasePath = Path.Combine(environment.ContentRootPath, "Data", "todos.txt");
        LoadTodos();
    }

    [BindProperty]
    public string? NewTodoTitle { get; set; }

    public IReadOnlyList<TodoItem> Todos => Items;

    public void OnGet()
    {
    }

    public IActionResult OnPostAdd()
    {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle))
        {
            Items.Add(new TodoItem
            {
                Id = nextId++,
                Title = NormalizeTitle(NewTodoTitle)
            });

            SaveTodos();
        }

        return RedirectToPage();
    }

    public IActionResult OnPostToggle(int id)
    {
        var item = Items.FirstOrDefault(todo => todo.Id == id);

        if (item is not null)
        {
            item.IsDone = !item.IsDone;
            SaveTodos();
        }

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var item = Items.FirstOrDefault(todo => todo.Id == id);

        if (item is not null)
        {
            Items.Remove(item);
            SaveTodos();
        }

        return RedirectToPage();
    }

    private void LoadTodos()
    {
        lock (FileLock)
        {
            if (isLoaded)
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);

            if (!System.IO.File.Exists(databasePath))
            {
                System.IO.File.WriteAllText(databasePath, string.Empty);
                isLoaded = true;
                return;
            }

            foreach (var line in System.IO.File.ReadAllLines(databasePath))
            {
                var parts = line.Split('|', 3);

                if (parts.Length != 3 || !int.TryParse(parts[0], out var id) || !bool.TryParse(parts[1], out var isDone))
                {
                    continue;
                }

                Items.Add(new TodoItem
                {
                    Id = id,
                    IsDone = isDone,
                    Title = parts[2]
                });
            }

            if (Items.Count > 0)
            {
                nextId = Items.Max(todo => todo.Id) + 1;
            }

            isLoaded = true;
        }
    }

    private void SaveTodos()
    {
        lock (FileLock)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);

            var lines = Items.Select(todo => $"{todo.Id}|{todo.IsDone}|{NormalizeTitle(todo.Title)}");
            System.IO.File.WriteAllLines(databasePath, lines);
        }
    }

    private static string NormalizeTitle(string title)
    {
        return title.Trim().Replace("\r", " ").Replace("\n", " ");
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
