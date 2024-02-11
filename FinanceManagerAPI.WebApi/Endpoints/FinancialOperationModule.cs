using FinanceManagerAPI.Application.FinancialOperationBehavior.Create;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Get;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Update;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Delete;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerAPI.WebApi.Endpoints;

public class FinancialOperationModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoint)
    {
        var mapGroup = endpoint.MapGroup("api/financial-operation");
        
        mapGroup.MapPost("", CreateFinancialOperation);
        mapGroup.MapGet("{id:int}", GetFinancialOperation);
        mapGroup.MapPut("{id:int}", UpdateFinancialOperation);
        mapGroup.MapDelete("{id:int}", DeleteFinancialOperation);
    }
    
    // [Post]
    public static async Task<IResult> CreateFinancialOperation(
        [FromBody] CreateFinancialOperationCommand command,
        IValidator<CreateFinancialOperationCommand> validator,
        ISender sender)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
            
        await sender.Send(command);

        return TypedResults.Ok();
    }
    
    // [Get]
    public static async Task<IResult> GetFinancialOperation(
        [FromRoute] int id,
        IValidator<GetFinancialOperationQuery> validator,
        ISender sender)
    {
        var query = new GetFinancialOperationQuery(id);
            
        var validationResult = await validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
            
        try
        {
            return TypedResults.Ok(await sender.Send(query));
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error 404: {ex.Message}");
        }
    }
    
    // [Put/Update]
    public static async Task<IResult> UpdateFinancialOperation(
        [FromRoute] int id,
        [FromBody] UpdateFinancialOperationCommand command,
        IValidator<UpdateFinancialOperationCommand> validator,
        ISender sender)
    {
        command.FinancialOperationDto.Id = id;

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        try
        {
            await sender.Send(command);
                
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error 404: {ex.Message}");
        }
    }
    
    // [Delete]
    public static async Task<IResult> DeleteFinancialOperation(
        [FromRoute] int id,
        [FromBody] DeleteFinancialOperationCommand command,
        IValidator<DeleteFinancialOperationCommand> validator,
        ISender sender)
    {
        command.FinancialOperationDto.Id = id;
            
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        try
        {
            await sender.Send(command);
                
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error 404: {ex.Message}");
        }
    }
}