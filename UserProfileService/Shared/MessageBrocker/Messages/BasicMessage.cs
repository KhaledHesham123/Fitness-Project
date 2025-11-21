namespace UserProfileService.Shared.MessageBrocker.Messages
{
    public class BasicMessage
    {
        public string Type { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

    }
}
