using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessagedThread(int currentUserId,int recipientId);
        Task <bool> SaveAllSync();
    }
}