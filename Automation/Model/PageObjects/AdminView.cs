using Microsoft.Playwright;
using Automation.Model.Elements;
using Automation.Model.PageFragments;

namespace Automation.Model.PageObjects;

public class AdminView : BasePage
{
    public readonly Navigation Navigation;
    private readonly Elements.Header Header;

    public AdminView(IPage page) : base(page)
    {
        // Url
        Url = "https://eviltester.github.io/simpletodolist/adminview.html";
        // Fragments
        Navigation = new Navigation(page);
        // Elements
        Header = new Elements.Header(page, "h1");
    }

    public async Task<string?> HeaderContentAsync()
    {
        return await Header.GetTextAsync();
    }
}