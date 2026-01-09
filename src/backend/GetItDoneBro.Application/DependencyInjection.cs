using System.Reflection;
using FluentValidation;
using GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;
using GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.AssignUserToProject;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.RemoveUserFromProject;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.UpdateUserRole;
using GetItDoneBro.Application.UseCases.ProjectUsers.Queries.GetProjectUsers;
using GetItDoneBro.Application.UseCases.ProjectUsers.Queries.GetUserProjects;
using Microsoft.Extensions.DependencyInjection;

namespace GetItDoneBro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ICreateProjectHandler, CreateProjectHandler>();
        services.AddScoped<IUpdateProjectHandler, UpdateProjectHandler>();
        services.AddScoped<IDeleteProjectHandler, DeleteProjectHandler>();

        services.AddScoped<IGetProjectByIdHandler, GetProjectByIdHandler>();
        services.AddScoped<IGetAllProjectsHandler, GetAllProjectsHandler>();

        services.AddScoped<IAssignUserToProjectHandler, AssignUserToProjectHandler>();
        services.AddScoped<IRemoveUserFromProjectHandler, RemoveUserFromProjectHandler>();
        services.AddScoped<IUpdateUserRoleHandler, UpdateUserRoleHandler>();
        services.AddScoped<IGetProjectUsersHandler, GetProjectUsersHandler>();
        services.AddScoped<IGetUserProjectsHandler, GetUserProjectsHandler>();

        return services;
    }
}
