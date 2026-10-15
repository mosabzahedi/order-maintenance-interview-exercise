using System.ComponentModel.DataAnnotations;

namespace OrderMaintenance.Contracts;

// Values returned to the order form after loading or saving.
public record OrderView(
    int Id,
    string TrackingNumber,
    string CustomerDisplayName,
    string Status,
    string CustomerNote);

// Values submitted by the order form.
public record UpdateOrderRequest(
    [Required]
    [RegularExpression("^(Pending|Approved)$")]
    string Status,

    [Required(AllowEmptyStrings = true)]
    [StringLength(500)]
    string CustomerNote);
