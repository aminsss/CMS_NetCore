using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IUserService
{
    Task<DataGridViewModel<User>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    );

    Task<User> GetUserByIdentity(string mobile);

    Task<User> GetUserByPassword(
        int userId,
        string password
    );

    Task<User> GetById(int? id);
    Task Add(User user);
    Task Edit(User user);

    Task EditPassword(
        User user,
        string password
    );

    Task Remove(User user);

    Task<User> UniqueEmail(
        string email,
        int userId
    );

    Task<User> UniqueMobile(
        string mobile,
        int userId
    );

    Task<string> Authenticate(
        string username,
        string password
    );

    Task<IEnumerable<User>> GetAll();
    Task<IEnumerable<User>> GetAllAdmin();
    Task<User> IsExist(int id);
    Task<IList<User>> GetContactPerson();
}