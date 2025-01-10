using Microsoft.Playwright;

namespace Automation.Model.PageFragments;

/// <summary>
/// Base class for all page fragments.
/// Page fragments are reusable components that are used in multiple pages.
/// </summary>
public class BaseFragment
{
    public readonly IPage Page;

    public BaseFragment(IPage page)
    {
        Page = page;
    }
}
