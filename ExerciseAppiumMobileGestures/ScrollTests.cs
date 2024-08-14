using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;

namespace Scroll
{
    public class ScrollTests
    {
        private AndroidDriver driver;
        private AppiumLocalService appiumLocalService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            appiumLocalService = new AppiumServiceBuilder()
                .WithIPAddress("127.0.0.1")
                .UsingPort(4723)
                .Build();
            appiumLocalService.Start();

            var androidOptions = new AppiumOptions
            {
                PlatformName = "Android",
                AutomationName = "UIAutomator2",
                DeviceName = "MyDevice",
                App = "C:\\ApiDemos-debug.apk",
                PlatformVersion = "14"
            };

            driver = new AndroidDriver(appiumLocalService, androidOptions);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() 
        { 
            driver?.Quit();
            driver?.Dispose();
            appiumLocalService?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            driver.FindElement(MobileBy.AccessibilityId("Views")).Click();
        }

        [Test]
        public void ScrollTest()
        {
            ScrollToText("Lists");

            AppiumElement listsLinks = driver.FindElement(MobileBy.AccessibilityId("Lists"));
            listsLinks.Click();

            AppiumElement singleChoiceListElement = driver.FindElement(MobileBy.AccessibilityId("10. Single choice list"));

            Assert.That(singleChoiceListElement, Is.Not.Null, "The expected element was not found.");
        }

        private void ScrollToText(string text)
        {
            driver.FindElement(MobileBy.AndroidUIAutomator($"new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"{text}\"))"));
        }
    }
}