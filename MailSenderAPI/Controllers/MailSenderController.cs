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
        public IActionResult SendMail([FromBody] MailDto mail)
        {
           _service.SendMail(mail.Subject, mail.Body, mail.Recipients);
            return Ok();
        }

        [HttpGet]
        [Route("/api/mails")]
        public IActionResult GetAllMails()
        {
            return Ok(_service.GetAllMails().ToJson());
        }

        private readonly MailSenderService _service;
    }
}
