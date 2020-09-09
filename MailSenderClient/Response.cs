using System;
using System.Collections.Generic;
using System.Text;

namespace MailSenderClient
{
    public class Response
    {
        public string Result { get;}
        public string FailedMessage { get;}

        public Response(string result, string failedMessage)
        {
            Result = result;
            FailedMessage = failedMessage;
        }
    }
}
