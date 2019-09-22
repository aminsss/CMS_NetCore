using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfUserService : RepositoryBase<User>, IUserService
    {
       
        public EfUserService(AppDbContext contex)
        : base(contex)
        {
        }

        public async Task<DataGridViewModel<User>> GetBySearch(int page, int pageSize, string srchString = "")
        {
            var DataGridView = new DataGridViewModel<User>
            {
                Records = await FindByCondition(x => x.moblie.Contains(srchString)).OrderBy(x => x.UserId)
                .Include(x => x.Role).Include(x => x.chartPost)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(x => x.moblie.Contains(srchString)).OrderBy(x => x.UserId)
                .Include(x => x.Role).Include(x => x.chartPost).CountAsync()
            };

            return DataGridView;
        }

        public async Task<User> GetById(int? id) =>
            await FindByCondition(x => x.UserId.Equals(id)).DefaultIfEmpty(new User())
            .Include(x => x.Role).Include(x=>x.chartPost).SingleAsync();


        public async Task<User> GetUserByIdentity(string mobile) =>
           await FindByCondition(x => x.moblie == mobile).FirstOrDefaultAsync();


        public async Task<User> GetUserByPassword(int userId, string password) =>
            await FindByCondition(u => u.UserId == userId && u.Password == password).FirstOrDefaultAsync();

        public async Task Add(User user)
        {
            user.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
            user.AddedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            Create(user);
            await SaveAsync();
        }

        public async Task Edit(User user)
        {
            user.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
            user.ModifiedDate = DateTime.Now;
            Update(user);
            await SaveAsync();
        }

        public async Task EditPassword(User user, string password)
        {
            user.Password = password;
            Update(user);
            await SaveAsync();
        }

        public async Task Remove(User user)
        {
            Delete(user);
            await SaveAsync();
        }

        public async Task<User> UniqueEmail(string email, int userId) =>
            await FindByCondition(s => s.Email == email && s.UserId != userId).FirstOrDefaultAsync();

        public async Task<User> UniqueMobile(string mobile, int userId) =>
            await FindByCondition(s => s.Email == mobile && s.UserId != userId).FirstOrDefaultAsync();
        

        public async Task<IEnumerable<User>> Users() =>
            await FindAll().ToListAsync();
        

        public async Task<IEnumerable<User>> GetAllAdmin() =>
            await FindByCondition(x => x.RoleId == 1).Include(x=>x.Role).ToListAsync();

        public async Task<User> UserExistence(int id) =>
            await FindByCondition(x => x.UserId == id).FirstOrDefaultAsync();
            
    }
}
