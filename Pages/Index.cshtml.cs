using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoBasic.Pages;

public class IndexModel : PageModel
{
    private static readonly List<TodoItem> Items = [];
    private static int nextId = 1;

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
                Title = NewTodoTitle.Trim()
            });
        }

        return RedirectToPage();
    }

    public IActionResult OnPostToggle(int id)
    {
        var item = Items.FirstOrDefault(todo => todo.Id == id);

        if (item is not null)
        {
            item.IsDone = !item.IsDone;
        }

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var item = Items.FirstOrDefault(todo => todo.Id == id);

        if (item is not null)
        {
            Items.Remove(item);
        }

        return RedirectToPage();
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
