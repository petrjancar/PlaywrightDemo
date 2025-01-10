using Microsoft.Playwright;

namespace Automation.Model.PageObjects;

/// <summary>
/// Base class for all page objects.
/// Page objects are classes that represent a page in the application.
/// </summary>
public class BasePage
{
    public readonly IPage Page;
    public string? Url; // required keyword in C# 11 ensures Url is set in derived classes

    public BasePage(IPage page)
    {
        Page = page;
    }

    /// <summary>
    /// Navigates to the page.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task GotoAsync()
    {
        if (Url == null)
        {
            throw new NullReferenceException("Url was null.");         
        }

        await Page.GotoAsync(Url);
    }

    /// <summary>
    /// Navigates to the page and waits for URL to match.
    /// </summary>
    public async Task ExpectUrlAsync()
    {
        if (Url == null)
        {
            throw new NullReferenceException("Url was null.");         
        }

        await Assertions.Expect(Page).ToHaveURLAsync(Url);
    }

    /// <summary>
    /// Navigates to the page and waits for title to match.
    /// </summary>
    /// <param name="expected">The expected title.</param>
    public async Task ExpectTitleAsync(string expected)
    {
        await Assertions.Expect(Page).ToHaveTitleAsync(expected);
    }
}

