using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.WebAPI.Controlers
{
    //http://localhost:5000/api/Home/GetHello
    [Route("api/[controller]")]
    [ApiController]
    public class HomeControler : ControllerBase
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
