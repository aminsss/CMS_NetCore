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
using CMS_NetCore.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CMS_NetCore.ServiceLayer
{
    public class EfUserService : RepositoryBase<User>, IUserService
    {

        private readonly AppSettings _appSettings;


        public EfUserService(AppDbContext contex, IOptions<AppSettings> appSettings)
        : base(contex)
        {
            _appSettings = appSettings.Value;
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

        public async Task<IList<User>> GetContactctPerson() =>
             await FindAll().Include(x => x.ContactPersons).ToListAsync();

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await FindByCondition(u => u.UserName == username && u.Password == password).FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public async  Task<IEnumerable<User>> GetAll() =>
            await FindAll().ToListAsync();

    }
}
