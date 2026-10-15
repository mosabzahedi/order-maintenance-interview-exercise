namespace OrderMaintenance.Data;

public class Order
{
    public int Id { get; set; }

    public string TrackingNumber { get; set; } = "";

    public string CustomerDisplayName { get; set; } = "";

    public string Status { get; set; } = "Pending";

    public string CustomerNote { get; set; } = "";

    public int? UpdatedByDemoUserId { get; set; }
}
