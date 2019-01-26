using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace diplom
{
    public partial class Form1 : Form
    {
        string[] allLines;
        string[] line;
        
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string[] data)
        {
            InitializeComponent();
            allLines = data;
        }

        private void pathB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ok = new OpenFileDialog();

            if (ok.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathT.Text = ok.FileName;
            }

            ok.Dispose();
        }

        private void pathT_TextChanged(object sender, EventArgs e)
        {

        }

        public float retNum(string str)
        {
            float num = 0;
            int i = 0;

            foreach (var a in str)
                if (Char.IsNumber(a) || a == '-')
                {
                    i = str.IndexOf(a);
                    break;
                }

            string numStr = "";

            do
            {
                numStr += str[i];
                i++;
            } while (Char.IsNumber(str[i]) || str[i] == '.');



            return num = float.Parse(numStr, CultureInfo.InvariantCulture.NumberFormat);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void fileCheck()
        {
            StreamReader myReader = new StreamReader(pathT.Text);
            string text = myReader.ReadToEnd();
            string transLine = "";
            bool func = false;
            string line1 = line[1], arg1 = "", arg2 = "", arg3 = "", ure = "";
            int i = 0;
            while(line1.IndexOf('(')!=-1)
            {
                i = 0;
                while (line1[i] != null)
                {
                    arg1 = ""; arg2 = ""; arg3 = ""; ure = "";
                    if (line1[i] == ')')
                    {
                        while (line1[i] != '(') i--;
                        while (line1[i] != ' ')
                        {
                            i++;
                            if (line1[i] != ' ') arg1 += line1[i];
                        }
                        do
                        {
                            i++;
                            if (line1[i] != ' ') arg2 += line1[i];
                        }
                        while (line1[i] != ' ');
                        do
                        {
                            i++;
                            if (line1[i] != ')') arg3 += line1[i];
                        }
                        while (line1[i] != ')');
                        if (arg1 == "^")
                            ure += arg2 + "^" + arg3;
                        else if (arg1 == "*")
                            ure += arg2 + "/*/" + arg3;
                        else if (arg1 == "+")
                            ure += arg2 + "/+/" + arg3;
                        else if (arg1 == "-")
                            ure += arg2 + "/-/" + arg3;
                        line1 = line1.Replace("(" + arg1 + " " + arg2 + " " + arg3 + ")", ure);
                        break;
                    }
                    i++;
                }
            }
            ure = ""; arg1 = "";arg2 = "";arg3 = "";
            i = line1.IndexOf('{');
            
            while (line1[i] != ' ')
            {
                i++;
                if(line1[i]!=' ')arg1 += line1[i];
            }
            i++;
            while (line1[i] != ' ')
            {
                if (line1[i] != ' ') arg2 += line1[i];
                i++;
            }
            while (line1[i] != '}')
            {
                i++;
                if (line1[i] != '}') arg3 += line1[i];
            }
            ure = arg1 + "(" + arg2 + "," + arg3 + ") := ";
            line1 = line1.Replace("{" + arg1 + " " + arg2 + " " + arg3 + "}", ure);
            line1 = line1.Replace('/', ' ');
            MessageBox.Show(line1);
            myReader.Close();
        }

        string lineTransform(string line1)
        {
            string arg1 = "", arg2 = "", arg3 = "", ure = "", saveArg1, saveArg2, saveArg3;
            int i = 0;
            while (line1.IndexOf('(') != -1)
            {
                int parents = 0;
                i = 0;
                while (line1[i] != null)
                {
                    arg1 = ""; arg2 = ""; arg3 = ""; ure = "<ml:apply>\n";
                    if (line1[i] == ')')
                    {
                        while (line1[i] != '(') i--;
                        while (line1[i] != ' ')
                        {
                            i++;
                            if (line1[i] != ' ') arg1 += line1[i];
                        }
                        do
                        {
                            i++;
                            if (line1[i] != ' ') arg2 += line1[i];
                        }
                        while (line1[i] != ' ');
                        do
                        {
                            i++;
                            if (line1[i] != ')') arg3 += line1[i];
                        }
                        while (line1[i] != ')');
                        saveArg1 = arg1; saveArg2 = arg2; saveArg3 = arg3;
                        //if ((arg2.IndexOf("plus") != -1 || arg2.IndexOf("minus") != -1) &&(arg1.IndexOf('^')!=-1 || arg1.IndexOf('*') != -1 || arg1.IndexOf("::") != -1))
                        //    parents++;
                        //if ((arg3.IndexOf("plus") != -1 || arg3.IndexOf("minus") != -1) && (arg1.IndexOf('^') != -1 || arg1.IndexOf('*') != -1 || arg1.IndexOf("::") != -1))
                        //    parents += 2;
                        //if (parents == 1)
                        //{
                        //    arg2 = "<ml:parens>\n" + arg2 + "</ml:parens>\n";
                        //}else
                        //if (parents == 2)
                        //{
                        //    arg3 = "<ml:parens>\n" + arg3 + "</ml:parens>\n";
                        //}else
                        //if (parents == 3)
                        //{
                        //    arg2 = "<ml:parens>\n" + arg2 + "</ml:parens>\n";
                        //    arg3 = "<ml:parens>\n" + arg3 + "</ml:parens>\n";
                        //}
                        if (arg1 == "^")
                            ure += "<ml:pow/>\n";
                        else if (arg1 == "*")
                            ure += "<ml:mult/>\n";
                        else if (arg1 == "+")
                            ure += "<ml:plus/>\n";
                        else if (arg1 == "-")
                            ure += "<ml:minus/>\n";
                        else if (arg1 == "::")
                            ure += "<ml:div/>\n";
                        if (arg1 == "^" || arg1 == "-" || arg1 == "*" || arg1 == "+" || arg1 == "::")
                        {
                            if (arg2.IndexOf('<') == -1) ure += checkNumber(arg2);
                            else ure += arg2;
                            if (arg3.IndexOf('<') == -1) ure += checkNumber(arg3);
                            else ure += arg3;
                        }
                        line1 = line1.Replace("(" + saveArg1 + " " + saveArg2 + " " + saveArg3 + ")", ure + "</ml:apply>\n");
                        break;
                    }
                    i++;
                }
            }
            ure = ""; arg1 = ""; arg2 = ""; arg3 = "";
            if (line1.IndexOf('{') != -1)
            {
                i = line1.IndexOf('{');
                while (line1[i] != ' ')
                {
                    i++;
                    if (line1[i] != ' ') arg1 += line1[i];
                }
                i++;
                while (line1[i] != ' ')
                {
                    if (line1[i] != ' ') arg2 += line1[i];
                    i++;
                }
                while (line1[i] != '}')
                {
                    i++;
                    if (line1[i] != '}') arg3 += line1[i];
                }
                ure = "<ml:function>\n<ml:id xml:space=\"preserve\">" + arg1 + "</ml:id>\n<ml:boundVars>\n<ml:id xml:space=\"preserve\">" + arg2 + "</ml:id>\n<ml:id xml:space=\"preserve\">" + arg3 + "</ml:id>\n</ml:boundVars>\n</ml:function>\n";
                line1 = line1.Replace("{" + arg1 + " " + arg2 + " " + arg3 + "}", ure);
            }
            line1 = line1.Replace('_', ' ');
            line1 = line1.Replace("oo", "<ml:parens>\n");
            line1 = line1.Replace("cc", "</ml:parens>\n");
            return line1;
        }

        string checkNumber(string data)
        {
            if (Int32.TryParse(data, out int n)) return "<ml:real>"+data+ "</ml:real>\n";
            else return "<ml:id_xml:space=\"preserve\">"+data+ "</ml:id>\n";
        }

        private void complete_Click(object sender, EventArgs e)
        {
            bool variantF = false;
            int error = 0;
            StreamReader myReader = new StreamReader(pathT.Text);
            string text = myReader.ReadToEnd();
            text = text.Replace(" style=\"auto-select\"", "");
            text = text.Replace("\t", "");
            text = text.Replace("\r", "");
            myReader.Close();
            if (variantT.Text == "" || pathT.Text == "")
                MessageBox.Show("Заполните поля!");
            else
            {
                FileInfo FI = new FileInfo(pathT.Text);
                if (!FI.Exists) MessageBox.Show("Проверьте введеный путь!");
                else
                    foreach (var a in allLines)
                    {
                        line = a.Split('|');
                        if (line[0] == variantT.Text) { variantF = true; break; }
                    }
                if (pathT.Text.Substring(pathT.Text.LastIndexOf(".") + 1) != "xmcd") MessageBox.Show("Формат файла должен быть xmcd!");
                else
                if (variantF)
                {
                    if (line[1] == "1")//множители Лагранжа
                    {
                        Label[] labels = {labelG,labelF,labelDXF };
                        int i = 0;
                        while (i < labels.Length)
                        {
                            string currentLine = lineTransform(line[i+2]);
                            if (text.IndexOf(currentLine) != -1) labels[i].Text = "+";//MessageBox.Show("ура");
                            else { error = i+1; break; }
                            i++;
                        } 
                    }
                }
                else MessageBox.Show("Вариант не найден!");
            }
        }
    }
}


