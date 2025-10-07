namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart) 
    : ICommand<CreateBasketResult>;
public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator
    : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName)
            .NotEmpty().WithMessage("User name is required.");
        RuleFor(x => x.ShoppingCart.Items)
            .NotNull().WithMessage("Shopping cart items cannot be null.");

        RuleForEach(x => x.ShoppingCart.Items)
            .ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("Product ID is required.");
                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
                items.RuleFor(i => i.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than zero.");
                items.RuleFor(i => i.ProductName)
                    .NotEmpty().WithMessage("Product name is required.");
            });
    }
}

public class CreateBasketHandler(IBasketRepository repository)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingCart = ShoppingCart.Create(Guid.NewGuid(), command.ShoppingCart.UserName);

        foreach(var item in command.ShoppingCart.Items)
        {
            shoppingCart.AddItem(
                item.ProductId,
                item.Quantity,
                item.Color,
                item.Price,
                item.ProductName);
        }
        await repository.CreateBasket(shoppingCart, cancellationToken);
        return new CreateBasketResult(shoppingCart.Id);
    }
}
