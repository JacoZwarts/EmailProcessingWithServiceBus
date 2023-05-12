namespace Email.Shared
{
    public class EmailRequestMessage
    {
        public List<string> To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; } = new();
    }
}