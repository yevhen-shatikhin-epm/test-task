using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Scenario1.Support;

public static class WebDriverWaits
{
    public static void WaitForUrl(this IWebDriver driver, string url, TimeSpan timeout)
    {
        var wait = new WebDriverWait(driver, timeout);
        wait.Until(d => d.Url == url);
    }
}