using MailSenderClient;
using Microsoft.AspNetCore.Mvc;

namespace MailSenderAPI.Controllers
{
    [ApiController]
    public class MailSenderController : ControllerBase
    {
        public MailSenderController(MailSenderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/api/mails")]
        public IActionResult SendMail([FromBody] MessageDto message)
        {
           _service.SendMail(message.Subject, message.Body, message.Recipients);
            return Ok();
        }

        [HttpGet]
        [Route("/api/mails")]
        public IActionResult GetAllMail()
        {
            return Ok(_service.GetAllMails().ToJson());
        }

        private readonly MailSenderService _service;
    }
}
