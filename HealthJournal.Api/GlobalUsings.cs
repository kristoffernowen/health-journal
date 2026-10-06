global using HealthJournal.Api;
global using FluentValidation;

global using HealthJournal.Api.Domain.Journal;
global using HealthJournal.Api.Domain.Journal.Base;
global using HealthJournal.Api.Domain.Journal.ValueObjects;
global using HealthJournal.Api.Features.JournalEntries;
global using HealthJournal.Api.Features.JournalEntries.ActivityEntries;
global using HealthJournal.Api.Features.JournalWeeks;
global using HealthJournal.Api.Infrastructure.Data;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Serilog;

global using HealthJournal.Api.Domain.Exceptions;
global using Serilog.Events;
global using HealthJournal.Api.Features.JournalEntries.Service;
global using Auth0.AspNetCore.Authentication.Api;
global using HealthJournal.Api.DiagnosticFeature;
