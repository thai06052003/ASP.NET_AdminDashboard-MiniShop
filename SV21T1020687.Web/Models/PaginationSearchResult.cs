using SV21T1020687.DomainModels;

namespace SV21T1020687.Web.Models
{
    /// <summary>
    /// Lớp cơ sở cho các dữ liệu tìm kiếm, hiển thị dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchResult
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }
        public int PageCount
        {
            get 
            { 
                if (PageSize == 0) return 1;
                int n = RowCount / PageSize;
                if (RowCount % PageSize > 0) n += 1;
                return n;
            }

        }
    }
    /// <summary>
    /// Kết quả tìm kiếm khách hàng
    /// </summary>
    public class CustomerSearchResult : PaginationSearchResult
    {
        public List<Customer> Data { get; set; }
    }
    public class SupplierSearchResult : PaginationSearchResult
    {
        public List<Supplier> Data { get; set; }
    }
    public class ShipperSearchResult : PaginationSearchResult
    {
        public List<Shipper> Data { get; set; }
    }
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public List<Employee> Data { get; set; }
    }
    public class CategorySearchResult : PaginationSearchResult
    {
        public List<Category> Data { get; set; }
    }
    public class ProductSearchResult : PaginationSearchResult
    {
        public List<Product> Data { get; set; }
    }
    public class OrderSearchResult : PaginationSearchResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
