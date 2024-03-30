using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Scenario1.Drivers;

public class BrowserDriver : IDisposable
{
    private readonly Lazy<IWebDriver> _currentWebDriverLazy;
    private bool _isDisposed;

    public BrowserDriver()
    {
        _currentWebDriverLazy = new Lazy<IWebDriver>(CreateWebDriver);
    }

    public IWebDriver Current => _currentWebDriverLazy.Value;

    private IWebDriver CreateWebDriver()
    {
        //We use the Chrome browser
        var chromeDriverService = ChromeDriverService.CreateDefaultService();

        var chromeOptions = new ChromeOptions();

        var chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);

        chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        return chromeDriver;
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        if (_currentWebDriverLazy.IsValueCreated)
        {
            Current.Quit();
        }

        _isDisposed = true;
    }
}