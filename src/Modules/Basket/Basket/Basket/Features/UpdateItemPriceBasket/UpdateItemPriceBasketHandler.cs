namespace Basket.Basket.Features.UpdateItemPriceBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool Success);

public class UpdateItemPirceInBasketValidator
    : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPirceInBasketValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
    }
}

public class UpdateItemPriceBasketHandler(BasketDbContext context)
    : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        
        var items = await context.ShoppingCartItems
            .Where(i => i.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);
        if (items.Count == 0)
        {
            return new UpdateItemPriceInBasketResult(false);
        }
        foreach (var item in items)
        {
            item.UpdatePrice(command.Price);
        }
        await context.SaveChangesAsync(cancellationToken);
        return new UpdateItemPriceInBasketResult(true);


    }
}
