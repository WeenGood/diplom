namespace diplom
{
    partial class mainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.check = new System.Windows.Forms.Button();
            this.options = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.theory = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // check
            // 
            this.check.Location = new System.Drawing.Point(43, 68);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(221, 23);
            this.check.TabIndex = 0;
            this.check.Text = "Проверить ответ";
            this.check.UseVisualStyleBackColor = true;
            this.check.Click += new System.EventHandler(this.button1_Click);
            // 
            // options
            // 
            this.options.Location = new System.Drawing.Point(43, 156);
            this.options.Name = "options";
            this.options.Size = new System.Drawing.Size(221, 23);
            this.options.TabIndex = 1;
            this.options.Text = "Настройки базы данных";
            this.options.UseVisualStyleBackColor = true;
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(43, 203);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(221, 23);
            this.exit.TabIndex = 2;
            this.exit.Text = "Выход";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // theory
            // 
            this.theory.Location = new System.Drawing.Point(43, 111);
            this.theory.Name = "theory";
            this.theory.Size = new System.Drawing.Size(221, 23);
            this.theory.TabIndex = 3;
            this.theory.Text = "Теория";
            this.theory.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(221, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Получить задание";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // mainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 249);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.theory);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.options);
            this.Controls.Add(this.check);
            this.Name = "mainMenu";
            this.Text = "Главное меню";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainMenu_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button check;
        private System.Windows.Forms.Button options;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button theory;
        private System.Windows.Forms.Button button1;
    }
}