using Dotnet6MvcLogin.Models.Domain;
using Dotnet6MvcLogin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dotnet6MvcLogin.Repositories.Implementation
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DatabaseContext _dbContext;

        public BaseRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<ApplicationUser> GetAll()
        {
            return _dbContext.Set<ApplicationUser>().AsQueryable();
        }
        public ApplicationUser? GetById(int id)
        {
            var user = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => Equals(x.Id, id));
            if (user is not null)
            {
                return user;
            }

            return null;
        }

        public bool Create(ApplicationUser entity)
        {
            var result = _dbContext.Set<ApplicationUser>().Add(entity);
            _dbContext.SaveChanges();
            if (result is not null)
            {
                return true;
            }
            return false;
        }

        public bool Update(int id)
        {
            var user = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => Equals(x.Id, id));
            ApplicationUser newUser = new();
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;
            _dbContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var result = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => Equals(x.Id, id));
            if (result is not null)
            {
                _dbContext.Set<ApplicationUser>().Remove(result);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }

}
