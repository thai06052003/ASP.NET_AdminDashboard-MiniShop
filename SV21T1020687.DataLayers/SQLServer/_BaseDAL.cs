using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020687.DataLayers.SQLServer
{
    /// <summary>
    /// Lớp đóng vai trò là lớp "cha" cho các lớp cài đặt các phép xử lý dữ liệu trên cơ sở dữ liệu SQL Server
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";
        public _BaseDAL(string connectionString) 
        { 
            _connectionString = connectionString;
        }
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
