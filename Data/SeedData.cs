using Microsoft.EntityFrameworkCore;

namespace OrderMaintenance.Data;

public static class SeedData
{
    public static async Task InitializeAsync(OrdersDbContext db)
    {
        // Insert the sample once, keeping any edits made during previous runs.
        if (!await db.Orders.AnyAsync(x => x.TrackingNumber == "DEMO-1001"))
        {
            db.Orders.Add(new Order
            {
                TrackingNumber = "DEMO-1001",
                CustomerDisplayName = "Maple Workshop",
                CustomerNote = "Deliver to reception."
            });

            await db.SaveChangesAsync();
        }
    }
}
