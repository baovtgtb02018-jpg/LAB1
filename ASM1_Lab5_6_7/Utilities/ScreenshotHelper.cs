using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ASM.Utilities
{
    public static class ScreenshotHelper
    {
        public static string Capture(IWebDriver driver, string testName)
        {
            string safeName = SanitizeFileName(testName);
            string screenshotDir = Path.Combine(AppContext.BaseDirectory, "screenshots");
            Directory.CreateDirectory(screenshotDir);

            string fileName = $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss_fff}.png";
            string filePath = Path.Combine(screenshotDir, fileName);

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);
            TestContext.AddTestAttachment(filePath);

            return filePath;
        }

        private static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name.Length > 100 ? name[..100] : name;
        }
    }
}
