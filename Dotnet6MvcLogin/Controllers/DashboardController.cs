using Dotnet6MvcLogin.Models.Domain;
using Dotnet6MvcLogin.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet6MvcLogin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IBaseRepository _repository;

        public DashboardController(IBaseRepository repository)
        {
            _repository = repository;
        }
        
        public IActionResult Display()
        {
            var result = _repository.GetAll();
            return View(result);
        }

       
    }
}
