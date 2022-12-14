namespace WebApi.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public AppUser Sender { get; set; }
        public AppUser Recipient { get; set; }

        public DateTime? SentDate { get; set; }=DateTime.Now;
        public DateTime ReadDate { get; set; }
        public string Content { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }        
    }
}