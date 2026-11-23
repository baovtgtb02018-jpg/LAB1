namespace ASM.Config
{
    public class TestConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Browser { get; set; } = "Chrome";
        public int TimeoutSeconds { get; set; } = 10;
        public bool Headless { get; set; } = true;
        public int WindowWidth { get; set; } = 1920;
        public int WindowHeight { get; set; } = 1080;
    }
}
