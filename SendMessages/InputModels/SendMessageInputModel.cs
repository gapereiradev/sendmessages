namespace SendMessages.InputModels
{
    public class SendMessageInputModel
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Content { get; set; }
        public string CreatedAt { get; set; }
    }
}
