using MediatR;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{

    public class PurchaseService: IRequestHandler<Command>
    {
        private readonly Data.PaymentDbContext _dbContext;
        private readonly IMediator _mediator;

        public PurchaseService(IMediator mediator, Data.PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var account = _dbContext.Accounts.FirstOrDefault(account => account.IbanCode == request.Iban);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            decimal totalValue = 0;
            string currency = "";

            var transaction = new Transaction
            {
                Currency = account.Currency,
                DateOfTransaction = DateTime.UtcNow,
                DateOfOperation = DateTime.UtcNow,
                Type = "Purchase",
                AccountId = account.Id
            };
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            //int auxTransId = _dbContext.Transactions.Last().Id;
            foreach (var item in request.Details)
            {
                Product product = _dbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                if (product.Limit < item.NoPurchased)
                {
                    throw new Exception("Not enough items in storage");
                }
                totalValue += product.Value * item.NoPurchased;
                currency = product.Currency;
                product.Limit -= item.NoPurchased;

                if (account.Balance < totalValue)
                {
                    throw new Exception("Not enough capital");
                }
                decimal value = item.NoPurchased * product.Value;
                var pxt = new ProductXtransaction
                {
                    IdProduct = product.Id,
                    IdTransaction = transaction.Id,
                    NoPurchased = item.NoPurchased,
                    Value = value
                };
                product.Limit -= item.NoPurchased;

                _dbContext.ProductXtransactions.Add(pxt);
            }
            transaction.Amount = -totalValue;

            _dbContext.SaveChanges();
            return Unit.Task;
        }
        /*
public async Task<Unit> Handle(PurchaseServiceCommand request, CancellationToken cancellationToken)
{
   //var accountIdent = new AccountIbanOperations(_dbContext);
   var account = _dbContext.Accounts.FirstOrDefault(account=> account.IbanCode == request.Iban);

   if (account == null)
   {
       throw new Exception("Account not found");
   }
   decimal totalValue = 0;
   string currency = "";
   foreach (var item in request.Product)
   {
       var service = _dbContext.Services.FirstOrDefault(service => service.Id == item.IdService);

       if (service == null)
       {
           throw new Exception("Service not found");
       }
       if (service.Limit < item.NoPurchased)
       {
           throw new Exception("Not enough items in storage");

       }
       totalValue += service.Value;
       currency = service.Currency;
       service.Limit -= item.NoPurchased;
   }

   if (totalValue > account.Balance)
   {
       throw new Exception("Not enough capital");
   }

   Transaction transaction = new()
   {
       Amount = totalValue,
       Currency = currency,
       DateOfTransaction = request.DateOfTransaction,
       Value = totalValue,
       Type = "Purchase",
       AccountId = account.AccountID
   };
   transaction.DateOfOperation = transaction.GetOpDate();
   //transaction.Id = _dbContext.Transactions.Count() + 1;

   _dbContext.Transactions.Add(transaction);

   account.Balance -= totalValue;

   foreach (var item in request.Product)
   {
       ProductXTransaction servXTransItem = new()
       {
           IdTransaction = transaction.Id
       };
       servXTransItem.ServiceIdList.IdService = item.IdService;
       servXTransItem.ServiceIdList.NoPurchased = item.NoPurchased;
       _dbContext.ServiceXTransaction.Add(servXTransItem);
   }

   //_dbContext.SaveChanges();
   //ServicePurchased eventServPurchased = new(request.Iban, request.Cnp, request.PersonName, request.Product);
   //await _mediator.Publish(eventServPurchased, cancellationToken);
   //return Unit.Value;

}*/
    }
}
