namespace Client
{
    partial class Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnListFiles = new System.Windows.Forms.Button();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstLocalFiles = new System.Windows.Forms.ListBox();
            this.lblLocalDirectory = new System.Windows.Forms.Label();
            this.lblRemoteDirectory = new System.Windows.Forms.Label();
            this.lstRemoteFiles = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(26, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(364, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "File Manager Client";
            // 
            // btnListFiles
            // 
            this.btnListFiles.Location = new System.Drawing.Point(34, 74);
            this.btnListFiles.Name = "btnListFiles";
            this.btnListFiles.Size = new System.Drawing.Size(104, 32);
            this.btnListFiles.TabIndex = 1;
            this.btnListFiles.Text = "List Files";
            this.btnListFiles.UseVisualStyleBackColor = true;
            this.btnListFiles.Click += new System.EventHandler(this.btnListFiles_Click);
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(144, 74);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(104, 32);
            this.btnGetFile.TabIndex = 2;
            this.btnGetFile.Text = "Get File";
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Location = new System.Drawing.Point(254, 74);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(104, 32);
            this.btnAddFile.TabIndex = 3;
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(279, 396);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 32);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstLocalFiles
            // 
            this.lstLocalFiles.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLocalFiles.ItemHeight = 18;
            this.lstLocalFiles.Location = new System.Drawing.Point(24, 304);
            this.lstLocalFiles.Name = "lstLocalFiles";
            this.lstLocalFiles.Size = new System.Drawing.Size(359, 76);
            this.lstLocalFiles.TabIndex = 9;
            // 
            // lblLocalDirectory
            // 
            this.lblLocalDirectory.AutoSize = true;
            this.lblLocalDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalDirectory.Location = new System.Drawing.Point(151, 266);
            this.lblLocalDirectory.Name = "lblLocalDirectory";
            this.lblLocalDirectory.Size = new System.Drawing.Size(84, 32);
            this.lblLocalDirectory.TabIndex = 7;
            this.lblLocalDirectory.Text = "Local";
            // 
            // lblRemoteDirectory
            // 
            this.lblRemoteDirectory.AutoSize = true;
            this.lblRemoteDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemoteDirectory.Location = new System.Drawing.Point(148, 132);
            this.lblRemoteDirectory.Name = "lblRemoteDirectory";
            this.lblRemoteDirectory.Size = new System.Drawing.Size(114, 32);
            this.lblRemoteDirectory.TabIndex = 4;
            this.lblRemoteDirectory.Text = "Remote";
            // 
            // lstRemoteFiles
            // 
            this.lstRemoteFiles.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstRemoteFiles.FormattingEnabled = true;
            this.lstRemoteFiles.ItemHeight = 18;
            this.lstRemoteFiles.Location = new System.Drawing.Point(24, 170);
            this.lstRemoteFiles.Name = "lstRemoteFiles";
            this.lstRemoteFiles.Size = new System.Drawing.Size(359, 76);
            this.lstRemoteFiles.TabIndex = 6;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnRefresh.Location = new System.Drawing.Point(268, 265);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(42, 33);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnLocalRefresh_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(419, 451);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lstRemoteFiles);
            this.Controls.Add(this.lblRemoteDirectory);
            this.Controls.Add(this.lblLocalDirectory);
            this.Controls.Add(this.lstLocalFiles);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddFile);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.btnListFiles);
            this.Controls.Add(this.lblTitle);
            this.Name = "Client";
            this.Text = "File Manager Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnListFiles;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLocalDirectory;
        private System.Windows.Forms.Label lblRemoteDirectory;
        private System.Windows.Forms.ListBox lstRemoteFiles;
        private System.Windows.Forms.ListBox lstLocalFiles;
        private System.Windows.Forms.Button btnRefresh;
    }
}

