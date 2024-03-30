using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Scenario1.Support.PageObjects;

public class LogInPage
{
    public const string Url = @"https://qa.sorted.com/newtrack";

    private IWebDriver _driver;

    [FindsBy(How.XPath, "//form[@id='loginForm']/input[1]")]
    private IWebElement _usernameInput;

    [FindsBy(How.XPath, "//form[@id='loginForm']/input[2]")]
    private IWebElement _passwordInput;

    [FindsBy(How.XPath, "//form[@id='loginForm']/submit[1]")]
    private IWebElement _logInButton;

    [FindsBy(How.XPath, "//form[@id='loginForm']/submit[2]")]
    private IWebElement _registerButton;

    public LogInPage(IWebDriver driver)
    {
        _driver = driver;
        PageFactory.InitElements(driver, this);
    }

    public void EnterUsername(string username) => _usernameInput.SendKeys(username);

    public void EnterPassword(string password) => _passwordInput.SendKeys(password);

    public void SubmitLogin() => _logInButton.Click();

    public void SubmitRegister() => _registerButton.Click();

    public void LogIn(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        SubmitLogin();
    }

    public void Register(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        SubmitRegister();
    }
}