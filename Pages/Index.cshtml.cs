using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderMaintenance.Data;

namespace OrderMaintenance.Pages;

public class IndexModel(OrdersDbContext db, IPasswordHasher<DemoUser> hasher) : PageModel
{
    [BindProperty]
    [Required]
    [MinLength(10)]
    public string Secret { get; set; } = "";

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Serialize the small demo user lookup/create so simultaneous first use agrees.
        await using var transaction = await db.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable);

        var users = await db.DemoUsers
            .FromSqlRaw("SELECT * FROM DemoUsers WITH (UPDLOCK, HOLDLOCK)")
            .ToListAsync();

        // Verify against framework hashes; the submitted secret is never stored.
        var user = users.FirstOrDefault(user =>
        {
            var result = hasher.VerifyHashedPassword(user, user.SecretHash, Secret);
            return result != PasswordVerificationResult.Failed;
        });

        if (user is null)
        {
            user = new DemoUser();
            user.SecretHash = hasher.HashPassword(user, Secret);
            db.DemoUsers.Add(user);
            await db.SaveChangesAsync();
        }

        await transaction.CommitAsync();

        // Store only the selected demo user's ID in the protected cookie.
        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())],
            CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToPage("/Orders");
    }
}
