﻿using System.Reflection;
using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Application.ReportBehavior.DailyReport;
using FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;
using FinanceManagerAPI.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManagerAPI.Application;

public static class DependencyInjection
{
    // Register dependency that is primarily used in Application layer.
    public static void RegisterApplicationDependency(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IBaseBehavior<FinancialOperation>, FinancialOperationBehavior.FinancialOperationBehavior>();
        services.AddScoped<IBaseBehavior<OperationType>, OperationTypeBehavior.OperationTypeBehavior>();
        
        services.AddTransient<IRequestHandler<GetDailyReportQuery, DailyReportResponse>, GetDailyReportQueryHandler>();
        services.AddTransient<IRequestHandler<GetDatePeriodReportQuery,DatePeriodReportResponse>, GetDatePeriodReportQueryHandler>();
    }
}