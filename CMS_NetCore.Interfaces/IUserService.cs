using CMS_NetCore.DomainClasses;
using System;
using System.Collections.Generic;
using  CMS_NetCore.ViewModels;
using System.Threading.Tasks;

namespace CMS_NetCore.Interfaces
{
    public interface IUserService
    {
        Task<DataGridViewModel<User>> GetBySearch(int page, int pageSize, string srchString);
        Task<User> GetUserByIdentity(string mobile);
        Task<User> GetUserByPassword(int userId, string password);
        Task<User> GetById(int? id);
        Task Add(User user);
        Task Edit(User user);
        Task EditPassword(User user, string password);
        Task Remove(User user);
        Task<User> UniqueEmail(string email, int userId);
        Task<User> UniqueMobile(string mobile, int userId);
        Task<IEnumerable<User>> Users();
        Task<IEnumerable<User>> GetAllAdmin();
        Task<User> UserExistence(int id);
        Task<IList<User>> GetContactctPerson();
    }
}
