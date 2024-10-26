using MiniExcelLibs;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text.RegularExpressions;
using static WinFormsApp1.DownloadSheet;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public static string rootpath;
        static bool flawed = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var configdict = new Dictionary<string, string>();
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.txt")))
            {
                var config = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "config.txt"));
                foreach (var line in config)
                {
                    configdict.Add(line.Split('=')[0], line.Split('=')[1]);
                    if (configdict.ContainsKey("DEFAULTPATH"))
                    {
                        textBox1.Text = configdict["DEFAULTPATH"];
                        rootpath = textBox1.Text;
                    }
                }
            }
            if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations")))
            {
                Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations"));
            }
            if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet")))
            {
                Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet"));
            }
            if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "GameFilesBackUp")))
            {
                Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "GameFilesBackUp"));
            }
            if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "NewFiles")))
            {
                Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "NewFiles"));
            }
            if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines")))
            {
                Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt")))
            {
                File.Delete(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
            }
            DownloadSheet.Main(@"https://docs.google.com/spreadsheets/d/1BoqPuD8CIJmfDyvMHAkvzphGkaSrFr3fZDkkQAViZLg/gviz/tq?tqx=out:csv&sheet=Final", Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
            DownloadSheet.MainCommon(@"https://docs.google.com/spreadsheets/d/1BoqPuD8CIJmfDyvMHAkvzphGkaSrFr3fZDkkQAViZLg/gviz/tq?tqx=out:csv&sheet=Common", Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Common.txt"));
            textBox2.Text = "Translations stored in " + textBox1.Text + @"\Translations\DownloadedFromSpreadsheet" + " as Translations.txt";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var TranslationDict = Helpers.FileToDictionary(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
            textBox3.Clear();
            var backupflag = checkBox1.Checked;
            var BackupFolder = new DirectoryInfo(Path.Combine(textBox1.Text, "Translations", "GameFilesBackUp"));
            var GameFilesFolder = new DirectoryInfo(Path.Combine(textBox1.Text, "game"));

            foreach (var file in GameFilesFolder.GetFiles())
            {
                if (file.Name.EndsWith(".rpy"))
                {
                    textBox3.AppendText("Now processing " + file.FullName + Environment.NewLine);
                    var lines = File.ReadAllLines(file.FullName);
                    string pattern = "\"([^\"]*)\"";
                    foreach (var line in lines)
                    {
                        var matches = Regex.Matches(line, pattern);
                        foreach (var match in matches)
                        {
                            if (Helpers.IsChinese(match.ToString()))
                            {
                                Helpers.untranslated.Add(match.ToString().Replace("\"", ""));
                            }
                        }
                    }

                }
            }

            if (File.Exists(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "UN.txt")))
            {
                File.Delete(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "UN.txt"));
            }
            using (StreamWriter sw = File.AppendText(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "UN.txt")))
            {
                foreach (var s in Helpers.untranslated.Distinct())
                {

                    sw.WriteLine(s);

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    textBox1.Text = fbd.SelectedPath;

                }
                if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations")))
                {
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations"));
                }
                if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet")))
                {
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet"));
                }
                if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "GameFilesBackUp")))
                {
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "GameFilesBackUp"));
                }
                if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "NewFiles")))
                {
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "NewFiles"));
                }
                if (!Directory.Exists(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines")))
                {
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines"));
                }
                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.txt")))
                {
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "config.txt"));
                }
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.txt"), "DEFAULTPATH=" + textBox1.Text);


            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            if (flawed)
            {
                textBox3.AppendText("Sadly, the translation contains one or several mistakes. They need to be fixed in the spreadsheet before patching the game. Afterwards, juste re-download the translation in the app and you're good to go !" + Environment.NewLine + Environment.NewLine);
            }
            else
            {
                Helpers.untranslated.Clear();
                textBox3.AppendText("Fetching translations from " + Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt").ToString());
                var TranslationDict = Helpers.FileToDictionary(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
                var NewTranslationdict = new Dictionary<string, string>();
                foreach (var x in TranslationDict)
                {
                    Console.Write("Key : " + x.Key + "// Value : " + x.Value);
                }

                var backupflag = checkBox1.Checked;
                var BackupFolder = new DirectoryInfo(Path.Combine(textBox1.Text, "Translations", "GameFilesBackUp"));
                var GameFilesFolder = new DirectoryInfo(Path.Combine(textBox1.Text, "game"));
                if (backupflag)
                {


                    foreach (var file in GameFilesFolder.GetFiles())
                    {

                        if (file.Name.EndsWith(".rpy") && !file.Name.EndsWith("rpyc"))
                        {
                            textBox3.AppendText("Now backing up " + file.FullName + " to " + textBox1.Text + @"\Translations\GameFilesBackUp\" + file.Name + Environment.NewLine);
                            if (File.Exists(Path.Combine(BackupFolder.FullName, file.Name)))
                            {
                                File.Delete(Path.Combine(BackupFolder.FullName, file.Name));
                            }
                            file.CopyTo(Path.Combine(BackupFolder.FullName, file.Name));
                        }
                    }
                }
                foreach (var file in GameFilesFolder.GetFiles())
                {
                    if (file.Name.EndsWith(".rpy"))
                    {
                        textBox3.AppendText("Now processing " + file.FullName + Environment.NewLine);
                        var lines = File.ReadAllLines(file.FullName);
                        string pattern = "\"([^\"]*)\"";
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var matches = Regex.Matches(lines[i], pattern);
                            foreach (var match in matches)
                            {
                                if (Helpers.IsChinese(match.ToString()))
                                {
                                    var initial = match.ToString().Replace("\"", "");
                                    if (TranslationDict.ContainsKey(initial))
                                    {
                                        textBox3.AppendText("Now translating " + initial + " to " + TranslationDict[initial] + Environment.NewLine);
                                        lines[i] = lines[i].Replace(match.ToString(), "\"" + TranslationDict[initial] + "\"");
                                        if(!NewTranslationdict.ContainsKey(initial))
                                        { 
                                        NewTranslationdict.Add(initial, TranslationDict[initial]);
                                        }

                                    }
                                    else
                                    {
                                        //textBox3.AppendText("Found Untranslated String : " + initial);
                                        Helpers.untranslated.Add(initial);

                                    }
                                }
                            }
                        }
                        File.WriteAllLines(Path.Combine(textBox1.Text, "Translations", "NewFiles", file.Name), lines);

                    }
                }





                if (File.Exists(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "UN.txt")))
                {
                    File.Delete(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "UN.txt"));
                }
                using (StreamWriter sw = File.AppendText(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "UN.txt")))
                {
                    foreach (var s in Helpers.untranslated.Distinct())
                    {

                        sw.WriteLine(s);

                    }
                }
                var ObseleteLinesDict = new Dictionary<string, string>();

                foreach(var line in TranslationDict.Keys)
                {
                    if(!NewTranslationdict.ContainsKey(line)) 
                    {
                        using (StreamWriter sw = File.AppendText(Path.Combine(textBox1.Text, "Translations", "UntranslatedLines", "ObseleteLinesDict.txt")))
                        {
                                sw.WriteLine(line + "¤" + TranslationDict[line]);
                        }
                    }
                }

                textBox3.AppendText("Patched game files have been generated. They are stored in " + textBox1.Text + @"\Translations\NewFiles\" + Environment.NewLine + "To use them, copy them to your data folder, i.e. " + textBox1.Text + @"\game\" + Environment.NewLine + "Hopefully, it will work fine !");
                textBox3.AppendText(@"Notice that currently untranslated lines have been stored in \Translations\UntranslatedLines\UN.txt" + Environment.NewLine);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private int FindLineNumber(string s)
        {
            if (Helpers.lineorder.ContainsValue(s))
            {
                return Helpers.lineorder.FirstOrDefault(x => x.Value == s).Key;
            }
            else
            {
                return 0;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            flawed = false;
            textBox3.Clear();
            var lines = File.ReadAllLines(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("¤") && !lines[i].EndsWith("¤") && !lines[i].StartsWith("¤"))
                {
                    var chinese = lines[i].Split("¤")[0];
                    var english = lines[i].Split("¤")[1];


                    if (lines[i] != "" && lines[i] != null)
                    {

                        if (english.Contains("\""))
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Found double quotes in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine + lines[i]  + Environment.NewLine + Environment.NewLine);
                        }
                        if ((chinese.Contains("【") || chinese.Contains("】")) && (!english.Contains("【") && !english.Contains("】")))
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Brackets mismatch in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if ((chinese.Contains("（") && !english.Contains("（")) || (chinese.Contains("）") && !english.Contains("）")))
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Parenthesis mismatch in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if (Regex.Matches(chinese, "<[^>]*>").Count != Regex.Matches(english, "<[^>]*>").Count || Regex.Matches(chinese, "\\{[^\\}]*\\}").Count != Regex.Matches(english, "\\{[^\\}]*\\}").Count)
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Symbol ( < > { } ) mismatch in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if (Regex.Matches(chinese, @"\[([^\]]+)\]").Count > 0)
                        {
                            var done = new List<int>();
                            var chmatches = Regex.Matches(chinese, @"\[([^\]]+)\]");
                            var enmatches = Regex.Matches(english, @"\[([^\]]+)\]");
                            var chlist = new List<string>();
                            var enlist = new List<string>();

                            foreach (var ch in chmatches)
                            {
                                chlist.Add(ch.ToString());
                            }
                            foreach (var en in enmatches)
                            {
                                enlist.Add(en.ToString());
                            }
                            for (int j = 0; j < chlist.Count; j++)
                            {
                                //Console.WriteLine("Line : " + lines[i]);
                                //Console.WriteLine("CHLIST : " + chlist.Count + " : ENLIST : " + enlist.Count);
                                try
                                {

                                    if (!enlist.Contains(chlist[j]))
                                    {
                                        if(!done.Contains(FindLineNumber(chinese)))
                                        { 
                                        textBox3.AppendText("ERROR : Text between [ ] mismatch in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                                        flawed = true;
                                        done.Add(FindLineNumber(chinese));
                                        }
                                    }
                                }
                                catch (Exception x)
                                {
                                   
                                    textBox3.AppendText("ERRORcatch : Text between [ ] mismatch in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                                    foreach (var item in chlist)
                                    {
                                        textBox3.AppendText("CHList" + item.ToString() + Environment.NewLine);
                                    }
                                    foreach (var item in enlist)
                                    {
                                        textBox3.AppendText("ENList : " + item.ToString() + Environment.NewLine);
                                    }
                                    flawed = true;
                                }
                            }

                        }
                        if (Regex.Matches(chinese, @"\·").Count != Regex.Matches(english, @"\·").Count)
                        {
                            textBox3.AppendText("· character mismatch between in translation line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine);
                            textBox3.AppendText("Number of · characters in CH line : " + Regex.Matches(chinese, "·").Count + " ; vs · characters in EN line : " + Regex.Matches(english, "·").Count + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                            flawed = true;
                        }

                        var chtagsreg = Regex.Matches(chinese, @"{/color}|{color=[^}]*}").ToList();
                        var entagsreg = Regex.Matches(english, @"{/color}|{color[=| ][^}]*}").ToList();
                        var ListCH = chtagsreg.Where(p => p.Success).Select(p => p.Value).ToList().OrderBy(x => x).ToList();
                        var ListEN = entagsreg.Where(p => p.Success).Select(p => p.Value).ToList().OrderBy(x => x).ToList();


                        /*if (ListCH.Count != 0 && ListEN.Count != 0)
                        {
                            var a = ListCH.SequenceEqual(ListEN);
                            if (a == false)
                            {
                                Console.WriteLine("Color tags mismatch line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine);

                                Console.WriteLine("List CH : " + String.Join("\n", ListCH));
                                Console.WriteLine("List EN : " + String.Join("\n", ListEN));

                                textBox3.AppendText("Color tags mismatch line n°" + FindLineNumber(chinese) + " : " + Environment.NewLine);

                                textBox3.AppendText("Chinese line : " + chinese + Environment.NewLine + "English line : " + english + Environment.NewLine);
                                        textBox3.AppendText("There's potentially an opening color tag i.e. {color=#xxxxxx} which isn't closed with a {/color}. " + Environment.NewLine + 
                                            "You may have {color=#xxxxxx}yyy{color=#xxxxxx}zzz {/color} {/color}, but you can't have {color=#xxxxxx}yyy{/color}{/color} or {color=#xxxxxx}{color=#xxxxxx}yyy{/color}. \n In other words, take care that any opening color tag is matched somewhere with a closed tag and vice versa !" +
                                            Environment.NewLine + "There may also be a nasty space between a {color= # or {color =#. It should be {color=# "+ Environment.NewLine + Environment.NewLine);
                                        flawed = true;



                            }
                            else
                            {
                                Console.WriteLine("Line : " + FindLineNumber(chinese) + ": Similar lists");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Line : " + FindLineNumber(chinese) + ": Empty lists");
                        }*/

                    }
                }
                else if (!lines[i].Contains("¤"))
                {
                    textBox3.AppendText(@"WARNING : One of your lines does has an issue with the separator symbol, ¤, sorry, I can't find the line right now, but it's : " + lines[i] + Environment.NewLine + Environment.NewLine);
                    textBox3.AppendText("SKIPPING this line fow now, it won't be patched into the game but you may continue" + Environment.NewLine + Environment.NewLine);
                    lines[i] = "0¤0";

                }
            }
            textBox3.AppendText("Done !" + Environment.NewLine);
            if (flawed)
            {
                textBox3.AppendText("The translation contains errors... can't patch the game, or it will likely crash ! Please fix them using the spreadsheet and re-check them here" + Environment.NewLine);
            }
            else
            {
                textBox3.AppendText("Everything looks good, ready to patch !" + Environment.NewLine);
            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult fontResult = fontDialog1.ShowDialog();
            if (fontResult == DialogResult.OK)
            {
                textBox3.Font = fontDialog1.Font;

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WebClientEx wc = new WebClientEx(new CookieContainer());
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0");
            wc.Headers.Add("DNT", "1");
            wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            wc.Headers.Add("Accept-Encoding", "deflate");
            wc.Headers.Add("Accept-Language", "en-US,en;q=0.5");

            var outputCSVdata = wc.DownloadString(@"https://docs.google.com/spreadsheets/d/1BoqPuD8CIJmfDyvMHAkvzphGkaSrFr3fZDkkQAViZLg/gviz/tq?tqx=out:csv&sheet=Translations");

            CsvParser.CsvParser csvparser = new CsvParser.CsvParser(delimeter: ',');
            var csvarray = csvparser.Parse(outputCSVdata);
            var i = 1;
            if (File.Exists(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Replaced.txt")))
            {
                File.Delete(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Replaced.txt"));
            }

            using (StreamWriter tw = new StreamWriter(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Replaced.txt"), append: true))
            {


                var h = 0;
                foreach (string[] str in csvarray)
                {
                    h++;
                    if (h > 1)
                    {
                        Console.WriteLine("H : " + h + " // " + str);
                        var z = "";
                        if (str[2] != null && str[2] != "")
                        {
                            foreach (var x in Helpers.replacements)
                            {
                                foreach (var y in x.Value)
                                {
                                    if (str[2].Contains(y))
                                    {
                                        //Console.WriteLine("To Replace : " + y + " // Replacement Value : " + Helpers.final[x.Key]);
                                        //Console.WriteLine("Before : " + str[2]);
                                        z = Regex.Replace(str[2], y, Helpers.final[x.Key],RegexOptions.IgnoreCase);
                                        //Console.WriteLine("After : " + z);
                                    }

                                }
                            }
                            Helpers.towrite.Add(h, new List<string>() { str[2], z });
                        }
                        else
                        {
                            if (str[1] != null && str[1] != "")
                            {
                                foreach (var x in Helpers.replacements)
                                {
                                    foreach (var y in x.Value)
                                    {
                                        if (str[1].Contains(y))
                                        {

                                            Console.WriteLine("To Replace : " + y + " // Replacement Value : " + Helpers.final[x.Key]);
                                            Console.WriteLine("Before : " + str[1]);
                                            z = Regex.Replace(str[1], y, Helpers.final[x.Key], RegexOptions.IgnoreCase);
                                            Console.WriteLine("After : " + z);

                                        }

                                    }
                                }
                                Helpers.towrite.Add(h, new List<string>() { str[2], z });
                            }



                        }
                        if (str[1].Length < 1 && str[2].Length < 1)
                        {
                            z = "";
                            Helpers.towrite.Add(h, new List<string>() { "NULL", "NULL"});

                        }
                    }

                }
                Console.WriteLine("Comparison : CSVARRAY : " + csvarray.Count() + " // DICT : " + Helpers.towrite.Count());
                foreach(var x in Helpers.towrite)
                {
                    tw.WriteLine(x.Key + " // " + String.Join(" ; ", x.Value[0], x.Value[1]));
                }
                
            }
        }
    }
}