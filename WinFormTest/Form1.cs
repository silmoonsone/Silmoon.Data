using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ctlSqlServerButton_Click(object sender, EventArgs e)
        {
            var form = new SqlServerTestForm();
            form.FormClosed += (s, e) => Close();
            form.Show();
            Hide();
        }

        private void ctlMongoDbButton_Click(object sender, EventArgs e)
        {
            var form = new MongoDBTestForm();
            form.FormClosed += (s, e) => Close();
            form.Show();
            Hide();
        }

        private void ctlPostgreSqlButton_Click(object sender, EventArgs e)
        {
            var form = new PostgreSqlTestForm();
            form.FormClosed += (s, e) => Close();
            form.Show();
            Hide();
        }
    }
}
