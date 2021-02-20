using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class connect
    {
        private static connect obj;
        private string strConn;
        private SqlConnection con;
        private SqlCommand command;
        private SqlDataAdapter cmd;
        private connect()
        {
            strConn = @"Data Source=DESKTOP-M8O21U1\SHAHZAIB;Initial Catalog=assignment_system;User ID=sa;Password=abc.123";
            con = new SqlConnection(this.strConn);
        }

        public void openconnect()
        {
            con.Open();
        }
        public void closeconnect()
        {
            con.Close();
        }
        public SqlCommand execute(string queryString)
        {
            command = new SqlCommand(queryString, con);
            return command;
        }

        public static connect getconnect()
        {
            if (obj == null)
            {
                obj = new connect();
            }
            return obj;
        }
    }
}
