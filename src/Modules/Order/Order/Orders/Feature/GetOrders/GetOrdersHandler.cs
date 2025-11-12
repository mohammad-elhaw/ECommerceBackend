using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Orders.Dtos;
using Shared.Contracts.CQRS;
using Shared.Pagination;

namespace Order.Orders.Feature.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

internal class GetOrdersHandler(OrderDbContext context)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {

        int pageNumber = query.PaginationRequest.PageNumber < 1 ? 1 
            : query.PaginationRequest.PageNumber;
        int pageSize = query.PaginationRequest.PageSize < 1 ? 10
            : query.PaginationRequest.PageSize;

        long totalRecords = await context.Orders.LongCountAsync(cancellationToken);
        long totalPages = (long)Math.Ceiling(totalRecords / (double)pageSize);

        var orders = await context.Orders
            .Include(o => o.OrderItems).AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(o => o.OrderName)
            .Select(o => new OrderDto(
                o.Id,
                o.CustomerId,
                o.OrderName,
                new AddressDto(
                    o.ShippingAddress.EmailAddress,
                    o.ShippingAddress.AddressLine,
                    o.ShippingAddress.Country,
                    o.ShippingAddress.State,
                    o.ShippingAddress.ZipCode),
                new AddressDto(
                    o.BillingAddress.EmailAddress,
                    o.BillingAddress.AddressLine,
                    o.ShippingAddress.Country,
                    o.ShippingAddress.State,
                    o.ShippingAddress.ZipCode),
                new PaymentDto(
                    o.Payment.CardName,
                    o.Payment.CardNumber,
                    o.Payment.Expiration,
                    o.Payment.PaymentMethod,
                    o.Payment.CVV),
                o.OrderItems.Select(i => new OrderItemDto(
                    i.OrderId,
                    i.ProductId,
                    i.Quantity,
                    i.Price)).ToList()
                )).ToListAsync();

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                pageNumber,
                pageSize,
                totalRecords,
                totalPages,
                orders)
            );
    }
}