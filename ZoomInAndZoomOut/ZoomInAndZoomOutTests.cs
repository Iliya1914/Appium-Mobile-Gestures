using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Interactions;
using System.Reflection;
using PointerInputDevice = OpenQA.Selenium.Interactions.PointerInputDevice;

namespace ZoomInAndZoomOut
{
    public class ZoomInAndZoomOutTests
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

            ScrollToText("WebView");

            AppiumElement webViewLink = driver.FindElement(MobileBy.AccessibilityId("WebView"));
            webViewLink.Click();
        }

        [Test]
        public void ZoomInAndOutTest()
        {
            PerformZoomIn(408, 901, 370, 521, 422, 1165, 472, 1408);

            PerformZoomOut(482, 1362, 475, 996, 454, 524, 433, 964);

        }

        private void PerformZoomIn(int startX1, int startY1, int endX1, int endY1, int startX2, int startY2, int endX2, int endY2)
        {
            var finge1 = new PointerInputDevice(PointerKind.Touch);
            var finge2 = new PointerInputDevice(PointerKind.Touch);

            var zoomIn1 = new ActionSequence(finge1);

            zoomIn1.AddAction(finge1.CreatePointerMove(CoordinateOrigin.Viewport, startX1, startY1, TimeSpan.Zero));
            zoomIn1.AddAction(finge1.CreatePointerDown(MouseButton.Left));
            zoomIn1.AddAction(finge1.CreatePointerMove(CoordinateOrigin.Viewport, endX1, endY1, TimeSpan.FromSeconds(1)));
            zoomIn1.AddAction(finge1.CreatePointerUp(MouseButton.Left));

            var zoomIn2 = new ActionSequence(finge2);

            zoomIn2.AddAction(finge2.CreatePointerMove(CoordinateOrigin.Viewport, startX2, startY2, TimeSpan.Zero));
            zoomIn2.AddAction(finge2.CreatePointerDown(MouseButton.Left));
            zoomIn2.AddAction(finge2.CreatePointerMove(CoordinateOrigin.Viewport, endX2, endY2, TimeSpan.FromSeconds(1)));
            zoomIn2.AddAction(finge2.CreatePointerUp(MouseButton.Left));

            driver.PerformActions(new List<ActionSequence> { zoomIn1, zoomIn2 });
        }

        private void PerformZoomOut(int startX1, int startY1, int endX1, int endY1, int startX2, int startY2, int endX2, int endY2)
        {
            var finger1 = new PointerInputDevice(PointerKind.Touch);
            var finger2 = new PointerInputDevice(PointerKind.Touch);

            var zoomOut1 = new ActionSequence(finger1);

            zoomOut1.AddAction(finger1.CreatePointerMove(CoordinateOrigin.Viewport, startX1, startY1, TimeSpan.Zero));
            zoomOut1.AddAction(finger1.CreatePointerDown(MouseButton.Left));
            zoomOut1.AddAction(finger1.CreatePointerMove(CoordinateOrigin.Viewport, endX1, endY1, TimeSpan.FromSeconds(1)));
            zoomOut1.AddAction(finger1.CreatePointerUp(MouseButton.Left));

            var zoomOut2 = new ActionSequence(finger2);

            zoomOut2.AddAction(finger2.CreatePointerMove(CoordinateOrigin.Viewport, startX2, startY2, TimeSpan.Zero));
            zoomOut2.AddAction(finger2.CreatePointerDown(MouseButton.Left));
            zoomOut2.AddAction(finger2.CreatePointerMove(CoordinateOrigin.Viewport, endX2, endY2, TimeSpan.FromSeconds(1)));
            zoomOut2.AddAction(finger2.CreatePointerUp(MouseButton.Left));

            driver.PerformActions(new List<ActionSequence> { zoomOut1, zoomOut2 });
        }
        private void ScrollToText(string text)
        {
            driver.FindElement(MobileBy.AndroidUIAutomator($"new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"{text}\"))"));
        }
    }
}