using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.WebAPI.Controllers
{
    //http://localhost:5000/api/HomeController/GetHello
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("GetHello")]
        public string GetMessage()
        {
            return "Hello World again";
        }

        [HttpGet]
        [Route("Index")]
        public string GetIndex()
        {
            return "Index";
        }

    }
}
