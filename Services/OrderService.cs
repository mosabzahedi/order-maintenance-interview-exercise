using Microsoft.EntityFrameworkCore;
using OrderMaintenance.Contracts;
using OrderMaintenance.Data;

namespace OrderMaintenance.Services;

public class OrderService
{
    private readonly OrdersDbContext db;

    public OrderService(OrdersDbContext db)
    {
        this.db = db;
    }

    public async Task<OrderView?> GetAsync(int id)
    {
        // Return only the fields displayed in the order form.
        return await db.Orders
            .AsNoTracking()
            .Where(order => order.Id == id)
            .Select(order => new OrderView(
                order.Id,
                order.TrackingNumber,
                order.CustomerDisplayName,
                order.Status,
                order.CustomerNote))
            .SingleOrDefaultAsync();
    }

    public async Task<OrderView?> UpdateAsync(int id, UpdateOrderRequest request, int actorId)
    {
        var order = await db.Orders.FindAsync(id);
        if (order is null)
        {
            return null;
        }

        order.Status = request.Status;
        order.CustomerNote = request.CustomerNote;
        order.UpdatedByDemoUserId = actorId;

        await db.SaveChangesAsync();

        return new OrderView(
            order.Id,
            order.TrackingNumber,
            order.CustomerDisplayName,
            order.Status,
            order.CustomerNote);
    }
}
