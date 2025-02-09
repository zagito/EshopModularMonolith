﻿global using Carter;
global using FluentValidation;
global using Mapster;
global using MediatR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using Ordering.Data;
global using Ordering.Orders.Dtos;
global using Ordering.Orders.Events;
global using Ordering.Orders.Models;
global using Ordering.Orders.Models.ValueObjects;
global using Shared.Data;
global using Shared.DDD;
global using SharedContracts.CQRS;
global using SharedContracts.Results;
global using System.Reflection;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using static Ordering.Orders.Utils.OrdersConstants;
