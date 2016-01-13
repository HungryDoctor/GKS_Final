using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GKS
{
    public static class IO
    {
        public static Dictionary<int, string> ReadStringsFromFile(string path)
        {
            try
            {
                StreamReader reader = new StreamReader(path, Encoding.Default);
                string line;
                Dictionary<int, string> strings = new Dictionary<int, string>();

                int counter = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    strings.Add(counter, line.Trim().ToUpper());
                    counter++;
                }

                return strings;
            }
            catch
            {
                return null;
            }
        }

    }
}