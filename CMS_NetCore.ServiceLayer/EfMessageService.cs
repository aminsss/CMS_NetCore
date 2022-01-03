using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.DataLayer;
using CMS_NetCore.ViewModels;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfMessageService : RepositoryBase<Message>,IMessageService
    {

        public EfMessageService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<Message>> GetBySearch(int page, int pageSize, string searchString,string identity)
        {
            var dataGridView = new DataGridViewModel<Message>
            {
                Records = await FindByCondition(x => (x.UsersFrom.moblie == identity)
                && (x.UsersFrom.moblie.Contains(searchString) || x.Subject.Contains(searchString)))
                .OrderBy(o => o.MessageId).Include(x=>x.UsersFrom)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(x => (x.UsersFrom.moblie == identity)
                && (x.UsersFrom.moblie.Contains(searchString) || x.Subject.Contains(searchString)))
                .OrderBy(o => o.MessageId).Include(x=>x.UsersFrom).CountAsync(),
            };

            return dataGridView;
        }
        public async Task Add(Message message)
        {
            message.AddedDate = DateTime.Now;
            message.ModifiedDate = DateTime.Now;
            message.ISRead = false;
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
            message.ISRead = true;
            message.ModifiedDate = DateTime.Now;
            Update(message);
            await SaveAsync();
        }

        public async Task<Message> GetById(int? id) =>
            await FindByCondition(x => x.MessageId.Equals(id)).FirstOrDefaultAsync();
    }
}
