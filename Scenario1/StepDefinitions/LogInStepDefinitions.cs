using OpenQA.Selenium;
using Scenario1.Drivers;
using Scenario1.Support;
using Scenario1.Support.PageObjects;

namespace Scenario1.StepDefinitions
{
    [Binding]
    public class LogInStepDefinitions
    {
        private readonly IWebDriver _driver;

        private readonly LogInPage _logInPage;

        private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

        private const string RequiresAuthenticationPageUrl = @"https://qa.sorted.com/cabinet";

        private const string NewUserUsername = "john_smith@sorted.com";

        private const string NewUserPassword = "Pa55w0rd";

        public LogInStepDefinitions(BrowserDriver browserDriver)
        {
            _driver = browserDriver.Current;

            _driver.Manage().Window.Maximize();

            _logInPage = new LogInPage(_driver);
        }

        [Given(@"I’m not logged in with a genuine user")]
        public void GivenIMNotLoggedInWithAGenuineUser()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
        }

        [When(@"I navigate to any page on the tracking site")]
        public void WhenINavigateToAnyPageOnTheTrackingSite()
        {
            _driver.Navigate().GoToUrl(RequiresAuthenticationPageUrl);
        }

        [Then(@"I am presented with a login screen")]
        public void ThenIAmPresentedWithALoginScreen()
        {
            var expectedUrl = LogInPage.Url;

            var actualUrl = _driver.Url;

            _driver.WaitForUrl(expectedUrl, _defaultTimeout);

            actualUrl.Should().Be(expectedUrl);
        }

        [Given(@"valid user credentials are already registered")]
        public void GivenValidUserCredentialsAreAlreadyRegistered()
        {
            _driver.Navigate().GoToUrl(LogInPage.Url);

            // register new user
            _logInPage.Register(NewUserUsername, NewUserPassword);
        }

        [Given(@"I'm on the login screen")]
        public void GivenImOnTheLoginScreen()
        {
            _driver.Navigate().GoToUrl(LogInPage.Url);
        }

        [When(@"I enter a valid username and password and submit")]
        public void WhenIEnterAValidUsernameAndPasswordAndSubmit()
        {
            _logInPage.LogIn(NewUserUsername, NewUserPassword);
        }

        [Then(@"I am logged in successfully")]
        public void ThenIAmLoggedInSuccessfully()
        {
            var expectedUrl = @"https://qa.sorted.com/newtrack/loginSuccess";

            var actualUrl = _driver.Url;

            _driver.WaitForUrl(expectedUrl, _defaultTimeout);

            actualUrl.Should().Be(expectedUrl);
        }
    }
}