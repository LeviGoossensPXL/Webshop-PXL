using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockItemController : ControllerBase
    {

        private readonly ILogger<StockItemController> _logger;

        public StockItemController(ILogger<StockItemController> logger)
        {
            _logger = logger;
        }
    }
}
