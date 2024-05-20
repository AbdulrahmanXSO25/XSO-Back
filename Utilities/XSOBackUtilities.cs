namespace XSOBack
{
    public static class XSOBackUtilities
    {
        public static void XSOBackLog(string log, Type type = null)
        {
            if (type == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[HOST]: ");
            }
            else
            {
                var className = type.Name;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"[{className}]: ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(log);

            Console.ResetColor();
        }
    }

}