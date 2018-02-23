namespace AsyncManger
{
    partial class BaseProcessBarForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar_MainBar = new System.Windows.Forms.ProgressBar();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.label_ProcessDescrib = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar_MainBar
            // 
            this.progressBar_MainBar.Location = new System.Drawing.Point(12, 29);
            this.progressBar_MainBar.Name = "progressBar_MainBar";
            this.progressBar_MainBar.Size = new System.Drawing.Size(502, 23);
            this.progressBar_MainBar.TabIndex = 0;
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Location = new System.Drawing.Point(439, 58);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancle.TabIndex = 1;
            this.btn_Cancle.Text = "终止";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // label_ProcessDescrib
            // 
            this.label_ProcessDescrib.AutoSize = true;
            this.label_ProcessDescrib.Location = new System.Drawing.Point(13, 10);
            this.label_ProcessDescrib.Name = "label_ProcessDescrib";
            this.label_ProcessDescrib.Size = new System.Drawing.Size(89, 12);
            this.label_ProcessDescrib.TabIndex = 2;
            this.label_ProcessDescrib.Text = "正在运行请等待";
            // 
            // BaseProcessBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 91);
            this.Controls.Add(this.label_ProcessDescrib);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.progressBar_MainBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseProcessBarForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请等待";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseProcessBarForm_FormClosing);
            this.Load += new System.EventHandler(this.BaseProcessBarForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar_MainBar;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Label label_ProcessDescrib;
    }
}