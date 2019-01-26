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
using System.Security.Cryptography;
using System.Text;

//ВАРИАНТ|  МЕТОД|    ЗАДАНИЕ|  ЗАДАНИЕ|  dx f(x,y)|  dy f(x,y)|  dx g(x,y)|  dy g(x,y)|  dx f(x,y) + lya * dx g(x,y)|    dy f(x,y) + lya * dy g(x,y)|   x|  y|  lya| x|  y|  lya

//методы: 1 - ммл, 2 - ?

namespace diplom
{
    public partial class mainMenu : Form
    {
        string[] allLines;
        string codeEnc = "115257", text;

        public mainMenu()
        {   
            InitializeComponent();

            


            StreamReader encReader = new StreamReader("db.txt");
            string original = encReader.ReadToEnd();
            encReader.Close();

            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            byte[] IV = { 156, 158, 224, 153, 115, 56, 171, 196 };
            ICryptoTransform decryptor = rc2CSP.CreateDecryptor(Encoding.Default.GetBytes(codeEnc), IV);
            MemoryStream msDecrypt = new MemoryStream(Encoding.Default.GetBytes(original));
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            StringBuilder roundtrip = new StringBuilder();

            int b = 0;
            do
            {
                b = csDecrypt.ReadByte();

                if (b != -1)
                {
                    roundtrip.Append((char)b);
                }

            } while (b != -1);
            msDecrypt.Close();
            csDecrypt.Close();

            text = roundtrip.ToString();
            if (text.IndexOf("admin") == 0)
            {
                int length = 1;
                foreach (var a in text)
                    if (a == '\n') length++;
                allLines = new string[length];
                allLines = text.Split('\n');
            }
            else MessageBox.Show("Что-то случилось с базой данных, попробуйте переустановить программу.", "Ошибка");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 checkAnswer = new Form1(allLines);
            checkAnswer.Show();
        }

        private void mainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            string text = "";
            foreach (var a in allLines)
                text += a;
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            byte[] key = Encoding.Default.GetBytes(codeEnc);
            byte[] IV = { 156, 158, 224, 153, 115, 56, 171, 196 };

            ICryptoTransform encryptor = rc2CSP.CreateEncryptor(rc2CSP.Key, IV);
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            byte[] toEncrypt = Encoding.Default.GetBytes(text);
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();
            byte[] encrypted = msEncrypt.ToArray();

            FileInfo test = new FileInfo("C:/Users/Ween Good/Desktop/db.txt");
            if (test.Exists) test.Delete();
            StreamWriter myWriter = new StreamWriter("C:/Users/Ween Good/Desktop/db.txt");

            myWriter.Write(Encoding.Default.GetString(encrypted));
            myWriter.Close();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
