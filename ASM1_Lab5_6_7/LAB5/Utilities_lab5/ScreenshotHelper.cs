using System;
using System.IO;
using OpenQA.Selenium;

namespace LAB5.Utilities
{
    public static class ScreenshotHelper
    {
        public static string CaptureFailure(IWebDriver driver, string testName)
        {
            if (driver == null)
            {
                return string.Empty;
            }

            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver == null)
            {
                return string.Empty;
            }

            string folderPath = Path.Combine(AppContext.BaseDirectory, "screenshots");
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            string safeTestName = testName;
            if (string.IsNullOrEmpty(safeTestName))
            {
                safeTestName = "FailedTest";
            }

            char[] invalidChars = Path.GetInvalidFileNameChars();
            int index = 0;
            while (index < invalidChars.Length)
            {
                safeTestName = safeTestName.Replace(invalidChars[index], '_');
                index = index + 1;
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = safeTestName + "_" + timestamp + ".png";
            string fullPath = Path.Combine(folderPath, fileName);

            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            return fullPath;
        }
    }
}