using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Linq;

namespace Silmoon.Data.SqlServer
{
    /// <summary>
    /// MySQLʵ����
    /// </summary>
    public class SqlServerManager
    {
        SqlServerOperate SqlServerOperate { get; set; }
        /// <summary>
        /// ʹ��ָ����ODBC�������Ӵ���MYSQLʵ�ù���
        /// </summary>
        /// <param name="odbc">ָ��һ���Ѿ�����ʹ�õ�ODBC����</param>
        public SqlServerManager(SqlServerOperate sqlServerOperate)
        {
            SqlServerOperate = sqlServerOperate;
        }
        /// <summary>
        /// ˢ�����ݿ����ж���
        /// </summary>
        public void Refresh()
        {
            SqlServerOperate.ExecuteNonQuery("RECONFIGURE WITH OVERRIDE");
        }

        /// <summary>
        /// ����һ�����ݿ�
        /// </summary>
        /// <param name="database">���ݿ�����</param>
        /// <returns></returns>
        public int CreateDatabase(string database)
        {
            if (IsExistDatabase(database)) throw new MSSQLException(null, "���ݿ��Ѵ���");
            return SqlServerOperate.ExecuteNonQuery("CREATE DATABASE " + database);
        }
        /// <summary>
        /// ɾ��һ�����ݿ�
        /// </summary>
        /// <param name="database">���ݿ�����</param>
        /// <returns></returns>
        public int DropDatabase(string database)
        {
            if (database.ToLower() == "master") throw new MSSQLException(null, "ϵͳ���ݿ��޷�ɾ��");
            if (!IsExistDatabase(database)) throw new MSSQLException(null, "ָ����һ�������ڵ����ݿ�");
            return SqlServerOperate.ExecuteNonQuery("DROP DATABASE " + database);
        }
        /// <summary>
        /// ���һ���û������ƶ�����������ݿ�
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="database">ָ�������ݿ�</param>
        public int AddUserToDatabase(string username, string database)
        {
            if (!IsExistDatabase(database)) throw new MSSQLException(null, "ָ����һ�������ڵ����ݿ�");
            if (!IsExistUser(username)) throw new MSSQLException(null, "ָ����һ�������ڵ����ݿ�");

            int result = SqlServerOperate.ExecuteNonQuery("USE [" + database + "];CREATE USER [" + username + "] FOR LOGIN [" + username + "]");
            SqlServerOperate.ExecuteNonQuery("USE [" + database + "];EXEC sp_addrolemember N'db_owner', N'" + username + "'");
            SqlServerOperate.ExecuteNonQuery("USE [Master]");
            Refresh();
            return result;
        }
        /// <summary>
        /// ����һ���û�
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <returns></returns>
        public int CreateUser(string username, string password)
        {
            if (username == "") throw new MSSQLException(null, "������յ��û���!");
            if (IsExistUser(username)) throw new MSSQLException(null, "�û����Ѿ�����!");
            int result = SqlServerOperate.ExecuteNonQuery("CREATE LOGIN [" + username + "] WITH PASSWORD=N'" + password + "', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF");
            Refresh();
            return result;
        }
        /// <summary>
        /// ʹ������ǿ��ɾ��һ���û�
        /// </summary>
        /// <param name="username">�û���</param>
        /// <returns></returns>
        public int RemoveUser(string username)
        {
            if (username.ToLower() == "sa") throw new MSSQLException(null, "��ֹɾ��sa�û���");
            int result = SqlServerOperate.ExecuteNonQuery("DROP LOGIN [" + username + "]");
            Refresh();
            return result;
        }
        /// <summary>
        /// �Ƴ�һ���û������ݿ��Ȩ��
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="database">���ݿ�</param>
        /// <returns></returns>
        public int RemoveUserGrant(string username, string database)
        {
            if (!IsExistDatabase(database)) throw new MSSQLException(null, "ָ����һ�������ڵ����ݿ�");
            string brforeDatabase = SqlServerOperate.GetFieldObjectForSingleQuery("select db_name()").ToString();
            int result = SqlServerOperate.ExecuteNonQuery("USE " + database + ";drop user [" + username + "]");
            SqlServerOperate.ExecuteNonQuery("USE " + brforeDatabase);
            Refresh();
            return result;
        }
        /// <summary>
        /// ������ݿ��Ƿ����
        /// </summary>
        /// <param name="database">Ҫ�������ݿ�</param>
        /// <returns></returns>
        public bool IsExistDatabase(string database)
        {
            DataTable dt = SqlServerOperate.GetDataTable("Select Name From Master..SysDatabases");
            ArrayList _array = new ArrayList();
            foreach (DataRow row in dt.Rows) _array.Add(row[0]);
            string[] nameArr = (string[])_array.ToArray(typeof(string));
            dt.Clear();
            dt.Dispose();
            return nameArr.Contains(database);
        }
        /// <summary>
        /// ���һ���û����Ƿ����
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsExistUser(string username)
        {
            DataTable dt = SqlServerOperate.GetDataTable("select Name from Master..syslogins where isntname=0");
            ArrayList _array = new ArrayList();
            foreach (DataRow row in dt.Rows) _array.Add(row[0]);
            string[] nameArr = (string[])_array.ToArray(typeof(string));
            dt.Clear();
            dt.Dispose();
            return nameArr.Contains(username);
        }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="username">Ŀ���û�</param>
        /// <param name="password">����</param>
        /// <returns></returns>
        public int SetPassword(string username, string password)
        {
            int i = SqlServerOperate.ExecuteNonQuery("USE [master];ALTER LOGIN [" + username + "] WITH PASSWORD=N'" + password + "'");
            return i;
        }
        /// <summary>
        /// ����һ������MySQL ODBC 3.51��������������Դ�������ַ���
        /// </summary>
        /// <param name="hostname">������</param>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="database">���ݿ�</param>
        /// <returns></returns>
        public static string MakeConnectionString(string hostname, string username, string password, string database)
        {
            return "DRIVER={MySQL ODBC 3.51 Driver};SERVER=" + hostname + ";DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + password + ";";
        }
        /// <summary>
        /// ��ȡ�������ݿ�
        /// </summary>
        /// <returns></returns>
        public string[] GetDatabases()
        {
            List<string> list = new List<string>();
            using (DataTable dt = SqlServerOperate.GetDataTable("Select Name FROM Master.dbo.SysDatabases ORDER BY Name"))
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// ��ȡ�����û�
        /// </summary>
        /// <returns></returns>
        public string[] GetUsers()
        {
            List<string> list = new List<string>();
            using (DataTable dt = SqlServerOperate.GetDataTable("select Name from syslogins where isntname=0"))
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list.ToArray();
        }

        public string[] GetUserDatabases(string username)
        {
            List<string> list = new List<string>();

            using (DataTable namedt = SqlServerOperate.GetDataTable("Select Name FROM Master.dbo.SysDatabases ORDER BY Name"))
            {
                foreach (DataRow row in namedt.Rows)
                {
                    using (DataTable dt = SqlServerOperate.GetDataTable("use " + row["Name"].ToString() + ";select Name from sysusers where islogin=1;use master"))
                    {
                        foreach (DataRow namesrow in dt.Rows)
                            if (namesrow["Name"].ToString() == username)
                                list.Add(row["Name"].ToString());
                    }
                }
            }
            return list.ToArray();
        }

        public string[] GetDatabaseUsers(string database)
        {
            List<string> list = new List<string>();

            using (DataTable dt = SqlServerOperate.GetDataTable("use " + database + ";select Name from sysusers where islogin=1;use master"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(row["Name"].ToString());
                }
            }
            return list.ToArray();
        }
    }
    /// <summary>
    /// ��ʾMySQL�쳣
    /// </summary>
    public class MSSQLException : Exception
    {
        string _message;
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        override public string Message
        {
            get { return _message; }
        }
        Exception _innerException;
        /// <summary>
        /// ��ȡ�ڲ��쳣
        /// </summary>
        new public Exception InnerException
        {
            get { return _innerException; }
        }
        /// <summary>
        /// ʵ��������
        /// </summary>
        /// <param name="_innerException">������ǰ�쳣���ڲ��쳣</param>
        /// <param name="message">��Ϣ</param>
        public MSSQLException(Exception innerException, string message)
        {
            _innerException = innerException;
            _message = message;
        }
    }
}
