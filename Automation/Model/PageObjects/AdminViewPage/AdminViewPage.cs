using Microsoft.Playwright;
using Automation.Model.PageFragments.Navigation;

namespace Automation.Model.PageObjects.AdminViewPage;

public partial class AdminViewPage : BasePage
{
    public override string Url { get; } = "https://eviltester.github.io/simpletodolist/adminview.html";

    public readonly Navigation Navigation;
    private readonly Elements.Header Header;

    public AdminViewPage(IPage page) : base(page)
    {
        Navigation = new Navigation(page);
        Header = new Elements.Header(page, "h1");
    }
}