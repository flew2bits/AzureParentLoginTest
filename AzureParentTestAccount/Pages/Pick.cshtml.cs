using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureParentTestAccount.Pages;

public class Pick : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public void OnGet()
    {
        ReturnUrl = Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : null;
    }
}