namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(
    string UserName, 
    Guid ProductId) 
    : ICommand<RemoveItemFromBasketResult>;
public record RemoveItemFromBasketResult(Guid BasketId);

public class RemoveItemFromBasketCommandValidator
    : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.");
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");
    }
}

public class RemoveItemFromBasketHandler(IBasketRepository repository)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, 
        CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);

        var shoppingCartItem = shoppingCart.Items
            .SingleOrDefault(item => item.Id == command.ProductId)
            ?? throw new BasketItemNotFoundException(command.ProductId);

        shoppingCart.RemoveItem(shoppingCartItem.ProductId);
        await repository.SaveChanges(cancellationToken);

        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}
