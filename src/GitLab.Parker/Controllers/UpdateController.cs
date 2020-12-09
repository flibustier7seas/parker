using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GitLab.Parker.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly IBotUpdateHandler botUpdateHandler;

        public UpdateController(IBotUpdateHandler botUpdateHandler)
        {
            this.botUpdateHandler = botUpdateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            await botUpdateHandler.HandleAsync(update);
            return Ok();
        }
    }
}
