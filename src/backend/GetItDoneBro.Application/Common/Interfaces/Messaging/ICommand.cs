using MediatR;

namespace GetItDoneBro.Application.Common.Interfaces.Messaging;

public interface ICommand : IRequest, ICommandBase
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
{
}
