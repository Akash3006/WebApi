using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;
using  WebApi.Interfaces;
namespace WebApi.Controllers
{
    public class MessagesController:BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository,IMessageRepository messageRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageDto){

            var username = User.GetUserName();

            var sender = await _userRepository.GetUserByNameAsync(username);
            var reciever =await _userRepository.GetUserByNameAsync(messageDto.RecipientUsername);

            if(reciever == null) return NotFound();
            var message = new Message{

                Sender =sender,
                Recipient =reciever,
                SenderName = sender.UserName,
                RecipientName = reciever.UserName,
                Content = messageDto.Content
            };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllSync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Problem while sending message");

        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams){

                messageParams.Username = User.GetUserName();

                var messages = await _messageRepository.GetMessagesForUser(messageParams);

                Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage,messages.PageSize,messages.TotalCount,messages.TotalPages));

                return messages;

        }
    }
}