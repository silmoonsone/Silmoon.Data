namespace WinFormTest
{
    partial class MongoDBTestForm
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
            ctlWriteButton = new Button();
            ctlAbortSessionButton = new Button();
            ctlCommitSessionButton = new Button();
            ctlWriteBySessionButton = new Button();
            ctlStartSessionButton = new Button();
            SuspendLayout();
            // 
            // ctlWriteButton
            // 
            ctlWriteButton.Location = new Point(119, 64);
            ctlWriteButton.Name = "ctlWriteButton";
            ctlWriteButton.Size = new Size(75, 23);
            ctlWriteButton.TabIndex = 7;
            ctlWriteButton.Text = "Write";
            ctlWriteButton.UseVisualStyleBackColor = true;
            ctlWriteButton.Click += ctlWriteButton_Click;
            // 
            // ctlAbortSessionButton
            // 
            ctlAbortSessionButton.Location = new Point(281, 64);
            ctlAbortSessionButton.Name = "ctlAbortSessionButton";
            ctlAbortSessionButton.Size = new Size(75, 23);
            ctlAbortSessionButton.TabIndex = 9;
            ctlAbortSessionButton.Text = "AbortSession";
            ctlAbortSessionButton.UseVisualStyleBackColor = true;
            ctlAbortSessionButton.Click += ctlAbortSessionButton_Click;
            // 
            // ctlCommitSessionButton
            // 
            ctlCommitSessionButton.Location = new Point(200, 64);
            ctlCommitSessionButton.Name = "ctlCommitSessionButton";
            ctlCommitSessionButton.Size = new Size(75, 23);
            ctlCommitSessionButton.TabIndex = 8;
            ctlCommitSessionButton.Text = "CommitSession";
            ctlCommitSessionButton.UseVisualStyleBackColor = true;
            ctlCommitSessionButton.Click += ctlCommitSessionButton_Click;
            // 
            // ctlWriteBySessionButton
            // 
            ctlWriteBySessionButton.Location = new Point(119, 35);
            ctlWriteBySessionButton.Name = "ctlWriteBySessionButton";
            ctlWriteBySessionButton.Size = new Size(75, 23);
            ctlWriteBySessionButton.TabIndex = 6;
            ctlWriteBySessionButton.Text = "WriteBySession";
            ctlWriteBySessionButton.UseVisualStyleBackColor = true;
            ctlWriteBySessionButton.Click += ctlWriteBySessionButton_Click;
            // 
            // ctlStartSessionButton
            // 
            ctlStartSessionButton.Location = new Point(38, 64);
            ctlStartSessionButton.Name = "ctlStartSessionButton";
            ctlStartSessionButton.Size = new Size(75, 23);
            ctlStartSessionButton.TabIndex = 5;
            ctlStartSessionButton.Text = "StartSession";
            ctlStartSessionButton.UseVisualStyleBackColor = true;
            ctlStartSessionButton.Click += ctlStartSessionButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ctlWriteButton);
            Controls.Add(ctlAbortSessionButton);
            Controls.Add(ctlCommitSessionButton);
            Controls.Add(ctlWriteBySessionButton);
            Controls.Add(ctlStartSessionButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button ctlWriteButton;
        private Button ctlAbortSessionButton;
        private Button ctlCommitSessionButton;
        private Button ctlWriteBySessionButton;
        private Button ctlStartSessionButton;
    }
}
