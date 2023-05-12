namespace Email.Shared
{
    public class EmailRequest
    {
        public List<string> To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public List<EmailAttachment> Attachments { get; set; } = new();
    }
}