using Microsoft.Playwright;
using Automation.Model.Elements;
using Automation.Model.PageFragments.Navigation;

namespace Automation.Model.PageObjects.AdminLoginPage;

public partial class AdminLoginPage : BasePage
{
    public override string Url { get; } = "https://eviltester.github.io/simpletodolist/adminlogin.html";

    public readonly Navigation Navigation;
    private readonly Elements.Header Header;
    private readonly TextBox Username;
    private readonly TextBox Password;
    private readonly Button Login;
    private readonly Checkbox RememberMe;

    public AdminLoginPage(IPage page) : base(page)
    {
        Navigation = new Navigation(page);
        Header = new Elements.Header(page, "h1");
        Username = new TextBox(page, "[name='username']");
        Password = new TextBox(page, "[name='password']");
        Login  = new Button(page, "#login");
        RememberMe = new Checkbox(page, "[name='remember']");
    }
}