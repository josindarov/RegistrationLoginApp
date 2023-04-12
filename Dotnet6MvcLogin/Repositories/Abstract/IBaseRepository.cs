using System.ComponentModel;
using System.Linq.Expressions;
using System.Security.Principal;
using Dotnet6MvcLogin.Models.Domain;

namespace Dotnet6MvcLogin.Repositories.Abstract
{
    public interface  IBaseRepository
    { 
        IQueryable<ApplicationUser> GetAll();
        ApplicationUser? GetById(int id);
        bool Create(ApplicationUser entity);
        bool Update(int id);
        bool Delete(int id);
    }
}
