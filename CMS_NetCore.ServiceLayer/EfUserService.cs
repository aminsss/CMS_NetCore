using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Helpers;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CMS_NetCore.ServiceLayer;

public class EfUserService : RepositoryBase<User>, IUserService
{
    private readonly AppSettings _appSettings;


    public EfUserService(
        AppDbContext context,
        IOptions<AppSettings> appSettings
    )
        : base(context)
    {
        _appSettings = appSettings.Value;
    }

    public async Task<DataGridViewModel<User>> GetBySearch(
        int page,
        int pageSize,
        string searchString = ""
    )
    {
        return new DataGridViewModel<User>
        {
            Records = await FindByCondition(
                    user => user.Mobile
                        .Contains(searchString)
                )
                .OrderBy(user => user.Id)
                .Include(user => user.Role)
                .Include(user => user.ChartPost)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),

            TotalCount = await FindByCondition(
                    user => user.Mobile
                        .Contains(searchString)
                )
                .Include(user => user.Role)
                .Include(user => user.ChartPost)
                .CountAsync()
        };
    }

    public async Task<User> GetById(int? id) =>
        await FindByCondition(user => user.Id.Equals(id))
            .Include(user => user.Role)
            .Include(user => user.ChartPost)
            .FirstOrDefaultAsync();

    public async Task<User> GetUserByIdentity(string mobile) =>
        await FindByCondition(
            user =>
                user.Mobile == mobile
        ).FirstOrDefaultAsync();


    public async Task<User> GetUserByPassword(
        int userId,
        string password
    ) =>
        await FindByCondition(
            user =>
                user.Id == userId &&
                user.Password == password
        ).FirstOrDefaultAsync();

    public async Task Add(User user)
    {
        user.ActiveCode = Guid.NewGuid().ToString().Replace(
            "-",
            ""
        );

        user.CreatedDate = DateTime.Now;
        user.ModifiedDate = DateTime.Now;
        Create(user);
        await SaveAsync();
    }

    public async Task Edit(User user)
    {
        user.ActiveCode = Guid.NewGuid().ToString().Replace(
            "-",
            ""
        );
        user.ModifiedDate = DateTime.Now;
        Update(user);
        await SaveAsync();
    }

    public async Task EditPassword(
        User user,
        string password
    )
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

    public async Task<User> UniqueEmail(
        string email,
        int userId
    ) =>
        await FindByCondition(
            user =>
                user.Email == email &&
                user.Id != userId
        ).FirstOrDefaultAsync();

    public async Task<User> UniqueMobile(
        string mobile,
        int userId
    ) =>
        await FindByCondition(
            user =>
                user.Email == mobile &&
                user.Id != userId
        ).FirstOrDefaultAsync();

    public async Task<IEnumerable<User>> GetAllAdmin() =>
        await FindByCondition(user => user.RoleId == 1)
            .Include(x => x.Role)
            .ToListAsync();

    public async Task<User> IsExist(int id) =>
        await FindByCondition(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<IList<User>> GetContactPerson() =>
        await FindAll().Include(user => user.ContactPersons).ToListAsync();

    public async Task<string> Authenticate(
        string username,
        string password
    )
    {
        var user = await FindByCondition(
                user =>
                    user.Username == username
            )
            .Include(user => user.Role)
            .FirstOrDefaultAsync();

        // return null if user not found
        if (user == null)
            return null;

        //var md5 = new MD5CryptoServiceProvider();
        //var md5data = md5.ComputeHash(data);

        //var hashedPassword = Encoding.ASCII.GetBytes(user.Password);

        if (user.Password != password) return null;

        // authentication successful so generate jwt token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new(
                        ClaimTypes.Name,
                        user.Name
                    ),
                    new(
                        ClaimTypes.NameIdentifier,
                        user.Username
                    ),
                    new(
                        ClaimTypes.Role,
                        user.Role.Name
                    )
                }
            ),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        user.Token = tokenHandler.WriteToken(token);

        // remove password before returning
        user.Password = null;

        return user.Token;
    }

    public async Task<IEnumerable<User>> GetAll() =>
        await FindAll().ToListAsync();
}