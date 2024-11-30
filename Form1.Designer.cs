namespace CryptoHelper
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            button2 = new Button();
            textBox3 = new TextBox();
            button3 = new Button();
            label5 = new Label();
            button4 = new Button();
            textBox4 = new TextBox();
            label7 = new Label();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Bybit", "Binance", "Okx", "BingX", "GateIo", "Coinex", "Kucoin", "Mexc", "Bitget", "Htx(не работает)" });
            comboBox1.Location = new Point(-1, 24);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedValueChanged += ChangeExchange;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(156, 24);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(151, 28);
            comboBox2.TabIndex = 1;
            comboBox2.SelectedValueChanged += ChangePare;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1, 1);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 2;
            label1.Text = "Биржи";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(160, 2);
            label2.Name = "label2";
            label2.Size = new Size(118, 20);
            label2.TabIndex = 3;
            label2.Text = "Торговые пары";
            // 
            // button1
            // 
            button1.Location = new Point(1, 53);
            button1.Name = "button1";
            button1.Size = new Size(306, 57);
            button1.TabIndex = 4;
            button1.Text = "Получить пары";
            button1.UseVisualStyleBackColor = true;
            button1.Click += AwaitPares;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(670, 35);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(139, 27);
            textBox1.TabIndex = 6;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(822, 35);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(147, 27);
            textBox2.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(670, 12);
            label3.Name = "label3";
            label3.Size = new Size(139, 20);
            label3.TabIndex = 8;
            label3.Text = "Выбранная биржа";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(815, 12);
            label4.Name = "label4";
            label4.Size = new Size(127, 20);
            label4.TabIndex = 9;
            label4.Text = "Выбранная пара";
            // 
            // button2
            // 
            button2.Location = new Point(670, 63);
            button2.Name = "button2";
            button2.Size = new Size(299, 57);
            button2.TabIndex = 10;
            button2.Text = "Получить информацию";
            button2.UseVisualStyleBackColor = true;
            button2.Click += ShowInfoPerPare;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(670, 121);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(299, 319);
            textBox3.TabIndex = 11;
            // 
            // button3
            // 
            button3.Location = new Point(313, 19);
            button3.Name = "button3";
            button3.Size = new Size(89, 91);
            button3.TabIndex = 12;
            button3.Text = "Обновить список";
            button3.UseVisualStyleBackColor = true;
            button3.Click += UpdatePares;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label5.Location = new Point(670, -25);
            label5.Name = "label5";
            label5.Size = new Size(0, 31);
            label5.TabIndex = 13;
            // 
            // button4
            // 
            button4.Location = new Point(357, 198);
            button4.Name = "button4";
            button4.Size = new Size(169, 367);
            button4.TabIndex = 14;
            button4.Text = "Запустить бота арбитражника";
            button4.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(12, 198);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(339, 378);
            textBox4.TabIndex = 16;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 155);
            label7.Name = "label7";
            label7.Size = new Size(275, 40);
            label7.TabIndex = 17;
            label7.Text = "Введите через пробел телеграм айди,\r\n на которые хотите получать сигналы ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(992, 577);
            Controls.Add(label7);
            Controls.Add(textBox4);
            Controls.Add(button4);
            Controls.Add(label5);
            Controls.Add(button3);
            Controls.Add(textBox3);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Label label1;
        private Label label2;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label3;
        private Label label4;
        private Button button2;
        private TextBox textBox3;
        private Button button3;
        private Label label5;
        private Button button4;
        private TextBox textBox4;
        private Label label7;
    }
}
