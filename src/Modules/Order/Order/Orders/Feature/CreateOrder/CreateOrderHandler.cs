using FluentValidation;
using Order.Data;
using Order.Orders.Dtos;
using Order.Orders.ValueObjects;
using Shared.Contracts.CQRS;

namespace Order.Orders.Feature.CreateOrder;

public record CreateOrderCommand(OrderDto Order)
    : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid OrderId);

public class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");
        RuleFor(x => x.Order.Items)
            .NotEmpty().WithMessage("At least one order item is required.");
        RuleForEach(x => x.Order.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
            items.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            items.RuleFor(i => i.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        });
    }
}

public class CreateOrderHandler(OrderDbContext context)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateOrderResult(order.Id);
    }

    private static Models.Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(
            orderDto.ShippingAddress.EmailAddress,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode);

        var billingAddress = Address.Of(
            orderDto.BillingAddress.EmailAddress,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode);

        var payment = Payment.Of(
            orderDto.Payment.CardName,
            orderDto.Payment.CardNumber,
            orderDto.Payment.Expiration,
            orderDto.Payment.Cvv,
            orderDto.Payment.PaymentMethod);

        var newOrder = Models.Order.Create(
            Guid.NewGuid(),
            orderDto.CustomerId,
            orderDto.OrderName,
            shippingAddress,
            billingAddress,
            payment);

        orderDto.Items.ForEach(item =>
        {
            newOrder.Add(item.ProductId, item.Quantity, item.Price);
        });

        return newOrder;
    }
}
