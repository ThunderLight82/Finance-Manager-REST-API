using FinanceManagerAPI.Application.OperationTypeBehavior.Create;
using FinanceManagerAPI.Application.OperationTypeBehavior.Delete;
using FinanceManagerAPI.Application.OperationTypeBehavior.Get;
using FinanceManagerAPI.Application.OperationTypeBehavior.Update;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerAPI.WebApi.Endpoints;

public sealed class OperationTypeModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoint)
    {
        var mapGroup = endpoint.MapGroup("api/operation-type");
        
        mapGroup.MapPost("", CreateOperationType);
        mapGroup.MapGet("{id:int}", GetOperationType);
        mapGroup.MapPut("{id:int}", UpdateOperationType);
        mapGroup.MapDelete("{id:int}", DeleteOperationType);
    }

    // [Post]
    public static async Task<IResult> CreateOperationType(
        [FromBody] CreateOperationTypeCommand command,
        IValidator<CreateOperationTypeCommand> validator,
        ISender sender)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
            
        await sender.Send(command);

        return Results.Ok();
    }

    // [Get]
    public static async Task<IResult> GetOperationType(
        [FromRoute] int id,
        IValidator<GetOperationTypeQuery> validator,
        ISender sender)
    {
        var query = new GetOperationTypeQuery(id);
            
        var validationResult = await validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
            
        try
        {
            return Results.Ok(await sender.Send(query));
        }
        catch (Exception ex)
        {
            return Results.NotFound($"Error 404: {ex.Message}");
        }
    }

    // [Put/Update]
    public static async Task<IResult> UpdateOperationType(
        [FromRoute] int id,
        [FromBody] UpdateOperationTypeCommand command,
        IValidator<UpdateOperationTypeCommand> validator,
        ISender sender)
    {
        command.OperationTypeDto.Id = id;

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        try
        {
            await sender.Send(command);
                
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.NotFound($"Error 404: {ex.Message}");
        }
    }
    
    // [Delete]
    public static async Task<IResult> DeleteOperationType(
        [FromRoute] int id,
        [FromBody] DeleteOperationTypeCommand command,
        IValidator<DeleteOperationTypeCommand> validator,
        ISender sender)
    {
        command.OperationTypeDto.Id = id;
            
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        try
        {
            await sender.Send(command);
                
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.NotFound($"Error 404: {ex.Message}");
        }
    }
}