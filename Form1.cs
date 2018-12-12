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
        public Form1()
        {
            InitializeComponent();
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

            if (pathT.Text.Substring(pathT.Text.LastIndexOf(".") + 1) == "xmcd")
            {
                FileStream myStream = new FileStream(pathT.Text, FileMode.Open);
                StreamReader myReader = new StreamReader(myStream);

                string mcdStr;

                do
                {
                    mcdStr = myReader.ReadLine();
                    if (mcdStr == null)
                    {
                        MessageBox.Show("Ошибка!", "В файле не найдена переменная numA");
                        break;
                    }
                } while (mcdStr.IndexOf("numA") == -1);

                mcdStr = myReader.ReadLine();
                float numA = retNum(mcdStr);

                myReader.Close();
                myStream.Close();
                myStream.Dispose();

                myStream = new FileStream(pathT.Text, FileMode.Open);
                myReader = new StreamReader(myStream);
                do
                {
                    mcdStr = myReader.ReadLine();
                    if (mcdStr == null)
                    {
                        MessageBox.Show("Ошибка!", "В файле не найдена переменная numB");
                        break;
                    }
                } while (mcdStr.IndexOf("numB") == -1);

                mcdStr = myReader.ReadLine();
                float numB = retNum(mcdStr);

                myReader.Close();
                myStream.Close();
                myStream.Dispose();
                myStream = new FileStream(pathT.Text, FileMode.Open);
                myReader = new StreamReader(myStream);
                do
                {
                    mcdStr = myReader.ReadLine();
                    if (mcdStr == null)
                    {
                        MessageBox.Show("Ошибка!", "В файле не найдена переменная numC");
                        break;
                    }
                } while (mcdStr.IndexOf("numC") == -1);

                mcdStr = myReader.ReadLine();
                float numC = retNum(mcdStr);

                myReader.Close();
                myStream.Close();
                myStream.Dispose();

                myStream = new FileStream(pathT.Text, FileMode.Open);
                myReader = new StreamReader(myStream);
                do
                {
                    mcdStr = myReader.ReadLine();
                    if (mcdStr == null)
                    {
                        MessageBox.Show("Ошибка!", "В файле не найдена переменная discr");
                        break;
                    }
                } while (mcdStr.IndexOf("discr") == -1);

                mcdStr = myReader.ReadLine();
                float discr = retNum(mcdStr);

                myReader.Close();
                myStream.Close();
                myStream.Dispose();

                myStream = new FileStream(pathT.Text, FileMode.Open);
                myReader = new StreamReader(myStream);
                do
                {
                    mcdStr = myReader.ReadLine();
                    if (mcdStr == null)
                    {
                        MessageBox.Show("Ошибка!", "В файле не найдена переменная x1");
                        break;
                    }
                } while (mcdStr.IndexOf("x1") == -1);

                mcdStr = myReader.ReadLine();
                float x1 = retNum(mcdStr);

                myReader.Close();
                myStream.Close();
                myStream.Dispose();

                myStream = new FileStream(pathT.Text, FileMode.Open);
                myReader = new StreamReader(myStream);
                do
                {
                    mcdStr = myReader.ReadLine();
                    if (mcdStr == null)
                    {
                        MessageBox.Show("Ошибка!", "В файле не найдена переменная x2");
                        break;
                    }
                } while (mcdStr.IndexOf("x2") == -1);

                mcdStr = myReader.ReadLine();
                float x2 = retNum(mcdStr);

                myReader.Close();
                myStream.Close();
                myStream.Dispose();

                float myDiscr = (float)Math.Round(Convert.ToDouble(numB * numB - 4 * numA * numC), 3);


                float myX1 = (float)Math.Round(Convert.ToDouble(-numB - Math.Sqrt(myDiscr)) / (2 * numA), 3);
                float myX2 = (float)Math.Round(Convert.ToDouble(-numB + Math.Sqrt(myDiscr)) / (2 * numA), 3);

                labA.Text = "a = " + numA.ToString();
                labB.Text = "b = " + numB.ToString();
                labC.Text = "c = " + numC.ToString();
                labD.Text = "D = " + discr.ToString();
                labDC.Text = "D' = " + myDiscr.ToString();
                labX1.Text = "x1 = " + x1.ToString();
                labX2.Text = "x2 = " + x2.ToString();
                labX1C.Text = "x1' = " + myX1.ToString();
                labX2C.Text = "x2' = " + myX2.ToString();
                if (discr == myDiscr)
                    labDRes.Text = "+";
                else labDRes.Text = "-";
                if (x1 == myX1 || x2==myX1)
                    labX1Res.Text = "+";
                else labX1Res.Text = "-";
                if (x2 == myX2 || x1==myX2)
                    labX2Res.Text = "+";
                else labX2Res.Text = "-";
            }
            else MessageBox.Show("Ошибка!", "Неправильный формат файла!");

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
    }
}


