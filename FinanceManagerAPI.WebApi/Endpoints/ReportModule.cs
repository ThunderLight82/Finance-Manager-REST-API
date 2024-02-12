using Carter;
using FinanceManagerAPI.Application.ReportBehavior.DailyReport;
using FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerAPI.WebApi.Endpoints;

public class ReportModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoint)
    {
        var mapGroup = endpoint.MapGroup("api/report");

        mapGroup.MapGet("daily/{date}", GetDailyReport).CacheOutput();
        mapGroup.MapGet("date-period/{startDate}/{endDate}", GetDatePeriodReport).CacheOutput();
    }
    
    // [Get]
    public static async Task<IResult> GetDailyReport(
        [FromRoute] DateTime date,
        IValidator<GetDailyReportQuery> validator,
        ISender sender)
    {
        var query = new GetDailyReportQuery(date);
            
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
    
    // [Get]
    public static async Task<IResult> GetDatePeriodReport(
        [FromRoute] DateTime startDate,
        [FromRoute] DateTime endDate,
        IValidator<GetDatePeriodReportQuery> validator,
        ISender sender)
    {
        var query = new GetDatePeriodReportQuery(startDate, endDate);
            
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
}