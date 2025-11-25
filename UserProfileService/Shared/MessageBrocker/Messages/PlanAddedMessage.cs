namespace UserProfileService.Shared.MessageBrocker.Messages
{
    public class PlanAddedMessage:BasicMessage
    {
        public IEnumerable<Guid> Userid { get; set; }

        public Guid planid { get; set; }
    }
}
