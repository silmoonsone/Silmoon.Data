using Silmoon.Data.Models;
using Silmoon.Data.SqlServer;
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
    public partial class SqlServerTestForm : Form
    {
        SqlServerExecuter sqlExecuter = new SqlServerExecuter("Server=(local); Uid=TestDB; Pwd=TestDB; Database=TestDB; TrustServerCertificate=true");
        public SqlServerTestForm()
        {
            InitializeComponent();
        }

        private void ctlMainTestButton_Click(object sender, EventArgs e)
        {
            var user = new User()
            {
                Username = "Silmoon"
            };
            //using var trans = sqlExecuter.BeginTransaction();

            sqlExecuter.CreateTable<User>("users");

            sqlExecuter.AddObject("users", user);
            //sqlExecuter.CommitTransaction(trans);

        }

        private void ctlUpdateTestButton_Click(object sender, EventArgs e)
        {
            sqlExecuter.SetObjects("users", new User() { Username = "setSilmoon" }, new { id = 4 }, x => x.Username);
        }

        private void ctlDeleteTestButton_Click(object sender, EventArgs e)
        {
            sqlExecuter.DeleteObjects("users", new { id = 4 });
        }
    }
    class User : SqlObject
    {
        public string Username { get; set; }
    }
    class UserEx : User
    {
        public string Password { get; set; }
    }

}
