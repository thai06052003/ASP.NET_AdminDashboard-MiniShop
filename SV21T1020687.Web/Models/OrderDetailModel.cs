using SV21T1020687.DomainModels;

namespace SV21T1020687.Web.Models
{
    public class OrderDetailModel
    {
        public Order? Order { get; set; }
        public List<OrderDetail>? Details { get; set; }
    }
}
