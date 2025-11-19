namespace UserProfileService.Shared.MessageBrocker.Messages
{
    public class PlanAddedMessage:BasicMessage
    {
        public Guid Userid { get; set; }

        public Guid planid { get; set; }
    }
}
