using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
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

                    }
                }
            }
            if(!Directory.Exists(Path.Combine(textBox1.Text, "Translations")))
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
            DownloadSheet.Main(@"https://docs.google.com/spreadsheets/d/1BoqPuD8CIJmfDyvMHAkvzphGkaSrFr3fZDkkQAViZLg/gviz/tq?tqx=out:csv&sheet=Translations", Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
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
            if(flawed)
            {
                textBox3.AppendText("Sadly, the translation contains one or several mistakes. They need to be fixed in the spreadsheet before patching the game. Afterwards, juste re-download the translation in the app and you're good to go !" + Environment.NewLine + Environment.NewLine);
            }
            else
            { 
            Helpers.untranslated.Clear();

            var TranslationDict = Helpers.FileToDictionary(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
            textBox3.Clear();
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

        private void button5_Click(object sender, EventArgs e)
        {
            flawed = false;
            textBox3.Clear();
            var lines = File.ReadAllLines(Path.Combine(textBox1.Text, "Translations", "DownloadedFromSpreadsheet", "Translations.txt"));
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "" && lines[i] != null)
                {
                    if (!lines[i].Contains("¤") || lines[i].StartsWith("¤") || lines[i].EndsWith("¤"))
                    {
                        textBox3.AppendText("WARNING : One of your lines does has an issue with the separator symbol, ¤, please check line n°" + i+1 + "!" + Environment.NewLine + lines[i] + Environment.NewLine);
                        textBox3.AppendText("SKIPPING this line fow now, it won't be patched into the game but you may continue" + Environment.NewLine+ Environment.NewLine);
                        lines[i] = "0¤0";
                     }
                    else
                    {
                        var chinese = lines[i].Split("¤")[0];
                        var english = lines[i].Split("¤")[1];
                        if (english.Contains("\""))
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Found double quotes in translation line n°" + i + 1 + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if ((chinese.Contains("【") || chinese.Contains("】")) && (!english.Contains("【") || !english.Contains("】")))
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Brackets mismatch in translation line n°" + i + 1 + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if ((chinese.Contains("（") || chinese.Contains("）")) && (!english.Contains("（") || !english.Contains("）")))
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Parenthesis mismatch in translation line n°" + i + 1 + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if (Regex.Matches(chinese, "<[^>]*>").Count != Regex.Matches(english, "<[^>]*>").Count || Regex.Matches(chinese, "\\{[^\\}]*\\}").Count != Regex.Matches(english, "\\{[^\\}]*\\}").Count)
                        {
                            flawed = true;
                            textBox3.AppendText("ERROR : Symbol ( < > { } ) mismatch in translation line n°" + i + 1 + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                        }
                        if (Regex.Matches(chinese, @"\[([^\]]+)\]").Count > 0)
                        {

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
                                if (chlist[j] != enlist[j])
                                {

                                    textBox3.AppendText("ERROR : Text between [ ] mismatch in translation line n°" + i + 1 + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                                    flawed = true;
                                }
                            }

                        }
                        if (Regex.Matches(chinese, "·").Count != Regex.Matches(english, "·").Count)
                        {
                            textBox3.AppendText("· character mismatch between in translation line n°" + i + 1 + " : " + Environment.NewLine + lines[i] + Environment.NewLine + Environment.NewLine);
                            flawed = true;
                        }
                    }
                    
            }
            }

            textBox3.AppendText("Done !" + Environment.NewLine);
            if(flawed)
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

    }
}