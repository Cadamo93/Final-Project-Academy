namespace Z75.Project_X
{
    partial class FirstPage
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPocetokRab = new System.Windows.Forms.Button();
            this.btnKrajRabota = new System.Windows.Forms.Button();
            this.tbRabota = new System.Windows.Forms.TextBox();
            this.btnPocetokPauza = new System.Windows.Forms.Button();
            this.tbPauza = new System.Windows.Forms.TextBox();
            this.btnKrajPauza = new System.Windows.Forms.Button();
            this.btnKrajRucek = new System.Windows.Forms.Button();
            this.tbRucek = new System.Windows.Forms.TextBox();
            this.btnPocetokRucek = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.usernameHidden = new System.Windows.Forms.TextBox();
            this.btnZacuvajVreme = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(293, 14);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(525, 307);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(13, 296);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "Faild Payments Edit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPocetokRab
            // 
            this.btnPocetokRab.Location = new System.Drawing.Point(834, 70);
            this.btnPocetokRab.Name = "btnPocetokRab";
            this.btnPocetokRab.Size = new System.Drawing.Size(130, 23);
            this.btnPocetokRab.TabIndex = 2;
            this.btnPocetokRab.Text = "Pocetok Rabota";
            this.btnPocetokRab.UseVisualStyleBackColor = true;
            this.btnPocetokRab.Click += new System.EventHandler(this.btnPocetokRab_Click);
            // 
            // btnKrajRabota
            // 
            this.btnKrajRabota.Location = new System.Drawing.Point(1096, 70);
            this.btnKrajRabota.Name = "btnKrajRabota";
            this.btnKrajRabota.Size = new System.Drawing.Size(130, 23);
            this.btnKrajRabota.TabIndex = 3;
            this.btnKrajRabota.Text = "Kraj Rabota";
            this.btnKrajRabota.UseVisualStyleBackColor = true;
            this.btnKrajRabota.Click += new System.EventHandler(this.btnKrajRabota_Click);
            // 
            // tbRabota
            // 
            this.tbRabota.Location = new System.Drawing.Point(970, 71);
            this.tbRabota.Name = "tbRabota";
            this.tbRabota.Size = new System.Drawing.Size(120, 22);
            this.tbRabota.TabIndex = 4;
            // 
            // btnPocetokPauza
            // 
            this.btnPocetokPauza.Location = new System.Drawing.Point(834, 99);
            this.btnPocetokPauza.Name = "btnPocetokPauza";
            this.btnPocetokPauza.Size = new System.Drawing.Size(130, 23);
            this.btnPocetokPauza.TabIndex = 5;
            this.btnPocetokPauza.Text = "Pocetok Pauza";
            this.btnPocetokPauza.UseVisualStyleBackColor = true;
            this.btnPocetokPauza.Click += new System.EventHandler(this.btnPocetokPauza_Click);
            // 
            // tbPauza
            // 
            this.tbPauza.Location = new System.Drawing.Point(970, 100);
            this.tbPauza.Name = "tbPauza";
            this.tbPauza.Size = new System.Drawing.Size(120, 22);
            this.tbPauza.TabIndex = 6;
            // 
            // btnKrajPauza
            // 
            this.btnKrajPauza.Location = new System.Drawing.Point(1096, 100);
            this.btnKrajPauza.Name = "btnKrajPauza";
            this.btnKrajPauza.Size = new System.Drawing.Size(130, 23);
            this.btnKrajPauza.TabIndex = 7;
            this.btnKrajPauza.Text = "Kraj Pauza";
            this.btnKrajPauza.UseVisualStyleBackColor = true;
            this.btnKrajPauza.Click += new System.EventHandler(this.btnKrajPauza_Click);
            // 
            // btnKrajRucek
            // 
            this.btnKrajRucek.Location = new System.Drawing.Point(1096, 128);
            this.btnKrajRucek.Name = "btnKrajRucek";
            this.btnKrajRucek.Size = new System.Drawing.Size(130, 23);
            this.btnKrajRucek.TabIndex = 10;
            this.btnKrajRucek.Text = "Kraj Rucek";
            this.btnKrajRucek.UseVisualStyleBackColor = true;
            this.btnKrajRucek.Click += new System.EventHandler(this.btnKrajRucek_Click);
            // 
            // tbRucek
            // 
            this.tbRucek.Location = new System.Drawing.Point(970, 128);
            this.tbRucek.Name = "tbRucek";
            this.tbRucek.Size = new System.Drawing.Size(120, 22);
            this.tbRucek.TabIndex = 9;
            // 
            // btnPocetokRucek
            // 
            this.btnPocetokRucek.Location = new System.Drawing.Point(834, 127);
            this.btnPocetokRucek.Name = "btnPocetokRucek";
            this.btnPocetokRucek.Size = new System.Drawing.Size(130, 23);
            this.btnPocetokRucek.TabIndex = 8;
            this.btnPocetokRucek.Text = "Pocetok Rucek";
            this.btnPocetokRucek.UseVisualStyleBackColor = true;
            this.btnPocetokRucek.Click += new System.EventHandler(this.btnPocetokRucek_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // usernameHidden
            // 
            this.usernameHidden.Location = new System.Drawing.Point(834, 14);
            this.usernameHidden.Name = "usernameHidden";
            this.usernameHidden.Size = new System.Drawing.Size(392, 22);
            this.usernameHidden.TabIndex = 11;
            // 
            // btnZacuvajVreme
            // 
            this.btnZacuvajVreme.Location = new System.Drawing.Point(834, 156);
            this.btnZacuvajVreme.Name = "btnZacuvajVreme";
            this.btnZacuvajVreme.Size = new System.Drawing.Size(392, 23);
            this.btnZacuvajVreme.TabIndex = 12;
            this.btnZacuvajVreme.Text = "Zacuvaj Vreme";
            this.btnZacuvajVreme.UseVisualStyleBackColor = true;
            this.btnZacuvajVreme.Click += new System.EventHandler(this.btnZacuvajVreme_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(834, 42);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(392, 22);
            this.dateTimePicker1.TabIndex = 13;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(47, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(239, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Failed Payments - View";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FirstPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1831, 830);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnZacuvajVreme);
            this.Controls.Add(this.usernameHidden);
            this.Controls.Add(this.btnKrajRucek);
            this.Controls.Add(this.tbRucek);
            this.Controls.Add(this.btnPocetokRucek);
            this.Controls.Add(this.btnKrajPauza);
            this.Controls.Add(this.tbPauza);
            this.Controls.Add(this.btnPocetokPauza);
            this.Controls.Add(this.tbRabota);
            this.Controls.Add(this.btnKrajRabota);
            this.Controls.Add(this.btnPocetokRab);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FirstPage";
            this.Text = "ProjectX program";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPocetokRab;
        private System.Windows.Forms.Button btnKrajRabota;
        private System.Windows.Forms.TextBox tbRabota;
        private System.Windows.Forms.Button btnPocetokPauza;
        private System.Windows.Forms.TextBox tbPauza;
        private System.Windows.Forms.Button btnKrajPauza;
        private System.Windows.Forms.Button btnKrajRucek;
        private System.Windows.Forms.TextBox tbRucek;
        private System.Windows.Forms.Button btnPocetokRucek;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.TextBox usernameHidden;
        private System.Windows.Forms.Button btnZacuvajVreme;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button2;
    }
}