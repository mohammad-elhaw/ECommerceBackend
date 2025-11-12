using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Orders.Dtos;
using Shared.Contracts.CQRS;

namespace Order.Orders.Feature.GetOrderById;

public record GetOrderByIdQuery(Guid Id)
    : IQuery<GetOrderByIdResult>;

public record GetOrderByIdResult(OrderDto Order);

public class GetOrderByIdValidator 
    : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage("Order Id is Required");
    }
}

public class GetOrderByIdHandler(OrderDbContext context)
    : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .Where(o => o.Id == query.Id)
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Select(o => new OrderDto
            (
                o.Id,
                o.CustomerId,
                o.OrderName,

                new AddressDto
                (
                    o.ShippingAddress.EmailAddress,
                    o.ShippingAddress.AddressLine,
                    o.ShippingAddress.Country,
                    o.ShippingAddress.State,
                    o.ShippingAddress.ZipCode
                ),
                new AddressDto
                (
                    o.BillingAddress.AddressLine,
                    o.BillingAddress.EmailAddress,
                    o.BillingAddress.Country,
                    o.BillingAddress.State,
                    o.BillingAddress.ZipCode
                ),
                new PaymentDto
                (
                    o.Payment.CardName,
                    o.Payment.CardNumber,
                    o.Payment.Expiration,
                    o.Payment.PaymentMethod,
                    o.Payment.CVV
                ),
                o.OrderItems.Select(i => new OrderItemDto
                (
                    i.Id,
                    i.ProductId,
                    i.Quantity,
                    i.Price
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return order is null 
            ? throw new KeyNotFoundException($"Order with Id {query.Id} not found.") 
            : new GetOrderByIdResult(order);
    }
}