using Microsoft.Playwright;
using Automation.Model.Elements;

namespace Automation.Model.PageFragments.Navigation;

public partial class Navigation : BaseFragment
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
}