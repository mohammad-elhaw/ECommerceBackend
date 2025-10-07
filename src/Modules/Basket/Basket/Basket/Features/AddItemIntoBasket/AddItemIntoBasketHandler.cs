namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand
    (string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator
    : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.");
        RuleFor(x => x.ShoppingCartItem)
            .NotNull().WithMessage("Shopping cart item cannot be null.");
        When(x => x.ShoppingCartItem is not null, () =>
        {
            RuleFor(x => x.ShoppingCartItem.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.ShoppingCartItem.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.ShoppingCartItem.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.ShoppingCartItem.ProductName)
                .NotEmpty().WithMessage("Product name is required.");
        });
    }
}

public class AddItemIntoBasketHandler(IBasketRepository repository)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await 
            repository.GetBasket(command.UserName, false, cancellationToken);

        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            command.ShoppingCartItem.Price,
            command.ShoppingCartItem.ProductName);

        await repository.SaveChanges(cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}
