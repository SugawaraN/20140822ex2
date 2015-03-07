namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxDepth = new System.Windows.Forms.PictureBox();
            this.pictureBoxTest = new System.Windows.Forms.PictureBox();
            this.pictureBoxTest2 = new System.Windows.Forms.PictureBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTest2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxDepth
            // 
            this.pictureBoxDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxDepth.Location = new System.Drawing.Point(12, 29);
            this.pictureBoxDepth.Name = "pictureBoxDepth";
            this.pictureBoxDepth.Size = new System.Drawing.Size(495, 378);
            this.pictureBoxDepth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDepth.TabIndex = 0;
            this.pictureBoxDepth.TabStop = false;
            // 
            // pictureBoxTest
            // 
            this.pictureBoxTest.Location = new System.Drawing.Point(600, 212);
            this.pictureBoxTest.Name = "pictureBoxTest";
            this.pictureBoxTest.Size = new System.Drawing.Size(287, 195);
            this.pictureBoxTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTest.TabIndex = 1;
            this.pictureBoxTest.TabStop = false;
            // 
            // pictureBoxTest2
            // 
            this.pictureBoxTest2.Location = new System.Drawing.Point(600, 29);
            this.pictureBoxTest2.Name = "pictureBoxTest2";
            this.pictureBoxTest2.Size = new System.Drawing.Size(287, 177);
            this.pictureBoxTest2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTest2.TabIndex = 2;
            this.pictureBoxTest2.TabStop = false;
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonSave.Location = new System.Drawing.Point(513, 29);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 53);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "記録";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBox1
            // 
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBox1.Location = new System.Drawing.Point(513, 121);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "test";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(513, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "被験者名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(516, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "条件";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Items.AddRange(new object[] {
            "0",
            "15",
            "30",
            "left",
            "right",
            "back"});
            this.listBox1.Location = new System.Drawing.Point(513, 185);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 88);
            this.listBox1.TabIndex = 7;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "-15",
            "-10",
            "-5",
            "0",
            "5",
            "10",
            "15",
            "swing",
            "swing_low",
            "swing_high"});
            this.comboBox1.Location = new System.Drawing.Point(512, 296);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 419);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.pictureBoxTest2);
            this.Controls.Add(this.pictureBoxTest);
            this.Controls.Add(this.pictureBoxDepth);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTest2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDepth;
        private System.Windows.Forms.PictureBox pictureBoxTest;
        private System.Windows.Forms.PictureBox pictureBoxTest2;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

