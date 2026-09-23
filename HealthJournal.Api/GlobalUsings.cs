global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;

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
global using Microsoft.Extensions.Logging;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.Http;
global using Serilog;

