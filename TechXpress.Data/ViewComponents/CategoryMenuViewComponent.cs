using Microsoft.AspNetCore.Mvc;
using TechXpress.Data.Entities ;
using TechXpress.Data.Repositories.Interfaces;

namespace TechXpress.Data.ViewComponents
{
    public class CategoryMenuViewComponent:ViewComponent
    {                                                                                            
        private readonly  IUnitOfWork _unitOfWork;

        public CategoryMenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await  _unitOfWork.Category.GetAllAsync();
            return View(categories);
        }
    }
}

