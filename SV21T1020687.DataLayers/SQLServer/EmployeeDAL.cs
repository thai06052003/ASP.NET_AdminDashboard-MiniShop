using Dapper;
using SV21T1020687.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020687.DataLayers.SQLServer
{
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                            INSERT INTO Employees(FullName, BirthDate, Address, Phone, Email, Photo, IsWorking) 
                            VALUES(@FullName, @BirthDate, @Address, @Phone, @Email, @Photo, @IsWorking);
                            
                            SELECT @@IDENTITY";
                var parameter = new
                {
                    FullName = data.FullName ?? "",
                    data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    data.IsWorking
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
		                    from Employees
		                    where (FullName like @searchValue) or (BirthDate like @searchValue)";
                var parameters = new { searchValue = $"%{searchValue}%" };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                var parameters = new
                {
                    EmployeeId = id
                };
                result = connection.Execute(sql, parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
                var parameters = new
                {
                    EmployeeId = id
                };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (SELECT * FROM Orders WHERE EmployeeId = @EmployeeId)
                                SELECT 1
                            ELSE
                                SELECT 0";
                var parameters = new { EmployeeId = id };
                result = connection.ExecuteScalar<bool>(sql, parameters, commandType: CommandType.Text);

                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                            FROM (
		                            SELECT *,
				                            row_number() OVER (order by FullName) AS RowNumber
		                            FROM Employees
		                            WHERE FullName LIKE @searchValue or (Email LIKE @searchValue)
	                             ) as T
                            WHERE (@pageSize = 0)
	                            OR (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            ORDER BY RowNumber;";
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue = $"%{searchValue}%",
                };
                data = connection.Query<Employee>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }

            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Employees 
                            SET FullName = @FullName,
                                BirthDate  = @BirthDate,
                                Phone        = @Phone,
                                Email        = @Email,                               
                                Photo        = @Photo,
                                Address      = @Address,
                                IsWorking     = @IsWorking
                            WHERE EmployeeId = @EmployeeId";
                var parameters = new
                {
                    data.EmployeeId,
                    FullName = data.FullName ?? "",
                    data.BirthDate,
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    Address = data.Address ?? "",
                    data.IsWorking
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
