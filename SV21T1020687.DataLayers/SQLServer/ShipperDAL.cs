using Dapper;
using SV21T1020687.DomainModels;
using System.Data;

namespace SV21T1020687.DataLayers.SQLServer
{
    public class ShipperDAL : _BaseDAL, ICommonDAL<Shipper>
    {
        public ShipperDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Shipper data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                            INSERT INTO Shippers(ShipperName, Phone) 
                            VALUES(@ShipperName, @Phone);
                            
                            SELECT @@IDENTITY";
                var parameter = new
                {
                    ShipperName = data.ShipperName ?? "",
                    Phone = data.Phone ?? ""
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
		                    from Shippers
		                    where (ShipperName like @searchValue) or (Phone like @searchValue)";
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
                var sql = @"DELETE FROM Shippers WHERE ShipperId = @ShipperId";
                var parameters = new
                {
                    ShipperId = id
                };
                result = connection.Execute(sql, parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Shipper? Get(int id)
        {
            Shipper? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Shippers WHERE ShipperId = @ShipperId";
                var parameters = new
                {
                    ShipperId = id
                };
                data = connection.QueryFirstOrDefault<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (SELECT * FROM Orders WHERE ShipperId = @ShipperId)
                                SELECT 1
                            ELSE
                                SELECT 0";
                var parameters = new { ShipperId = id };
                result = connection.ExecuteScalar<bool>(sql, parameters, commandType: CommandType.Text);

                connection.Close();
            }
            return result;
        }

        public IList<Shipper> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Shipper> data = new List<Shipper>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                            FROM (
		                            SELECT *,
				                            row_number() OVER (order by ShipperName) AS RowNumber
		                            FROM Shippers
		                            WHERE ShipperName LIKE @searchValue or (Phone LIKE @searchValue)
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
                data = connection.Query<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }

            return data;
        }

        public bool Update(Shipper data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Shippers 
                            SET ShipperName = @ShipperName,
                                Phone  = @Phone
                            WHERE ShipperId = @ShipperId";
                var parameters = new
                {
                    data.ShipperId,
                    ShipperName = data.ShipperName ?? "",
                    Phone = data.Phone ?? ""
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
