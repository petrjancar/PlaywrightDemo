using Microsoft.Playwright;
using Automation.Model.Elements;
using Automation.Model.PageFragments;
using Automation.Model.FormData;

namespace Automation.Model.PageObjects;

public class AdminLogin : BasePage
{
    public readonly Navigation Navigation;
    private readonly Elements.Header Header;
    private readonly Input Username;
    private readonly Input Password;
    private readonly Button Login;
    private readonly Checkbox RememberMe;

    public AdminLogin(IPage page) : base(page)
    {
        // Url
        Url = "https://eviltester.github.io/simpletodolist/adminlogin.html";
        // Fragments
        Navigation = new Navigation(page);
        // Elements
        Header = new Elements.Header(page, "h1");
        Username = new Input(page, "[name='username']");
        Password = new Input(page, "[name='password']");
        Login  = new Button(page, "#login");
        RememberMe = new Checkbox(page, "[name='remember']");
    }

    /// <summary>
    /// High level method to login.
    /// </summary>
    /// <param name="adminLoginForm">Admin login form data.</param>
    /// <returns></returns>
    public async Task LoginAsync(AdminLoginFormData adminLoginForm)
    {
        await Username.SetTextAsync(adminLoginForm.Username);
        await Password.SetTextAsync(adminLoginForm.Password);
        if (adminLoginForm.RememberMe)
        {
            await RememberMe.CheckAsync();
        }
        else
        {
            await RememberMe.UncheckAsync();
        }

        await Login.ClickAsync();
    }

    public async Task<string?> HeaderContentAsync()
    {
        return await Header.GetTextAsync();
    }
}