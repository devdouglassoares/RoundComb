namespace RoundComb.Testing
{
    partial class TestLocal
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
            this.bt_createchatroom = new System.Windows.Forms.Button();
            this.btDownload = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_createchatroom
            // 
            this.bt_createchatroom.Location = new System.Drawing.Point(13, 13);
            this.bt_createchatroom.Name = "bt_createchatroom";
            this.bt_createchatroom.Size = new System.Drawing.Size(144, 23);
            this.bt_createchatroom.TabIndex = 0;
            this.bt_createchatroom.Text = "Create Chat Room";
            this.bt_createchatroom.UseVisualStyleBackColor = true;
            this.bt_createchatroom.Click += new System.EventHandler(this.bt_createchatroom_Click);
            // 
            // btDownload
            // 
            this.btDownload.Location = new System.Drawing.Point(13, 43);
            this.btDownload.Name = "btDownload";
            this.btDownload.Size = new System.Drawing.Size(144, 23);
            this.btDownload.TabIndex = 1;
            this.btDownload.Text = "Download File";
            this.btDownload.UseVisualStyleBackColor = true;
            this.btDownload.Click += new System.EventHandler(this.btDownload_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Create Contract Template";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TestLocal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 318);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btDownload);
            this.Controls.Add(this.bt_createchatroom);
            this.Name = "TestLocal";
            this.Text = "TestLocal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_createchatroom;
        private System.Windows.Forms.Button btDownload;
        private System.Windows.Forms.Button button1;
    }
}