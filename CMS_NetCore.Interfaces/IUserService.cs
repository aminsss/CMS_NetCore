using CMS_NetCore.DomainClasses;
using System;
using System.Collections.Generic;
using  CMS_NetCore.ViewModels;
using System.Threading.Tasks;

namespace CMS_NetCore.Interfaces
{
    public interface IUserService
    {
        DataGridViewModel<User> GetBySearch(int page, int pageSize,string srchString);
        User GetUserByIdentity(string mobile);
        User GetUserByPassword(int userId, string password);
        User GetById(int? id);
        void Add(User user);
        void Edit(User user);
        void EditPassword(User user,string password);
        void Delete(User user);
        void Delete(int id);
        User UniqueEmail(string email, int userId);
        User UniqueMobile(string mobile, int userId);
        IEnumerable<User> Users();
        IEnumerable<User> GetAllAdmin();
    }
}
