namespace TriadNSim.Forms
{
    partial class frmParamForGraphs
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCountVertex = new System.Windows.Forms.TextBox();
            this.txtProb = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCountStep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtK = new System.Windows.Forms.TextBox();
            this.cbGraphModels = new System.Windows.Forms.ComboBox();
            this.txtParamA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNodeName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество вершин:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Вероятность: ";
            // 
            // txtCountVertex
            // 
            this.txtCountVertex.Location = new System.Drawing.Point(180, 27);
            this.txtCountVertex.Name = "txtCountVertex";
            this.txtCountVertex.Size = new System.Drawing.Size(50, 20);
            this.txtCountVertex.TabIndex = 2;
            // 
            // txtProb
            // 
            this.txtProb.Location = new System.Drawing.Point(180, 53);
            this.txtProb.Name = "txtProb";
            this.txtProb.Size = new System.Drawing.Size(50, 20);
            this.txtProb.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(4, 303);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(236, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "Количество вершин на каждом шаге: ";
            // 
            // txtCountStep
            // 
            this.txtCountStep.Location = new System.Drawing.Point(180, 79);
            this.txtCountStep.Name = "txtCountStep";
            this.txtCountStep.Size = new System.Drawing.Size(50, 20);
            this.txtCountStep.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Параметр k: ";
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(180, 120);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(50, 20);
            this.txtK.TabIndex = 13;
            // 
            // cbGraphModels
            // 
            this.cbGraphModels.FormattingEnabled = true;
            this.cbGraphModels.Items.AddRange(new object[] {
            "Модель Эрдеш-Реньи",
            "Модель Барабаши-Альберта",
            "Модель Боллобоша-Риордана",
            "Модель Бакли-Остгуса",
            "Модель копирования"});
            this.cbGraphModels.Location = new System.Drawing.Point(6, 19);
            this.cbGraphModels.Name = "cbGraphModels";
            this.cbGraphModels.Size = new System.Drawing.Size(224, 21);
            this.cbGraphModels.TabIndex = 14;
            this.cbGraphModels.SelectedIndexChanged += new System.EventHandler(this.cbGraphModels_SelectedIndexChanged);
            // 
            // txtParamA
            // 
            this.txtParamA.Location = new System.Drawing.Point(180, 151);
            this.txtParamA.Name = "txtParamA";
            this.txtParamA.Size = new System.Drawing.Size(50, 20);
            this.txtParamA.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Параметр a: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbGraphModels);
            this.groupBox1.Location = new System.Drawing.Point(4, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 51);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выберите модель";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtCountVertex);
            this.groupBox2.Controls.Add(this.txtParamA);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtProb);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtK);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtCountStep);
            this.groupBox2.Location = new System.Drawing.Point(4, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(236, 184);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Введите параметры модели";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNodeName);
            this.groupBox3.Location = new System.Drawing.Point(4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(236, 47);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Общее имя для вершин";
            // 
            // txtNodeName
            // 
            this.txtNodeName.Location = new System.Drawing.Point(6, 19);
            this.txtNodeName.Name = "txtNodeName";
            this.txtNodeName.Size = new System.Drawing.Size(223, 20);
            this.txtNodeName.TabIndex = 0;
            // 
            // frmParamForGraphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 329);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmParamForGraphs";
            this.Text = "Ввод данных";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCountVertex;
        private System.Windows.Forms.TextBox txtProb;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCountStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.ComboBox cbGraphModels;
        private System.Windows.Forms.TextBox txtParamA;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtNodeName;
    }
}