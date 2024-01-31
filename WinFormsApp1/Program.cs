using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{

    internal static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
    public static class Helpers
    {
        static public string path;
        static public Dictionary<int, string> lineorder = new Dictionary<int, string>();
        static public List<string> untranslated = new List<string>();
        static public List<string> translationDict = new List<string>();
        public static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");
        public static bool IsChinese(string s)
        {
            return cjkCharRegex.IsMatch(s);
        }
        public static string CustomEscape(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }
        public static string CustomUnescape(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");
        }
        public static Dictionary<string, string> FileToDictionary(string dir)
        {

            

            Dictionary<string, string> dict = new Dictionary<string, string>();

            IEnumerable<string> lines = File.ReadLines(dir);

            foreach (string line in lines)
            {
                Console.WriteLine(line);
                try
                {
                    var arr = line.Split('¤');
                    if (arr[0] != null && arr[1] != null && arr[0] != "" && arr[1] != "")
                    {
                        if (arr[0] != arr[1])
                        {
                            var pair = new KeyValuePair<string, string>(Regex.Replace(arr[0], @"\t|\n|\r", ""), arr[1]);

                            if (!dict.ContainsKey(pair.Key))

                                dict.Add(pair.Key, pair.Value);
                        }




                        else
                        {
                            //Debug.Log("Not touching this with a 10ft pole : " + arr[0]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("*****WARNING : *****" + e.ToString());
                }
            }
                

            
        


            return dict;

        }
    }
}