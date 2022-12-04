using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(ApplicationDataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public Task<IEnumerable<MessageDto>> GetMessagedThread(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
           var query = _context.Messages.OrderByDescending(x=>x.SentDate).AsQueryable();

           query = messageParams.Container switch{
            "Inbox" => query.Where(u=>u.RecipientName == messageParams.Username),
            "Outbox" => query.Where(u=>u.SenderName == messageParams.Username),
            _=> query.Where(u=>u.RecipientName == messageParams.Username && u.SentDate == null)
           };

           var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

           return await PagedList<MessageDto>.CreatePageAsync(messages,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<bool> SaveAllSync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}