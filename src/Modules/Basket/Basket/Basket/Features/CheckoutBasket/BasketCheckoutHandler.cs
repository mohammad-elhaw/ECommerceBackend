using Shared.Messaging.Events;
using System.Text.Json;

namespace Basket.Basket.Features.CheckoutBasket;

public record BasketCheckoutCommand(BasketCheckoutDto BasketCheckout)
    : ICommand<BasketCheckoutResult>;

public record BasketCheckoutResult(bool IsSuccess);

public class BasketCheckoutValidator
    : AbstractValidator<BasketCheckoutCommand>
{
    public BasketCheckoutValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull()
            .WithMessage("Bakset Checkout can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty()
            .WithMessage("UserName is required");
        RuleFor(x => x.BasketCheckout.CustomerId)
            .NotEmpty().WithMessage("CustomerId is reuqired");
        RuleFor(x => x.BasketCheckout.TotalAmount)
            .GreaterThan(0).WithMessage("TotalAmount should be greater than zero");
        RuleFor(x => x.BasketCheckout.EmailAddress)
            .NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(x => x.BasketCheckout.AddressLine)
            .NotEmpty().WithMessage("AddressLine is required");
        RuleFor(x => x.BasketCheckout.Country)
            .NotEmpty().WithMessage("Country is required");
        RuleFor(x => x.BasketCheckout.State)
            .NotEmpty().WithMessage("State is required");
        RuleFor(x => x.BasketCheckout.ZipCode)
            .NotEmpty().WithMessage("ZipCode is required");
        RuleFor(x => x.BasketCheckout.CardName)
            .NotEmpty().WithMessage("CardName is required");
        RuleFor(x => x.BasketCheckout.CardNumber)
            .NotEmpty().WithMessage("CardNumber is required");
        RuleFor(x => x.BasketCheckout.Expiration)
            .NotEmpty().WithMessage("Expiration is required");
        RuleFor(x => x.BasketCheckout.Cvv)
            .NotEmpty().WithMessage("Cvv is required");
    }
}

public class BasketCheckoutHandler(BasketDbContext context)
    : ICommandHandler<BasketCheckoutCommand, BasketCheckoutResult>
{
    public async Task<BasketCheckoutResult> Handle(BasketCheckoutCommand command, CancellationToken cancellationToken)
    {

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var basket = await context.ShoppingCarts
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserName == command.BasketCheckout.UserName, cancellationToken);

        try
        {
            if(basket is null)
                throw new BasketNotFoundException(command.BasketCheckout.UserName);

            var eventMessage = new BasketCheckoutIntegrationEvent
            {
                UserName = command.BasketCheckout.UserName,
                CustomerId = command.BasketCheckout.CustomerId,
                TotalAmount = basket.TotalPrice,
                EmailAddress = command.BasketCheckout.EmailAddress,
                AddressLine = command.BasketCheckout.AddressLine,
                Country = command.BasketCheckout.Country,
                State = command.BasketCheckout.State,
                ZipCode = command.BasketCheckout.ZipCode,
                CardName = command.BasketCheckout.CardName,
                CardNumber = command.BasketCheckout.CardNumber,
                Expiration = command.BasketCheckout.Expiration,
                PaymentMethod = command.BasketCheckout.PaymentMethod,
                Cvv = command.BasketCheckout.Cvv,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage)
            };

            context.OutboxMessages.Add(outboxMessage);
            context.ShoppingCarts.Remove(basket);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new BasketCheckoutResult(true);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return new BasketCheckoutResult(false);
        }
    }
}
