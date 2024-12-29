namespace WinFormTest
{
    partial class SqlServerTestForm
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
            ctlInsertTestButton = new Button();
            ctlUpdateTestButton = new Button();
            ctlDeleteTestButton = new Button();
            SuspendLayout();
            // 
            // ctlInsertTestButton
            // 
            ctlInsertTestButton.Location = new Point(12, 12);
            ctlInsertTestButton.Name = "ctlInsertTestButton";
            ctlInsertTestButton.Size = new Size(75, 23);
            ctlInsertTestButton.TabIndex = 0;
            ctlInsertTestButton.Text = "Insert Test";
            ctlInsertTestButton.UseVisualStyleBackColor = true;
            ctlInsertTestButton.Click += ctlMainTestButton_Click;
            // 
            // ctlUpdateTestButton
            // 
            ctlUpdateTestButton.Location = new Point(93, 12);
            ctlUpdateTestButton.Name = "ctlUpdateTestButton";
            ctlUpdateTestButton.Size = new Size(75, 23);
            ctlUpdateTestButton.TabIndex = 1;
            ctlUpdateTestButton.Text = "Update Test";
            ctlUpdateTestButton.UseVisualStyleBackColor = true;
            ctlUpdateTestButton.Click += ctlUpdateTestButton_Click;
            // 
            // ctlDeleteTestButton
            // 
            ctlDeleteTestButton.Location = new Point(174, 12);
            ctlDeleteTestButton.Name = "ctlDeleteTestButton";
            ctlDeleteTestButton.Size = new Size(75, 23);
            ctlDeleteTestButton.TabIndex = 2;
            ctlDeleteTestButton.Text = "Delete Test";
            ctlDeleteTestButton.UseVisualStyleBackColor = true;
            ctlDeleteTestButton.Click += ctlDeleteTestButton_Click;
            // 
            // SqlServerTestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 503);
            Controls.Add(ctlDeleteTestButton);
            Controls.Add(ctlUpdateTestButton);
            Controls.Add(ctlInsertTestButton);
            Name = "SqlServerTestForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SqlServerTestForm";
            ResumeLayout(false);
        }

        #endregion

        private Button ctlInsertTestButton;
        private Button ctlUpdateTestButton;
        private Button ctlDeleteTestButton;
    }
}