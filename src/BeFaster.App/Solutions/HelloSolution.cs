namespace BeFaster.App.Solutions
{
    public static class HelloSolution
    {
        public static string Hello(string friendName)
        {
            if (string.IsNullOrWhiteSpace(friendName))
            {
                return "Hello, World!";
            }
            return string.Format("Hello, {0}!", friendName);
        }
    }
}
