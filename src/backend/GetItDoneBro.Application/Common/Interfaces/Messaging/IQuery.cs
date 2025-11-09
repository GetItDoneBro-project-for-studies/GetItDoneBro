using MediatR;

namespace GetItDoneBro.Application.Common.Interfaces.Messaging;


public interface IQuery<out TResponse> : IRequest<TResponse>, ICommandBase;
