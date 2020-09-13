using System.Linq;
using MailSenderClient;
using Microsoft.AspNetCore.Mvc;

namespace MailSenderAPI.Controllers
{
    [ApiController]
    public class MailSenderController : ControllerBase
    {
        private readonly MailSenderService _service;

        public MailSenderController(MailSenderService service)
        {
            _service = service;
        }

        /// <summary>
        ///     Send message with given parameters
        /// </summary>
        [HttpPost]
        [Route("/api/mails")]
        public IActionResult SendMessage([FromBody] MessageDto message)
        {
            _service.SendMessage(message.Subject, message.Body, message.Recipients);
            return Ok();
        }

        /// <summary>
        ///     Get all messages from server
        /// </summary>
        [HttpGet]
        [Route("/api/mails")]
        public IActionResult GetAllMessages()
        {
            var messages = _service.GetAllMessages();

            if (messages != null && messages.Any())
                return Ok(messages.ToJson());
            return BadRequest("Messages did not found");
        }
    }
}