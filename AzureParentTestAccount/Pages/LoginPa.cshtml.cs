using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureParentTestAccount.Pages;

public class LoginPa : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }
    
    public IActionResult OnGet() => Url.IsLocalUrl(ReturnUrl) ? Redirect(ReturnUrl): RedirectToPage("/Index");}