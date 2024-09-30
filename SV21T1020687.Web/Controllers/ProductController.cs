using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020687.BusinessLayers;
using SV21T1020687.DomainModels;
using SV21T1020687.Web.Models;
using System.Reflection;

namespace SV21T1020687.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;
        private const string SEARCH_CONDITION = "product_search"; //Tên biến dùng để lưu trong session
        public IActionResult Index()
        {
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SEARCH_CONDITION);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListOfProduct(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SEARCH_CONDITION, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            ViewBag.Category = CommonDataService.ListOfCategory();
            ViewBag.Supplier = CommonDataService.ListOfSupplier();
            Product product = new Product()
            {
                ProductID = 0,
                Photo = "nophoto.png"
            };
            return View("Edit", product);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";

            Product? product = ProductDataService.GetProduct(id);
            ViewBag.Photo = ProductDataService.ListOfProductPhoto(id);
            ViewBag.Attribute = ProductDataService.ListOfProductAttribute(id);
            ViewBag.Category = CommonDataService.ListOfCategory();
            ViewBag.Supplier = CommonDataService.ListOfSupplier();
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(product.Photo))
                product.Photo = "nophoto.png";
            return View(product);
        }
        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(data.ProductName))
            {
                ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được để trống");
            }

            //Xử lý với ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho product)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.WebRootPath, @"images\products"); //đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu D:\images\products\photo.png

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            // nếu tồn tại lỗi thì trả dữ liệu về cho view để người sử dụng nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                return RedirectToAction("Edit", data);
            }

            // Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.ProductID == 0)
            {
                ProductDataService.AddProduct(data);
            }
            else
            {
                ProductDataService.UpdateProduct(data);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            Product? product = ProductDataService.GetProduct(id);
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            if (product == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(id);

            return View(product);
        }
        public IActionResult Photo (int id = 0, string method = "", int photoId = 0)
        {
            ViewBag.Title = "Bổ sung ảnh mặt hàng";
            ProductPhoto data = new ProductPhoto()
            {
                PhotoID = photoId,
                ProductID = id,
                Description = "Đang cập nhật...",
                Photo = "nophoto.png"
            };
            switch (method) 
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View("photo", data);
                case "edit":
                    ViewBag.Title = "Cập nhật ảnh cho mặt hàng";
                    return View("photo", data);
                case "delete":
                    return RedirectToAction("Edit", new {id = id});
                default: return RedirectToAction("Index");
            }
        }
        public IActionResult Attribute(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính cho mặt hàng";
                    return View();
                case "delete":
                    return RedirectToAction("Edit", new { id = id });
                default: return RedirectToAction("Index");
            }
        }

        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(data.Description))
            {
                ModelState.AddModelError(nameof(data.Description), "Mô tả không được");
            }
            if (data.DisplayOrder == null)
            {
                ModelState.AddModelError(nameof(data.DisplayOrder), "thứ tự không được để trống");
            }

            //Xử lý với ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho product)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.WebRootPath, @"images\products"); //đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu D:\images\products\photo.png

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            // nếu tồn tại lỗi thì trả dữ liệu về cho view để người sử dụng nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung ảnh mặt hàng" : "Cập nhật ảnh mặt hàng";
                return RedirectToAction("Photo", data);
            }

            // Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.ProductID == 0)
            {
                ProductDataService.AddProductPhoto(data);
            }
            
            return RedirectToAction("edit", new {data.ProductID});
        }
    }
}
