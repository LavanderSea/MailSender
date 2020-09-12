using MailSenderClient.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MailSenderAPI
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = context.Exception is IncorrectFieldException exception
                ? new BadRequestObjectResult(exception.Message)
                : new BadRequestObjectResult("Unknown error");
        }
    }
}