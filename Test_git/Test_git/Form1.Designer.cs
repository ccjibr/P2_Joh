namespace Test_git
{
    partial class Form1
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
            this.cbTest = new System.Windows.Forms.CheckBox();
            this.tbInmatning = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbTest
            // 
            this.cbTest.AutoSize = true;
            this.cbTest.Location = new System.Drawing.Point(32, 39);
            this.cbTest.Name = "cbTest";
            this.cbTest.Size = new System.Drawing.Size(76, 21);
            this.cbTest.TabIndex = 0;
            this.cbTest.Text = "cb_test";
            this.cbTest.UseVisualStyleBackColor = true;
            // 
            // tbInmatning
            // 
            this.tbInmatning.Location = new System.Drawing.Point(32, 66);
            this.tbInmatning.Name = "tbInmatning";
            this.tbInmatning.Size = new System.Drawing.Size(100, 22);
            this.tbInmatning.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.tbInmatning);
            this.Controls.Add(this.cbTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbTest;
        private System.Windows.Forms.TextBox tbInmatning;
    }
}