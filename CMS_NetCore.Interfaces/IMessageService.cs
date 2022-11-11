using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IMessageService
{
    Task<DataGridViewModel<Message>> GetBySearch(
        int page,
        int pageSize,
        string searchString,
        string identity
    );

    Task<Message> GetById(int? id);
    Task Add(Message message);
    Task Edit(Message message);
    Task Remove(Message message);
}