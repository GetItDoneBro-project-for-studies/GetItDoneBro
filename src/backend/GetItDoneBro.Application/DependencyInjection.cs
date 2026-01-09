using System.Reflection;
using FluentValidation;
using GetItDoneBro.Application.UseCases.Projects.Commands.AssignUserToProject;
using GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;
using GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateUserRole;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectUsers;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.AssignUserToProject;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.RemoveUserFromProject;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.UpdateUserRole;
using GetItDoneBro.Application.UseCases.TaskColumns.Commands.CreateTaskColumn;
using GetItDoneBro.Application.UseCases.TaskColumns.Commands.UpdateTaskColumn;
using GetItDoneBro.Application.UseCases.TaskColumns.Commands.DeleteTaskColumn;
using GetItDoneBro.Application.UseCases.TaskColumns.Queries.GetTaskColumnsByProject;
using GetItDoneBro.Application.UseCases.Tasks.Commands.CreateTask;
using GetItDoneBro.Application.UseCases.Tasks.Commands.UpdateTask;
using GetItDoneBro.Application.UseCases.Tasks.Commands.DeleteTask;
using GetItDoneBro.Application.UseCases.Tasks.Commands.MoveTaskToColumn;
using GetItDoneBro.Application.UseCases.Tasks.Queries.GetTasksByProject;
using GetItDoneBro.Application.UseCases.Users.Commands.AddUser;
using GetItDoneBro.Application.UseCases.Users.Commands.DisableUser;
using GetItDoneBro.Application.UseCases.Users.Commands.ResetPasswordUser;
using GetItDoneBro.Application.UseCases.Users.Queries.GetUsers;
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
        services.AddScoped<IAddUserHandler, AddUserHandler>();
        services.AddScoped<IDisableUserHandler, DisableUserHandler>();
        services.AddScoped<IResetUserPasswordHandler, ResetUserPasswordHandler>();
        services.AddScoped<IGetUsersHandler, GetUsersHandler>();

        services.AddScoped<ICreateTaskColumnHandler, CreateTaskColumnHandler>();
        services.AddScoped<IUpdateTaskColumnHandler, UpdateTaskColumnHandler>();
        services.AddScoped<IDeleteTaskColumnHandler, DeleteTaskColumnHandler>();
        services.AddScoped<IGetTaskColumnsByProjectHandler, GetTaskColumnsByProjectHandler>();

        services.AddScoped<ICreateTaskHandler, CreateTaskHandler>();
        services.AddScoped<IUpdateTaskHandler, UpdateTaskHandler>();
        services.AddScoped<IDeleteTaskHandler, DeleteTaskHandler>();
        services.AddScoped<IMoveTaskToColumnHandler, MoveTaskToColumnHandler>();
        services.AddScoped<IGetTasksByProjectHandler, GetTasksByProjectHandler>();

        return services;
    }
}
