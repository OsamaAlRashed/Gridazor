using Gridazor.Demo.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Gridazor.Demo.Controllers;

public class CultureController : Controller
{
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        if (!LanguageKeys.Get().Contains(culture))
        {
            culture = LanguageKeys.En;
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            }
        );

        return LocalRedirect(returnUrl);
    }
}
