namespace StoreFlow.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string? MessageTittle { get; set; }
        public string? MessageDetails { get; set; }
        public string? SenderNameSurname { get; set; }
        public string? SenderImageUrl { get; set; }
        public DateTime Datetime { get; set; }
        public bool IsRead { get; set; }
    }
}
