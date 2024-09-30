using SV21T1020687.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020687.DataLayers
{
    public interface IOrderDAL
    {
        /// <summary>
        /// Tim kiem va lay danh sach don hang duoi dang phan trang
        /// </summary>
        IList<Order> List(int page = 1, int pageSize = 0,
        int status = 0, DateTime? fromTime = null, DateTime? toTime = null,string searchValue = "");
        /// <summary>
        /// Dem so luong don hang thoa dieu kien tim kiem
        /// </summary>
        int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "");
        /// <summary>
        /// Lay thong tin don hang theo ma don hang
        /// </summary>
        Order? Get(int orderID);
        /// <summary>
        /// Bo sung don hang moi
        /// </summary>
        int Add(Order data);
        /// <summary>
        /// Cap nhat thong tin cua don hang
        /// </summary>
        bool Update(Order data);
        /// <summary>
        /// Xoa don hang va chi tiet cua don hang
        /// </summary>
        bool Delete(int orderID);
        /// <summary>
        /// Lay danh sach hang duoc ban trong don hang (chi tiet don hang)
        /// </summary>
        IList<OrderDetail> ListDetails(int orderID);
        /// <summary>
        /// Lay thong tin cua 1 mat hang duoc ban trong don hang (1 chi tiet trong don hang)
        /// </summary>
        OrderDetail? GetDetail(int orderID, int productID);
        /// <summary>
        /// Them mat hang duoc ban trong don hang theo nguyen tac:
        /// - Neu mat hang chua co trong chi tiet don hang thi bo sung
        /// - Neu mat hang da co trong chi tiet don hang thi cap nhat lai so luong va gia ban
        /// </summary>
        bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice);
        /// <summary>
        /// Xoa 1 mat hang ra khoi don hang
        /// </summary>
        bool DeleteDetail(int orderID, int productID);

    }
}
