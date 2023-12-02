public static class Utils
{
    public static string GetInput(string fileName)
    {
        return File.ReadAllText(fileName);
    }
}