using Automation.Model.PageObjects.AdminLoginPage.Data;

namespace Automation.Model.PageObjects.AdminLoginPage;

public partial class AdminLoginPage
{
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
}
