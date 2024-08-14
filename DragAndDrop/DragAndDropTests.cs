using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;

namespace DragAndDrop
{
    public class DragAndDropTests
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
        public void DragAndDropTest()
        {
            AppiumElement dragAndDRopLink = driver.FindElement(MobileBy.AccessibilityId("Drag and Drop"));
            dragAndDRopLink.Click();

            AppiumElement firstRedDot = driver.FindElement(By.Id("drag_dot_1"));

            AppiumElement secondRedDot = driver.FindElement(By.Id("drag_dot_2"));

            var scriptArgs = new Dictionary<string, object>
            {
                { "elementId", firstRedDot.Id },
                { "endX", secondRedDot.Location.X + (secondRedDot.Size.Width / 2) },
                { "endY", secondRedDot.Location.Y + (secondRedDot.Size.Height / 2) },
                { "speed", 2500 }
            };

            driver.ExecuteScript("mobile: dragGesture", scriptArgs);

            AppiumElement dropResult = driver.FindElement(By.Id("drag_result_text"));

            Assert.That(dropResult.Text, Is.EqualTo("Dropped!"), "The drag and drop action was not successfully.");
        }
    }
}