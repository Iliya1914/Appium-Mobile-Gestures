using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Interactions;
using System.Drawing;

namespace Sliding
{
    public class SlidingTests
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
        public void SlidingTest()
        {
            ScrollToText("Seek Bar");

            AppiumElement seekBarLink = driver.FindElement(MobileBy.AccessibilityId("Seek Bar"));
            seekBarLink.Click();

            MoveSeekBarWithInspectorCoordinates(546, 300, 1052, 300);

            AppiumElement seekBarText = driver.FindElement(By.Id("progress"));

            Assert.That(seekBarText.Text, Is.EqualTo("100 from touch=true"), "The seek bar text was not as expected.");
        }

        private void MoveSeekBarWithInspectorCoordinates(int startX, int startY, int endX, int endY)
        {
            var finger = new OpenQA.Selenium.Appium.Interactions.PointerInputDevice(PointerKind.Touch);

            var start = new Point(startX, startY);
            var end = new Point(endX, endY);

            var swipe = new ActionSequence(finger);

            swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, startX, startY, TimeSpan.Zero));
            swipe.AddAction(finger.CreatePointerDown(MouseButton.Left));
            swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, endX, endY, TimeSpan.FromSeconds(1)));
            swipe.AddAction(finger.CreatePointerUp(MouseButton.Left));

            driver.PerformActions(new List<ActionSequence> { swipe });
        }

        private void ScrollToText(string text)
        {
            driver.FindElement(MobileBy.AndroidUIAutomator($"new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"{text}\"))"));
        }
    }
}