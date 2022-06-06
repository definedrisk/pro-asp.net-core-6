// using Microsoft.AspNetCore.Mvc;
// using LanguageFeatures.Models;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ViewResult> Index()
        {
            // long? length = await MyAsyncMethods.GetPageLength();
            // return View(new string[] { $"Length: {length}" });

            List<string> output = new List<string>();
            // foreach (long? len in await MyAsyncMethods.GetPageLengths(
            await foreach (long? len in MyAsyncMethods.GetPageLengths(
                output,
                "apress.com", "microsoft.com", "amazon.com"))
                {
                    output.Add($"Page Length: {len}");
                }
            return View(output);
        }
    }
}