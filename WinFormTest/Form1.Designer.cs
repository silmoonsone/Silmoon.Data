namespace WinFormTest
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
            ctlSqlServerButton = new Button();
            ctlMongoDbButton = new Button();
            ctlPostgreSqlButton = new Button();
            SuspendLayout();
            // 
            // ctlSqlServerButton
            // 
            ctlSqlServerButton.Location = new Point(61, 50);
            ctlSqlServerButton.Name = "ctlSqlServerButton";
            ctlSqlServerButton.Size = new Size(75, 23);
            ctlSqlServerButton.TabIndex = 0;
            ctlSqlServerButton.Text = "SqlServer";
            ctlSqlServerButton.UseVisualStyleBackColor = true;
            ctlSqlServerButton.Click += ctlSqlServerButton_Click;
            // 
            // ctlMongoDbButton
            // 
            ctlMongoDbButton.Location = new Point(175, 50);
            ctlMongoDbButton.Name = "ctlMongoDbButton";
            ctlMongoDbButton.Size = new Size(75, 23);
            ctlMongoDbButton.TabIndex = 1;
            ctlMongoDbButton.Text = "MongoDB";
            ctlMongoDbButton.UseVisualStyleBackColor = true;
            ctlMongoDbButton.Click += ctlMongoDbButton_Click;
            // 
            // ctlPostgreSqlButton
            // 
            ctlPostgreSqlButton.Location = new Point(292, 50);
            ctlPostgreSqlButton.Name = "ctlPostgreSqlButton";
            ctlPostgreSqlButton.Size = new Size(75, 23);
            ctlPostgreSqlButton.TabIndex = 2;
            ctlPostgreSqlButton.Text = "PostgreSql";
            ctlPostgreSqlButton.UseVisualStyleBackColor = true;
            ctlPostgreSqlButton.Click += ctlPostgreSqlButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ctlPostgreSqlButton);
            Controls.Add(ctlMongoDbButton);
            Controls.Add(ctlSqlServerButton);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button ctlSqlServerButton;
        private Button ctlMongoDbButton;
        private Button ctlPostgreSqlButton;
    }
}