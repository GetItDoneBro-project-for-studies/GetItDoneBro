using Microsoft.AspNetCore.Builder;

namespace GetItDoneBro.Application.Common.Interfaces;

public interface IApiEndpoint
{
    void MapEndpoint(WebApplication app);
}
