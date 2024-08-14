using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Interactions;

namespace Swipe
{
    public class SwipeTests
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

            var androidOptions = new OpenQA.Selenium.Appium.AppiumOptions
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
            driver.FindElement(OpenQA.Selenium.Appium.MobileBy.AccessibilityId("Views")).Click();
        }

        [Test]
        public void SwipeTest()
        {
            AppiumElement galleryLink = driver.FindElement(MobileBy.AccessibilityId("Gallery"));
            galleryLink.Click();

            AppiumElement photos1Link = driver.FindElement(MobileBy.AccessibilityId("1. Photos"));
            photos1Link.Click();

            AppiumElement firstPicture = driver.FindElements(By.ClassName("android.widget.ImageView"))[0];

            var actions = new Actions(driver);
            var swipe = actions.ClickAndHold(firstPicture)
                .MoveByOffset(-200, 0)
                .Release()
                .Build();

            swipe.Perform();

            AppiumElement thirdPicture = driver.FindElements(By.ClassName("android.widget.ImageView"))[2];

            Assert.That(thirdPicture, Is.Not.Null, "The third photo was not found.");
        }

    }
}