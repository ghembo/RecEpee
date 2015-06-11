using System.Diagnostics;

namespace RecEpee.Utilities
{
    class Osal
    {
        public static void ShowFileWithDefaultProgram(string path)
        {
            Process.Start(path);
        }
    }
}
