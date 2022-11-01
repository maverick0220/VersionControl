namespace VersionControl_v3 {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /*
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        */

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fromPathTextBox = new System.Windows.Forms.TextBox();
            this.toPathTextBox = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.TotalFileTransportCountLabel = new System.Windows.Forms.Label();
            this.AlreadyTransportedFileCount = new System.Windows.Forms.Label();
            this.doCopyWholeFolder = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "from :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "to :";
            // 
            // fromPathTextBox
            // 
            this.fromPathTextBox.Location = new System.Drawing.Point(114, 38);
            this.fromPathTextBox.Name = "fromPathTextBox";
            this.fromPathTextBox.Size = new System.Drawing.Size(496, 27);
            this.fromPathTextBox.TabIndex = 2;
            // 
            // toPathTextBox
            // 
            this.toPathTextBox.Location = new System.Drawing.Point(114, 74);
            this.toPathTextBox.Name = "toPathTextBox";
            this.toPathTextBox.Size = new System.Drawing.Size(496, 27);
            this.toPathTextBox.TabIndex = 3;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(177, 119);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(249, 29);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "Next Transport";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartSearchAndTransport);
            // 
            // TotalFileTransportCountLabel
            // 
            this.TotalFileTransportCountLabel.AutoSize = true;
            this.TotalFileTransportCountLabel.Location = new System.Drawing.Point(56, 164);
            this.TotalFileTransportCountLabel.Name = "TotalFileTransportCountLabel";
            this.TotalFileTransportCountLabel.Size = new System.Drawing.Size(201, 20);
            this.TotalFileTransportCountLabel.TabIndex = 5;
            this.TotalFileTransportCountLabel.Text = "Total files transport count:";
            // 
            // AlreadyTransportedFileCount
            // 
            this.AlreadyTransportedFileCount.AutoSize = true;
            this.AlreadyTransportedFileCount.Location = new System.Drawing.Point(56, 196);
            this.AlreadyTransportedFileCount.Name = "AlreadyTransportedFileCount";
            this.AlreadyTransportedFileCount.Size = new System.Drawing.Size(235, 20);
            this.AlreadyTransportedFileCount.TabIndex = 6;
            this.AlreadyTransportedFileCount.Text = "Already Transported file count:";
            // 
            // doCopyWholeFolder
            // 
            this.doCopyWholeFolder.AutoSize = true;
            this.doCopyWholeFolder.Location = new System.Drawing.Point(448, 123);
            this.doCopyWholeFolder.Margin = new System.Windows.Forms.Padding(2);
            this.doCopyWholeFolder.Name = "doCopyWholeFolder";
            this.doCopyWholeFolder.Size = new System.Drawing.Size(163, 24);
            this.doCopyWholeFolder.TabIndex = 7;
            this.doCopyWholeFolder.Text = "copy whole folder";
            this.doCopyWholeFolder.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 281);
            this.Controls.Add(this.doCopyWholeFolder);
            this.Controls.Add(this.AlreadyTransportedFileCount);
            this.Controls.Add(this.TotalFileTransportCountLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.toPathTextBox);
            this.Controls.Add(this.fromPathTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox fromPathTextBox;
        private TextBox toPathTextBox;
        private Button StartButton;
        private Label TotalFileTransportCountLabel;
        private Label AlreadyTransportedFileCount;
        private CheckBox doCopyWholeFolder;
    }
}