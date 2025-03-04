namespace Automation.Model.PageFragments.Navigation;

public partial class Navigation
{
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
