using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderMaintenance.Data;

namespace OrderMaintenance.Pages;

public class OrdersModel(OrdersDbContext db) : PageModel
{
    public List<Order> Orders { get; private set; } = [];

    public async Task OnGetAsync()
    {
        // Populate the selector. JavaScript loads the selected order through the API.
        Orders = await db.Orders
            .AsNoTracking()
            .OrderBy(order => order.Id)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostLeaveAsync()
    {
        await HttpContext.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
