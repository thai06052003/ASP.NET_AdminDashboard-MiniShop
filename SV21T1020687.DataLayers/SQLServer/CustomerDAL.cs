using Dapper;
using SV21T1020687.DomainModels;
using System.Data;

namespace SV21T1020687.DataLayers.SQLServer
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                            INSERT INTO Customers(CustomerName, ContactName, Province, Address, Phone, Email, IsLocked) 
                            VALUES(@CustomerName, @ContactName, @Province, @Address, @Phone, @Email, @IsLocked);
                            
                            SELECT @@IDENTITY";
                var parameter = new
                {
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsLocked = data.IsLocked
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text);
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
		                    from Customers
		                    where (CustomerName like @searchValue) or (ContactName like @searchValue)";
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
                var sql = @"DELETE FROM Customers WHERE CustomerId = @CustomerId";
                var parameters = new
                {
                    CustomerId = id
                };
                result = (connection.Execute(sql, parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }

        public Customer? Get(int id)
        {
            Customer? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                var parameters = new
                {
                    CustomerId = id
                };
                data = connection.QueryFirstOrDefault<Customer>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (SELECT * FROM Orders WHERE CustomerId = @CustomerId)
                                SELECT 1
                            ELSE
                                SELECT 0";
                var parameters = new { CustomerId = id };
                result = connection.ExecuteScalar<bool>(sql, parameters, commandType: CommandType.Text);
                
                connection.Close();
            }
            return result;
        }

        public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> data = new List<Customer>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                            FROM (
		                            SELECT *,
				                            row_number() OVER (order by CustomerName) AS RowNumber
		                            FROM Customers
		                            WHERE CustomerName LIKE @searchValue or (ContactName LIKE @searchValue)
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
                data = connection.Query<Customer>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }

            return data;
        }

        public bool Update(Customer data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Customers 
                            SET CustomerName = @CustomerName,
                                ContactName  = @ContactName,
                                Phone        = @Phone,
                                Email        = @Email,
                                Address      = @Address,
                                Province     = @Province,
                                IsLocked     = @IsLocked
                            WHERE CustomerId = @CustomerId";
                var parameters = new
                {
                    CustomerId = data.CustomerId,
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Address = data.Address ?? "",
                    Province = data.Province ?? "",
                    IsLocked = data.IsLocked
                };
                result = (connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
