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
            Label[] labels = { labelG, labelF, labelDXF, labelDYF, labelDXG, labelDYG, labelXX, labelYY };
            onOffLabels(labels);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        string findHeadSequence(string line1, string text)
        {
            int i = 0, count = 0;
            string arg1 = "", arg2 = "", arg3 = "", arg4 = "", ure = "", sequence = "", apply = "";
            string[] halfLine1 = line1.Split(','); ;
            if (halfLine1[0].IndexOf('{') != -1)
            {
                i = halfLine1[0].IndexOf('{');
                while (halfLine1[0][i] != ' ')
                {
                    i++;
                    if (halfLine1[0][i] != ' ') arg1 += halfLine1[0][i];
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg2 += halfLine1[0][i];
                    i++;
                }
                while (halfLine1[0][i] != '}')
                {
                    i++;
                    if (halfLine1[0][i] != '}') arg3 += halfLine1[0][i];
                }
                ure = "<ml:function>\n<ml:id xml:space=\"preserve\">" + arg1 + "</ml:id>\n<ml:boundVars>\n<ml:id xml:space=\"preserve\">" + arg2 + "</ml:id>\n<ml:id xml:space=\"preserve\">" + arg3 + "</ml:id>\n</ml:boundVars>\n</ml:function>\n";
                halfLine1[0] = halfLine1[0].Replace("{" + arg1 + " " + arg2 + " " + arg3 + "}", ure);
            }
            else
                if (halfLine1[0].IndexOf('\'') != -1)
            {
                i = halfLine1[0].IndexOf('\'');
                while (halfLine1[0][i] != ' ')
                {
                    i++;
                    if (halfLine1[0][i] != ' ') arg1 += halfLine1[0][i];
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg2 += halfLine1[0][i];
                    i++;
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg3 += halfLine1[0][i];
                    i++;
                }
                while (halfLine1[0][i] != '\'')
                {
                    i++;
                    if (halfLine1[0][i] != '\'') arg4 += halfLine1[0][i];
                }
                ure = "<ml:apply>\n<ml:derivative/>\n<ml:lambda>\n<ml:boundVars>\n<ml:id xml:space=\"preserve\">" + arg1 + "</ml:id>\n</ml:boundVars>\n<ml:apply>\n<ml:id xml:space=\"preserve\">" + arg2 + "</ml:id>\n<ml:sequence>\n<ml:id xml:space=\"preserve\">" + arg3 + "</ml:id>\n<ml:id xml:space=\"preserve\">" + arg4 + "</ml:id>\n</ml:sequence>\n</ml:apply>\n</ml:lambda>\n</ml:apply>\n<ml:symResult>\n";
                halfLine1[0] = halfLine1[0].Replace("\'" + arg1 + " " + arg2 + " " + arg3 + " " + arg4 + "\'", ure);
            }
            i = text.IndexOf(halfLine1[0]) + halfLine1[0].Length;
            do
            {
                while (text[i] != '\n')
                {
                    apply += text[i];
                    i++;
                }
                if (apply == "<ml:apply>") count++;
                else if (apply == "</ml:apply>") count--;
                sequence += apply + '\n';
                apply = "";
                i++;
            } while (count != 0);
            return sequence;
        }

        int findHead(string line, string text)
        {
            int i = text.IndexOf(line);
            return i + line.Length;
        }

        string transFromMcdToPoland(string sequence)//форматирует из маткадовской в польскую
        {
            sequence = sequence.Replace("<ml:parens>\n", "[");
            sequence = sequence.Replace("\n</ml:parens>", "]");
            sequence = sequence.Replace("<ml:id xml:space=\"preserve\">", "");
            sequence = sequence.Replace("</ml:real>", "");
            sequence = sequence.Replace("<ml:real>", "");
            sequence = sequence.Replace("</ml:id>", "");
            sequence = sequence.Replace("<ml:symResult>", "");
            sequence = sequence.Replace("</ml:symResult>", "");

            sequence = sequence.Replace("\n</ml:apply>", ")");
            sequence = sequence.Replace("<ml:apply>\n", "(");
            sequence = sequence.Replace("<ml:mult/>", "*");
            sequence = sequence.Replace("<ml:plus/>", "+");
            sequence = sequence.Replace("<ml:minus/>", "-");
            sequence = sequence.Replace("<ml:div/>", "::");
            sequence = sequence.Replace("<ml:pow/>", "^");
            sequence = sequence.Replace("<ml:sqrt/>", "sqrt");

            sequence = sequence.Replace('\n', ' ');

            return sequence;
        }

        string transFromPolandToMcd(string line1)// форматирует из польской в мадкадовскую (что-то одно, шапку или хвост)
        {
            int i = 0;
            string arg1 = "", arg2 = "", arg3 = "", ure = "", arg4= "";
            if (line1.IndexOf('{') != -1)
            {
                i = line1.IndexOf('{') + 1;
                for (; line1[i] != ' '; i++)
                    arg1 += line1[i];
                i++;
                for (; line1[i] != ' '; i++)
                    arg2 += line1[i];
                i++;
                for (; line1[i] != '}' && line1[i] != ' '; i++)
                    arg3 += line1[i];
                if (line1[i] == ' ')
                {
                    i++;
                    for (; line1[i] != '}'; i++)
                        arg4 += line1[i];
                }
                if (arg4 == "")
                {
                    ure = "<ml:function>\n" + checkNumber(arg1) + "<ml:boundVars>\n" + checkNumber(arg2) + checkNumber(arg3) + "</ml:boundVars>\n</ml:function>\n";
                    line1 = line1.Replace("{" + arg1 + " " + arg2 + " " + arg3 + "}", ure);
                }
                else
                {
                    ure = "<ml:function>\n" + checkNumber(arg1) + "<ml:boundVars>\n" + checkNumber(arg2) + checkNumber(arg3) + checkNumber(arg4) + "</ml:boundVars>\n</ml:function>\n";
                    line1 = line1.Replace("{" + arg1 + " " + arg2 + " " + arg3 + ' ' + arg4 + "}", ure);
                }
                
                return line1.Replace("_", " ");
            }
            arg1 = ""; arg2 = ""; arg3 = ""; arg4 = ""; ure = "";
            if (line1.IndexOf('\'') != -1)
            {
                i = line1.IndexOf('\'')+1;
                for (; line1[i] != ' '; i++)
                    arg1 += line1[i];
                i++;
                for (; line1[i] != ' '; i++)
                    arg2 += line1[i];
                i++;
                for (; line1[i] != ' '; i++)
                    arg3 += line1[i];
                for (; line1[i] != '\''; i++)
                    arg4 += line1[i];
                ure = "<ml:apply>\n<ml:derivative/>\n<ml:lambda>\n<ml:boundVars>\n" + checkNumber(arg1) + "</ml:boundVars>\n<ml:apply>\n" + checkNumber(arg2)+ "<ml:sequence>\n" + checkNumber(arg3)+ checkNumber(arg3) + "</ml:sequence>\n</ml:apply>\n</ml:lambda>\n</ml:apply>\n";
                line1 = line1.Replace("\'" + arg1 + " " + arg2 + " " + arg3 + arg4 + "\'", ure);
                return line1.Replace("_", " ");
            }
            arg1 = ""; arg2 = ""; arg3 = ""; ure = "";
            while (line1.IndexOf(')')!=-1)
            {
                i = line1.IndexOf(')');
                while (line1[i] != '(') i--;
                i++;
                for (; line1[i] != ' '; i++)
                    arg1 += line1[i];
                i++;
                for (; line1[i] != ' ' && line1[i] != ')'; i++)
                    arg2 += line1[i];
                i++;
                if(arg1 != "sqrt")
                for (; line1[i] != ')'; i++)
                    arg3 += line1[i];
                if (arg1 == "+")
                    ure += "<ml:plus/>\n";
                else if (arg1 == "-")
                    ure += "<ml:minus/>\n";
                else if (arg1 == "*")
                    ure += "<ml:mult/>\n";
                else if (arg1 == "::")
                    ure += "<ml:div/>\n";
                else if (arg1 == "^")
                    ure += "<ml:pow/>\n";
                else if(arg1 == "sqrt")
                    ure += "<ml:sqrt/>\n";
                if (arg1 == "^" || arg1 == "-" || arg1 == "*" || arg1 == "+" || arg1 == "::" || arg1 == "sqrt")
                {
                    if (arg2.IndexOf('<') == -1) ure += checkNumber(arg2);
                    else ure += arg2;
                    if (arg3.IndexOf('<') == -1 && arg1!="sqrt") ure += checkNumber(arg3);
                    else ure += arg3;
                }
                if (arg1 != "sqrt") line1 = line1.Replace("(" + arg1 + " " + arg2 + " " + arg3 + ")", "<ml:apply>\n" + ure + "</ml:apply>\n").Replace("[", "<ml:parens>\n").Replace("]", "</ml:parens>\n");
                else line1 = line1.Replace("(" + arg1 + " " + arg2 + ")", "<ml:apply>\n" + ure + "</ml:apply>\n").Replace("[", "<ml:parens>\n").Replace("]", "</ml:parens>\n");
                arg1 = ""; arg2 = ""; arg3 = "";ure = "";
            }
            return line1.Replace("_", " ");
        }

        double calculater(string sequence, double factor = 7)//считает выражение в польской нотации
        {
            if(sequence.IndexOf(',')!=-1)
                sequence = sequence.Split(',')[1];
            double result = 0;
            Double.TryParse(sequence, out result);
            int i = 0;
            string arg1 = "", arg2 = "", arg3 = "", saveArg2 = "", saveArg3 = "";
            while (sequence.IndexOf('(')!=-1)
            {
                i = sequence.IndexOf(')');
                if (sequence[i] == ')')
                {
                    while (sequence[i] != '(') i--;
                    while (sequence[i] != ' ')
                    {
                        i++;
                        if (sequence[i] != ' ') arg1 += sequence[i];
                    }
                    do
                    {
                        i++;
                        if (sequence[i] != ' ' && sequence[i] != ')') arg2 += sequence[i];
                    }
                    while (sequence[i] != ' ' && sequence[i] != ')');
                    saveArg2 = arg2;
                    if (!Int32.TryParse(arg2, out int n)) arg2 = factor.ToString();
                    if (arg1 != "sqrt")
                    {
                        do
                        {
                            i++;
                            if (sequence[i] != ')') arg3 += sequence[i];
                        }
                        while (sequence[i] != ')');
                        saveArg3 = arg3;
                        if (!Int32.TryParse(arg3, out int m)) arg3 = factor.ToString();
                    }
                    if (arg1 == "^")
                        result = Math.Pow(Convert.ToDouble(arg2), Convert.ToDouble(arg3));
                    else if (arg1 == "*")
                        result = Convert.ToDouble(arg2) * Convert.ToDouble(arg3);
                    else if (arg1 == "+")
                        result = Convert.ToDouble(arg2) + Convert.ToDouble(arg3);
                    else if (arg1 == "-")
                        result = Convert.ToDouble(arg2) - Convert.ToDouble(arg3);
                    else if (arg1 == "::")
                        result = Convert.ToDouble(arg2) / Convert.ToDouble(arg3);
                    else if (arg1 == "sqrt")
                        result = Math.Sqrt(Convert.ToDouble(arg2));
                    if(arg1!= "sqrt")
                        sequence = sequence.Replace('(' + arg1 + ' ' + saveArg2 + ' ' + saveArg3 + ')', result.ToString());
                    else sequence = sequence.Replace('(' + arg1 + ' ' + saveArg2 + ')', result.ToString());
                }
                arg1 = ""; arg2 = ""; arg3 = "";
            }

            return result;
        }

        string checkNumber(string data)
        {
            if (Int32.TryParse(data, out int n)) return "<ml:real>"+data+ "</ml:real>\n";
            else return "<ml:id_xml:space=\"preserve\">"+data+ "</ml:id>\n";
        }

        string groupForHead(string fLine, string sLine)// делает из шапок 'x f x y' и 'x g x y' шапку 'x f x y'+ lya * 'x g x y'
        {
            string arg1 = "", arg2 = "", arg3 = "", arg4 = "", ure = "", result = "";
            string[] halfLine1 = fLine.Split(',');
            int i = 0;
            if (halfLine1[0].IndexOf('\'') != -1)
            {
                i = halfLine1[0].IndexOf('\'');
                while (halfLine1[0][i] != ' ')
                {
                    i++;
                    if (halfLine1[0][i] != ' ') arg1 += halfLine1[0][i];
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg2 += halfLine1[0][i];
                    i++;
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg3 += halfLine1[0][i];
                    i++;
                }
                while (halfLine1[0][i] != '\'')
                {
                    i++;
                    if (halfLine1[0][i] != '\'') arg4 += halfLine1[0][i];
                }
                ure = "<ml:apply>\n<ml:plus/>\n<ml:apply>\n<ml:derivative/>\n<ml:lambda>\n<ml:boundVars>\n<ml:id xml:space=\"preserve\">" + arg1 + "</ml:id>\n</ml:boundVars>\n<ml:apply>\n<ml:id xml:space=\"preserve\">" + arg2 + "</ml:id>\n<ml:sequence>\n<ml:id xml:space=\"preserve\">" + arg3 + "</ml:id>\n<ml:id xml:space=\"preserve\">" + arg4 + "</ml:id>\n</ml:sequence>\n</ml:apply>\n</ml:lambda>\n</ml:apply>\n";
                halfLine1[0] = halfLine1[0].Replace("\'" + arg1 + " " + arg2 + " " + arg3 + " " + arg4 + "\'", ure);
            }
            result = halfLine1[0];
            halfLine1 = sLine.Split(',');
            arg1 = "";arg2 = "";arg3 = "";arg4 = "";
            if (halfLine1[0].IndexOf('\'') != -1)
            {
                i = halfLine1[0].IndexOf('\'');
                while (halfLine1[0][i] != ' ')
                {
                    i++;
                    if (halfLine1[0][i] != ' ') arg1 += halfLine1[0][i];
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg2 += halfLine1[0][i];
                    i++;
                }
                i++;
                while (halfLine1[0][i] != ' ')
                {
                    if (halfLine1[0][i] != ' ') arg3 += halfLine1[0][i];
                    i++;
                }
                while (halfLine1[0][i] != '\'')
                {
                    i++;
                    if (halfLine1[0][i] != '\'') arg4 += halfLine1[0][i];
                }
                ure = "<ml:apply>\n<ml:mult/>\n<ml:id xml:space=\"preserve\">λ</ml:id>\n<ml:apply>\n<ml:derivative/>\n<ml:lambda>\n<ml:boundVars>\n<ml:id xml:space=\"preserve\">" + arg1 + "</ml:id>\n</ml:boundVars>\n<ml:apply>\n<ml:id xml:space=\"preserve\">" + arg2 + "</ml:id>\n<ml:sequence>\n<ml:id xml:space=\"preserve\">" + arg3 + "</ml:id>\n<ml:id xml:space=\"preserve\">" + arg4 + "</ml:id>\n</ml:sequence>\n</ml:apply>\n</ml:lambda>\n</ml:apply>\n</ml:apply>\n</ml:apply>\n<ml:symResult>\n";
                halfLine1[0] = halfLine1[0].Replace("\'" + arg1 + " " + arg2 + " " + arg3 + " " + arg4 + "\'", ure);
            }
            result += halfLine1[0];
            return result;
        }

        string groupForSequences(string fLine, string sLine)//делает из выражений под шапками 'x f x y' и 'x g x y' выражение 'x f x y'+ lya * 'x g x y'
        {
            string[] halfLine1 = fLine.Split(','), halfLine2 = sLine.Split(',');
            return "(+ " + halfLine1[1] + " (* λ "+ halfLine2[1]+"))";
        }

        string knife(int i,string text)//вырезает из текста текст, заключенный в apply, начиная с i индекса
        {
            int count = 0;
            string apply = "", sequence = "";
            do
            {
                while (text[i] != '\n')
                {
                    apply += text[i];
                    i++;
                }
                if (apply == "<ml:apply>") count++;
                else if (apply == "</ml:apply>") count--;
                sequence += apply + '\n';
                apply = "";
                i++;
            } while (count != 0);
            return sequence;
        }

        string halfLine(string line, int factor = 0)//возвращает шапку или хвост
        {
            string[] lines = line.Split(',');
            return lines[factor];
        }

        string matrixCraft(string line1 = "", string line2 = "", string line3 = "")
        {
                return "<ml:matrix rows=\"3\" cols=\"1\">\n"+line1+line2+line3 + "</ml:matrix>\n";
        }

        bool checkFyn(string matrix, string text)
        {
            string line = "<ml:function>\n<ml:id xml:space=\"preserve\">Fyn</ml:id>\n<ml:boundVars>\n<ml:id xml:space=\"preserve\">x</ml:id>\n<ml:id xml:space=\"preserve\">y</ml:id>\n<ml:id xml:space=\"preserve\">λ</ml:id>\n</ml:boundVars>\n</ml:function>\n";
            line += matrix;
            if (text.IndexOf(line) != -1) return true;
            else return false;
        }
        
        bool checkFynXYL(string text)
        {
            string fynxyl = "<ml:apply>\n<ml:id xml:space=\"preserve\">Fyn</ml:id>\n<ml:sequence>\n<ml:id xml:space=\"preserve\">x</ml:id>\n<ml:id xml:space=\"preserve\">y</ml:id>\n<ml:id xml:space=\"preserve\">λ</ml:id>\n</ml:sequence>\n</ml:apply>";
            if (text.IndexOf(fynxyl) != -1) return true;
            else return false;
        }

        bool checkFindXYL(string text)
        {
            int i = text.IndexOf("<ml:Find");
            string line = "";
            for (; text[i] != '\n'; i++)
                line += text[i];
            line += "<ml:sequence>\n<ml:id xml:space=\"preserve\">x</ml:id>\n<ml:id xml:space=\"preserve\">y</ml:id>\n<ml:id xml:space=\"preserve\">λ</ml:id>\n</ml:sequence>";
            if (text.IndexOf(line) != -1) return true;
            else return false;
        }

        bool checkAnswer(string text, string x, string y, string l)
        {
            string answer = "<ml:matrix rows=\"3\" cols=\"1\">\n<ml:real>",condition ="";
                            
            int i = text.IndexOf(answer)+answer.Length;
            answer = "";
            for (; condition != "</ml:matrix>"; i++)
            {
                answer += text[i];
                if (text[i] != '\n') condition += text[i];
                else condition = "";
            }
            answer = answer.Replace("<ml:real>", "").Replace("</ml:real>", "").Replace("</ml:matrix>", "").Replace('.',',');
            string[] xyl = answer.Split('\n');
            double result = Math.Round(Convert.ToDouble(xyl[0]),3) + Math.Round(Convert.ToDouble(xyl[1]), 3) + Math.Round(Convert.ToDouble(xyl[2]), 3);
            double xD = Convert.ToDouble(x.Replace('.',','));
            double yD = Convert.ToDouble(y.Replace('.', ','));
            double lD = Convert.ToDouble(l.Replace('.', ','));

            if (xD + yD + lD == result)
                return true;
            else return false;
        }

        void error(int index)
        {
            switch (index)
            {
                case 1:
                    {
                        MessageBox.Show("Проверьте правильность варианта и задания!", "Ошибка!");
                        break;
                    }
                case 2:
                    {
                        MessageBox.Show("Проверьте правильность варианта и задания!", "Ошибка!");
                        break;
                    }
                case 3:
                    {
                        MessageBox.Show("Частная производная d/dx от f(x,y) не найдена или найдена неверно!", "Ошибка!");
                        break;
                    }
                case 4:
                    {
                        MessageBox.Show("Частная производная d/dy от f(x,y) не найдена или найдена неверно!", "Ошибка!");
                        break;
                    }
                case 5:
                    {
                        MessageBox.Show("Частная производная d/dx от g(x,y) не найдена или найдена неверно!", "Ошибка!");
                        break;
                    }
                case 6:
                    {
                        MessageBox.Show("Частная производная d/dy от g(x,y) не найдена или найдена неверно!", "Ошибка!");
                        break;
                    }
                case 7:
                    {
                        MessageBox.Show("Функция Лагранжа не составлена или состаавлена неверно!", "Ошибка!");
                        break;
                    }
                case 8:
                    {
                        MessageBox.Show("Функция Лангранжа не составлена или составлена неверно!", "Ошибка!");
                        break;
                    }
                case 9:
                    {
                        MessageBox.Show("Система не составлена или составлена неверно!", "Ошибка!");
                        break;
                    }
                case 10:
                    {
                        MessageBox.Show("Проверьте, не забыли ли вы присвоить переменным значения!", "Ошибка!");
                        break;
                    }
                case 11:
                    {
                        MessageBox.Show("Не забудьте про ключевое слово Given!", "Ошибка!");
                        break;
                    }
                case 12:
                    {
                        MessageBox.Show("", "Ошибка!");
                        break;
                    }
                case 13:
                    {
                        MessageBox.Show("", "Ошибка!");
                        break;
                    }
                case 14:
                    {
                        MessageBox.Show("Ответ не совпал!", "Ошибка!");
                        break;
                    }
                default: break;
            }
        }

        void onLabel(Label lbl, string text = "")//меняет видимость и текст лейбла
        {
            lbl.Visible = true;
            lbl.Text = text;
        }

        void onOffLabels(Label[] labels)
        {
            foreach (var a in labels)
                a.Visible = !a.Visible;
        }

        bool equalNumber(string x, string text)
        {
            int i = 0, j = 0;
            string line = "", inverseLine = "";
            while(text.IndexOf("<ml:id xml:space=\"preserve\">" + x + "</ml:id>\n")!=-1)
            {
                i = text.IndexOf("<ml:id xml:space=\"preserve\">" + x + "</ml:id>\n") - 2;
                while(text[i]!='\n')
                {
                    inverseLine += text[i];
                    i--;
                }
                j = inverseLine.Length-1;
                while (-1!=j)
                {
                    line += inverseLine[j];
                    j--;
                }
                if (line.IndexOf("<ml:define") != -1)
                {
                    i = text.IndexOf("<ml:id xml:space=\"preserve\">" + x + "</ml:id>\n");
                    break;
                }
                else
                {
                    i = text.IndexOf("<ml:id xml:space=\"preserve\">" + x + "</ml:id>\n");
                    text = text.Remove(i, ("<ml:id xml:space=\"preserve\">" + x + "</ml:id>\n").Length);
                }
                inverseLine = ""; line = "";
            }
            j = ("<ml:id xml:space=\"preserve\">" + x + "</ml:id>\n").Length + i;
            line = "";
            for (; text[j] != '\n'; j++)
                line += text[j];
            line = line.Replace("<ml:real>", "").Replace("</ml:real>", "");
            if (Int32.TryParse(line, out int n)) return true;
            else return false;
        }

        private void complete_Click(object sender, EventArgs e)
        {
            bool variantF = false, fileOK = true;
            string poland = "";
            StreamReader myReader = null;
            Label[] labels = { labelG, labelF, labelDXF, labelDYF, labelDXG, labelDYG, labelXX, labelYY };
            int err = 0;
            do
            {
                try
                {
                    myReader = new StreamReader(pathT.Text);
                    fileOK = !fileOK;
                }
                catch
                {
                    MessageBox.Show("Возможно, Файл открыт в другом приложении, закройте и нажмите ОК");
                }
            } while (fileOK);
            string text = myReader.ReadToEnd(), currentLine;
            text = text.Replace(" style=\"auto-select\"", "").Replace("\t", "").Replace("\r", "").Replace(" font=\"0\"", "");
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
                    switch (line[1])
                    {
                        case "1"://множители Лагранжа
                        {
                                int i = 0;
                                double result1 = 0, result2 = 0;
                                string lineWithReplace = "", lineWithReplace2 = "", lineWithReplace1 = "", lineWithReplace3 = "", lineWithReplace4 = "";
                                do
                                {
                                    while (i < 6)
                                    {
                                        currentLine = findHeadSequence(line[i + 2], text);
                                        poland = transFromMcdToPoland(currentLine);
                                        poland = poland.Replace("[", "").Replace("]", "");
                                        result1 = calculater(poland);
                                        lineWithReplace = line[i + 2].Replace("[", "").Replace("]", "");
                                        result2 = calculater(lineWithReplace);
                                        if (result1 == result2) onLabel(labels[i],"+");
                                        else
                                        {
                                            onLabel(labels[i], "-");
                                            error(i + 1);
                                            err = i + 1;
                                            break;
                                        }
                                        i++;
                                    }
                                    if (err != 0) break;
                                    while (i < 8)
                                    {
                                        lineWithReplace1 = line[i - 2].Replace("[", "").Replace("]", "");
                                        lineWithReplace2 = line[i].Replace("]", "").Replace("[", "");
                                        lineWithReplace = transFromMcdToPoland(knife(findHead(groupForHead(lineWithReplace1, lineWithReplace2), text), text));
                                        lineWithReplace = lineWithReplace.Replace("[", "").Replace("]", "");
                                        result1 = calculater(lineWithReplace);
                                        result2 = calculater(groupForSequences(lineWithReplace1, lineWithReplace2));
                                        if (result1 == result2) onLabel(labels[i], "+");
                                        else
                                        {
                                            onLabel(labels[i], "-");
                                            error(i + 1);
                                            err = i + 1;
                                            break;
                                        }
                                        i++;
                                    }
                                    if (err != 0) break;
                                    lineWithReplace1 = halfLine(line[4], 0);
                                    lineWithReplace2 = halfLine(line[6], 0);
                                    lineWithReplace3 = halfLine(line[5], 0);
                                    lineWithReplace4 = halfLine(line[7], 0);
                                    string arg1 = groupForHead(lineWithReplace1, lineWithReplace2).Replace("<ml:symResult>\n", "");
                                    string arg2 = transFromPolandToMcd(halfLine(line[2])).Replace("<ml:function>", "<ml:apply>").Replace("</ml:function>", "</ml:apply>").Replace("<ml:boundVars>", "<ml:sequence>").Replace("</ml:boundVars>", "</ml:sequence>");
                                    string arg3 = groupForHead(lineWithReplace3, lineWithReplace4).Replace("<ml:symResult>\n", "");
                                    string matrix = matrixCraft(arg1, arg2, arg3).Replace("_", " ");//132
                                    if (text.IndexOf(matrix) != -1) onLabel(labelMatrix, "+");
                                    else
                                    {
                                        matrix = matrixCraft(arg1, arg3, arg2).Replace("_", " ");//123
                                        if (text.IndexOf(matrix) != -1) onLabel(labelMatrix, "+");
                                        else
                                        {
                                            matrix = matrixCraft(arg2, arg1, arg3).Replace("_", " ");//231
                                            if (text.IndexOf(matrix) != -1) onLabel(labelMatrix, "+");
                                            else
                                            {
                                                matrix = matrixCraft(arg3, arg1, arg2).Replace("_", " ");//213
                                                if (text.IndexOf(matrix) != -1) onLabel(labelMatrix, "+");
                                                else
                                                {
                                                    matrix = matrixCraft(arg2, arg1, arg3).Replace("_", " ");//321
                                                    if (text.IndexOf(matrix) != -1) onLabel(labelMatrix, "+");
                                                    else
                                                    {
                                                        matrix = matrixCraft(arg2, arg3, arg1).Replace("_", " ");//312
                                                        if (text.IndexOf(matrix) != -1) onLabel(labelMatrix, "+");
                                                        else
                                                        {
                                                            onLabel(labelMatrix, "-");
                                                            error(i + 1);
                                                            err = i + 1;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (err != 0) break;
                                    double sumRowsFile = calculater(transFromMcdToPoland(knife(findHead(groupForHead(lineWithReplace1, lineWithReplace2), text), text)).Replace("]", "").Replace("[", "")) + calculater(transFromMcdToPoland(knife(findHead(groupForHead(lineWithReplace3, lineWithReplace4), text), text)).Replace("]", "").Replace("[", "")) + calculater(transFromMcdToPoland(knife(findHead(transFromPolandToMcd(halfLine(line[2])).Replace("_", " "), text), text)));
                                    lineWithReplace1 = halfLine(line[4], 1);
                                    lineWithReplace2 = halfLine(line[6], 1);
                                    lineWithReplace3 = halfLine(line[5], 1);
                                    lineWithReplace4 = halfLine(line[7], 1);
                                    double sumRowsDb = calculater("(+ " + lineWithReplace1 + " (* λ " + lineWithReplace2 + "))") + calculater("(+ " + lineWithReplace3 + " (* λ " + lineWithReplace4 + "))") + calculater(halfLine(line[2], 1));
                                    if (sumRowsFile == sumRowsDb) onLabel(labelMatrix2, "+");
                                    else
                                    {
                                        onLabel(labelMatrix2, "-");
                                        error(i + 1);
                                        err = i + 1;
                                        break;
                                    }
                                    if (err != 0) break;
                                    if (checkFyn(matrix, text)) onLabel(labelFynMatrix, "+");
                                    else
                                    {
                                        onLabel(labelFynMatrix, "-");
                                        error(i + 1);
                                        err = i + 1;
                                        break;
                                    }
                                    if (err != 0) break;
                                    if (equalNumber("x", text) && equalNumber("y", text) && equalNumber("λ", text)) onLabel(labelX1Y1L1, "+");
                                    else
                                    {
                                        onLabel(labelX1Y1L1, "-");
                                        error(i + 1);
                                        err = i + 1;
                                        break;
                                    }
                                    if (err != 0) break;
                                    if (text.IndexOf("Given") != -1) onLabel(labelGiven, "+"); 
                                    else
                                    {
                                        onLabel(labelGiven, "-");
                                        error(i + 1);
                                        err = i + 1;
                                        break;
                                    }
                                    if (err != 0) break;
                                    if (checkFynXYL(text)) onLabel(labelFynXYL, "+"); 
                                    else
                                    {
                                        onLabel(labelFynXYL, "-");
                                        error(i + 1);
                                        err = i + 1;
                                        break;
                                    }
                                    if (err != 0) break;
                                    if (checkAnswer(text, line[8], line[9], line[10])) onLabel(labelAnswers, "+");
                                    else
                                    {
                                        onLabel(labelAnswers, "-");
                                        error(i + 1);
                                        err = i + 1;
                                        break;
                                    }
                                } while (false);
                                break;
                        }
                        case "2"://без ограничений
                            {
                                string quest = transFromPolandToMcd(halfLine(line[2])) + transFromPolandToMcd(halfLine(line[2], 1));
                                if (text.IndexOf(quest) != -1) onLabel(labels[0], "+");
                                else onLabel(labels[0], "-");
                                if(equalNumber("x", text)&& equalNumber("y", text)&& equalNumber("z", text)) onLabel(labels[1], "+");
                                else onLabel(labels[1], "-");
                                if () onLabel(labelAnswers, "+");
                                else
                                {
                                    onLabel(labelAnswers, "-");
                                    error(i + 1);
                                    err = i + 1;
                                    break;
                                }
                                break;
                            }
                        default:break;
                    }
                    
                }
                else MessageBox.Show("Вариант не найден!");
            }
        }
    }
}


