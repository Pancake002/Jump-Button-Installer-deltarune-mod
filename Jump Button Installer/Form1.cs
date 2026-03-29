using System.Diagnostics;
using Underanalyzer.Decompiler;
using UndertaleModLib;
using UndertaleModLib.Compiler;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;
namespace Jump_Button_Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string filePath = "";
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void preplace(UndertaleCode code, GlobalDecompileContext context, string dir, string pa, CompileGroup g, UndertaleData data)
        {
            string a = File.ReadAllText(Application.StartupPath + pa);
            if (code.Name.Content == "gml_Object_obj_mainchara_Create_0")
            {
                if (data.Sprites.IndexOfName("spr_noelleShadow") == -1)
                {
                    if (!File.Exists(filePath + "/spr_noelleShadow.png"))
                    {
                        new FileInfo(Application.StartupPath + "/spr_noelleShadow.png").CopyTo(filePath + "/spr_noelleShadow.png");
                    }

                    a += "\nspr_noelleShadow = sprite_add(working_directory + \"../spr_noelleShadow.png\", 1, false, false, 0, 21);";


                }
            }
            a += new DecompileContext(context, code).DecompileToString();







            g.QueueCodeReplace(code, a);
            bool didCompile = g.Compile().Successful;
            if (!didCompile)
            {
                MessageBox.Show("!! " + dir + code.Name.Content);
                MessageBox.Show(a);

            }



        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            FolderBrowserDialog openFileDialog1;
            openFileDialog1 = new FolderBrowserDialog();
            openFileDialog1.Description = "Select the root Deltarune folder";
            openFileDialog1.UseDescriptionForTitle = true;



            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.SelectedPath;
                Debug.WriteLine(filePath);


            }
            else
            {
                Close();
            }

            backgroundWorker1.RunWorkerAsync();

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                double num = 0.0;
                foreach (string dir in Directory.GetDirectories(filePath))
                {
                    backgroundWorker1.ReportProgress((int)((num / Directory.GetDirectories(filePath).Length) * 100));
                    Debug.WriteLine(((num / (double)Directory.GetDirectories(filePath).Length) * 100));
                    num++;
                    Debug.WriteLine(dir);
                    if (File.Exists(dir + "/data.win"))
                    {

                        if (!File.Exists(dir + "/backup.win"))
                        {
                            new FileInfo(dir + "/data.win").CopyTo(dir + "/backup.win");

                        }
                        FileStream streamm = File.OpenRead(dir + "\\backup.win");

                        UndertaleData data = UndertaleIO.Read(streamm, null, null, false);
                        GlobalDecompileContext context = new(data);
                        CompileGroup g = new(data, context);



                        UndertaleSprite a = new();




                        a.Name = data.Strings.MakeString("spriteemptyjumpmod");
                        data.Sprites.Add(a);
                        string[] codes = { "gml_Object_obj_mainchara_Step_0", "gml_Object_obj_mainchara_Create_0" };
                        foreach (string s in codes)
                        {
                            UndertaleCode code = data.Code.ByName(s);
                            if (code.Name.Content == "gml_Object_obj_mainchara_Step_0")
                            {


                                preplace(code, context, dir, "/MainCharaStepPre.gml", g, data);

                            }
                            else if (code.Name.Content == "gml_Object_obj_mainchara_Create_0")
                            {
                                preplace(code, context, dir, "/MainCharaCreatePost.gml", g, data);

                            }
                        }
                        streamm = File.Create(dir + "\\data.win");
                        UndertaleIO.Write(streamm, data);

                    }


                }


                foreach (string f in Directory.GetFiles(filePath))
                {
                    if (f.Contains(".exe"))
                    {
                        var p = new Process();
                        p.StartInfo.FileName = f;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.WorkingDirectory = filePath;
                        p.Start();

                        Application.Exit();
                    }
                }
            }
            finally
            {
                MessageBox.Show("Something went wrong.\nMake sure you selected the folder with DELTARUNE.exe in it.", "!!",MessageBoxButtons.OK);
                Close();
            }
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Close(); ;
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}
