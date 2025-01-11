﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? context)
    {
        if (context == null) 
            return;

        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Added) 
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = "System";
            }

            if (entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModified = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = "System";
            }
        }
    }
}

internal static class Extensions
{
    internal static bool HasChangedOwnedEntities(this EntityEntry<IEntity> entry) =>
        entry.References.Any(r => r.TargetEntry != null
            && r.TargetEntry.Metadata.IsOwned()
            && (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
