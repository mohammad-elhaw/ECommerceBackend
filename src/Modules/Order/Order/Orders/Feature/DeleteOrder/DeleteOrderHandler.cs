using FluentValidation;
using Order.Data;
using Order.Orders.Exceptions;
using Shared.Contracts.CQRS;

namespace Order.Orders.Feature.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId)
    : ICommand<DeleteOrderResult>;

public record DeleteOrderResult(bool IsDeleted);

public class DeleteOrderCommandValidator
    : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");
    }
}

public class DeleteOrderHandler(OrderDbContext context)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await context.Orders.FindAsync(command.OrderId, cancellationToken);
        if(order is null)
            throw new OrderNotFoundException(command.OrderId);

        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResult(true);
    }
}
