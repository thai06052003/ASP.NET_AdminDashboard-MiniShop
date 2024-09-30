using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020687.BusinessLayers;
using SV21T1020687.DomainModels;

namespace SV21T1020687.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategory(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            Models.CategorySearchResult model = new Models.CategorySearchResult()
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
            ViewBag.Title = "Bổ sung loại hàng";
            Category category = new Category()
            {
                CategoryId = 0
            };
            return View("Edit", category);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật loại hàng";
            Category? category = CommonDataService.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Save(Category data)
        {
            if (string.IsNullOrWhiteSpace(data.CategoryName))
            {
                ModelState.AddModelError(nameof(data.CategoryName), "Tên sản phẩm không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.Description))
            {
                ModelState.AddModelError(nameof(data.Description), "Tên sản phẩm không được để trống");
            }

            // nếu tồn tại lỗi thì trả dữ liệu về cho view để người sử dụng nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.CategoryId == 0 ? "Bổ sung sản phẩm" : "Cập nhật thông tin sản phẩm";
                return View("Edit", data);
            }

            // Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.CategoryId == 0)
            {
                CommonDataService.AddCategory(data);
            }
            else
            {
                CommonDataService.UpdateCategory(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            Category? category = CommonDataService.GetCategory(id);
            if (category == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);

            return View(category);
        }
    }
}
