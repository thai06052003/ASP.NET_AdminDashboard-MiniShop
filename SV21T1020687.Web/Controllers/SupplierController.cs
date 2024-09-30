using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020687.BusinessLayers;
using SV21T1020687.DomainModels;
using SV21T1020687.Web.Models;
using System.Reflection;

namespace SV21T1020687.Web.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        //private const int PAGE_SIZE = 20;
        //private const string SEARCH_CONDITION = "supplier_search"; //Tên biến dùng để lưu trong session


        //public IActionResult Index()
        //{
        //    PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SEARCH_CONDITION);
        //    if (input == null)
        //    {
        //        input = new PaginationSearchInput()
        //        {
        //            Page = 1,
        //            PageSize = PAGE_SIZE,
        //            SearchValue = "",
        //        };
        //    }
        //    return View(input);
        //}

        //public IActionResult Search(PaginationSearchInput input)
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfSupplier(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
        //    var model = new SupplierSearchResult()
        //    {
        //        Page = input.Page,
        //        PageSize = input.PageSize,
        //        SearchValue = input.SearchValue ?? "",
        //        RowCount = rowCount,
        //        Data = data
        //    };
        //    ApplicationContext.SetSessionData(SEARCH_CONDITION, input);
        //    return View("Search", model);
        //}

        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSupplier(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            SupplierSearchResult model = new Models.SupplierSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";

            Supplier supplier = new Supplier()
            {
                SupplierId = 0,
                Photo = "nophoto.png"

            };
            return View("Edit", supplier);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";

            Supplier? supplier = CommonDataService.GetSupplier(id);
            if (supplier == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(supplier.Photo))
                supplier.Photo = "nophoto.png";

            return View(supplier);
        }
        [HttpPost]
        public IActionResult Save(Supplier data, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(data.SupplierName))
            {
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.ContactName))
            {
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.Province))
            {
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh/thành");
            }

            data.Phone = data.Phone ?? "";
            data.Email = data.Email ?? "";
            data.Address = data.Address ?? "";

            //Xử lý với ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho employee)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.WebRootPath, @"images\suppliers"); //đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu D:\images\employees\photo.png

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            // nếu tồn tại lỗi thì trả dữ liệu về cho view để người sử dụng nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.SupplierId == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật thông tin nhà cung cấp";
                return View("Edit", data);
            }

            // Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.SupplierId == 0)
            {
                CommonDataService.AddSupplier(data);
            }
            else
            {
                CommonDataService.UpdateSupplier(data);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }

            Supplier? supplier = CommonDataService.GetSupplier(id);
            if (supplier == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !CommonDataService.IsUsedSupplier(id);

            return View(supplier);
        }
    }
}
