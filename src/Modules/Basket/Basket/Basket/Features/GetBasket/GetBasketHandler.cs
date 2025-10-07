namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName)
    : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCartDto ShoppingCart);
public class GetBasketHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.UserName, true, cancellationToken);
        
        return new GetBasketResult( new ShoppingCartDto
        (
            Id: basket.Id,
            UserName: basket.UserName,
            Items: basket.Items.Select(item => new ShoppingCartItemDto
            (
                Id: item.Id,
                ShoppingCartId: item.ShoppingCartId,
                ProductId: item.ProductId,
                Quantity: item.Quantity,
                Color: item.Color,
                Price: item.Price,
                ProductName: item.ProductName
            )).ToList()
        ));
    }
}
