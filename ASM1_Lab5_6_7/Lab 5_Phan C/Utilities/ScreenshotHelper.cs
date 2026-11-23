using OpenQA.Selenium;

namespace SeleniumFramework.Utilities;

public static class ScreenshotHelper
{
    public static string Capture(IWebDriver driver, string testName)
    {
        try
        {
            var dir = Path.Combine(AppContext.BaseDirectory, "screenshots");
            Console.WriteLine($"Đang chụp ảnh vào thư mục: {dir}");
            Directory.CreateDirectory(dir); Directory.CreateDirectory(dir);

            var safeName = string.Join("_",
                testName.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var filePath = Path.Combine(dir, fileName);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);
            return filePath;
        }
        catch
        {
            // Chụp ảnh lỗi KHÔNG được ngăn Quit() ở finally
            return string.Empty;
        }
    }
}