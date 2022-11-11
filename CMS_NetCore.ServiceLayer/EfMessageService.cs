using System;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfMessageService : RepositoryBase<Message>, IMessageService
{
    public EfMessageService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<Message>> GetBySearch(
        int page,
        int pageSize,
        string searchString,
        string identity
    )
    {
        return new DataGridViewModel<Message>
        {
            Records = await FindByCondition(
                    message => message.UserFrom.Mobile == identity &&
                               (message.UserFrom.Mobile.Contains(searchString) ||
                                message.Subject.Contains(searchString)
                               )
                )
                .OrderBy(message => message.Id)
                .Include(message => message.UserFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),

            TotalCount = await FindByCondition(
                    message => message.UserFrom.Mobile == identity &&
                               (message.UserFrom.Mobile.Contains(searchString) ||
                                message.Subject.Contains(searchString))
                )
                .OrderBy(message => message.Id)
                .Include(message => message.UserFrom)
                .CountAsync(),
        };
    }

    public async Task Add(Message message)
    {
        message.CreatedDate = DateTime.Now;
        message.IsRead = false;
        Create(message);
        await SaveAsync();
    }

    public async Task Remove(Message message)
    {
        Delete(message);
        await SaveAsync();
    }


    public async Task Edit(Message message)
    {
        message.IsRead = true;
        message.ModifiedDate = DateTime.Now;
        Update(message);
        await SaveAsync();
    }

    public async Task<Message> GetById(int? id) =>
        await FindByCondition(message => message.Id.Equals(id)).FirstOrDefaultAsync();
}