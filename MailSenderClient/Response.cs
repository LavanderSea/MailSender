namespace MailSenderClient
{
    public class Response
    {
        public Response(string result, string failedMessage)
        {
            Result = result;
            FailedMessage = failedMessage;
        }

        public string Result { get; }
        public string FailedMessage { get; }
    }
}