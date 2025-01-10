using Microsoft.Playwright;
using Automation.Model.Elements;

namespace Automation.Model.PageFragments;

public class Navigation : BaseFragment
{
    private readonly Button Lists;
    private readonly Button AdminLogin;
    private readonly Button Logout;

    public Navigation(IPage page) : base(page)
    {
        Lists = new Button(page, "#navtodolists");
        AdminLogin = new Button(page, "#navadminlogin");
        Logout = new Button(page, "#navadminlogout");
    }

    public async Task GoToListsAsync()
    {
        await Lists.ClickAsync();
    }

    public async Task GoToAdminLoginAsync()
    {
        await AdminLogin.ClickAsync();
    }

    public async Task LogoutAsync()
    {
        await Logout.ClickAsync();
    }
}