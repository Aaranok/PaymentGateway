using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.PublishedLanguage.Commands;
using System.Collections.Generic;

namespace PaymentGateway.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CreateAccount _createAccountCommandHandler;
        private readonly ListOfAccounts.QueryHandler _queryHandler;
        public AccountController(CreateAccount createAccountCommandHandler, ListOfAccounts.QueryHandler queryHandler)
        {
            _createAccountCommandHandler = createAccountCommandHandler;
            _queryHandler = queryHandler;
        }

        [HttpPost]
        [Route("Create")]
        public string CreateAccount(CreateAccountCommand command)
        {
            _createAccountCommandHandler.Handle(command, default).GetAwaiter().GetResult();

            return "OK";
        }
        [HttpGet]
        [Route("ListOfAccounts")]
        public List<ListOfAccounts.Model> GetListOfAccounts([FromQuery] ListOfAccounts.Query query)
        {
            var result = _queryHandler.Handle(query, default).GetAwaiter().GetResult();

            return result;
        }
    }
}