using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using SV21T1020687.BusinessLayers;
using static System.Net.Mime.MediaTypeNames;

namespace SV21T1020687.Web
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn tỉnh/thành --"
            });
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName
                });
            }
            return list;
        }

        /// <summary>
        /// Loại hàng 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn loại hàng --"
            });
            int rowCount;
            foreach (var item in CommonDataService.ListOfCategory(out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryId.ToString(),
                    Text = item.CategoryName
                });
            }
            return list;
        }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhà cung cấp --"
            });
            int rowCount;
            foreach (var item in CommonDataService.ListOfSupplier(out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierId.ToString(),
                    Text = item.SupplierName
                });
            }
            return list;
        }

        /// <summary>
        /// Khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Customers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn khách hàng --"
            });
            int rowCount;
            foreach (var item in CommonDataService.ListOfCustomer(out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CustomerId.ToString(),
                    Text = item.CustomerName
                });
            }
            return list;
        }

        public static List<SelectListItem> Shippers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn người giao hàng --"
            });
            int rowCount;
            foreach (var item in CommonDataService.ListOfShipper(out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ShipperId.ToString(),
                    Text = item.ShipperName
                });
            }
            return list;
        }

        /// <summary>
        /// Nhân viên
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Employees()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhân viên --"
            });
            int rowCount;
            foreach (var item in CommonDataService.ListOfEmployee(out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.EmployeeId.ToString(),
                    Text = item.FullName
                });
            }
            return list;
        }

        public static List<SelectListItem> Prices()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Chọn giá --"
            });
            int rowCount;
            foreach (var item in CommonDataService.ListOfEmployee(out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.EmployeeId.ToString(),
                    Text = item.FullName
                });
            }
            return list;
        }

        //public static List<SelectList> StatusOfOrder()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    list.Add(new SelectListItem()
        //    {
        //        Value = "0",
        //        Text = "-- Chọn trạng thái --"
        //    });
        //    int rowCount;
        //    foreach (var item in CommonDataService.ListOfEmployee(out rowCount))
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Value = item.EmployeeId.ToString(),
        //            Text = item.FullName
        //        });
        //    }
        //    return list;
        //}
    }
}
