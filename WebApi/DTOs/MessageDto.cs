namespace WebApi.Interfaces
{
    public class MessageDto
    {
         public int Id { get; set; }
        public int SenderId { get; set; }

        public string SenderPhotoUrl { get; set; }
        public string SenderName { get; set; }
        public int RecipientId { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string RecipientName { get; set; }     

        public DateTime? SentDate { get; set; }
        public DateTime ReadDate { get; set; }
        public string Content { get; set; }
       
    }
}