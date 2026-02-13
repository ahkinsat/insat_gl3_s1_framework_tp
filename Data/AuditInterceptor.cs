using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TP.Models;

namespace TP.Data;

public class AuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        AuditChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AuditChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AuditChanges(DbContext? context)
    {
        if (context == null) return;

        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // Skip AuditLog itself to avoid infinite loop
            if (entry.Entity is AuditLog) continue;

            var entityType = entry.Entity.GetType();
            var tableName = entityType.Name;
            var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "0";

            // Only log Added, Modified, Deleted
            if (entry.State == EntityState.Added || 
                entry.State == EntityState.Modified || 
                entry.State == EntityState.Deleted)
            {
                var auditLog = new AuditLog
                {
                    TableName = tableName,
                    Action = entry.State.ToString(),
                    EntityKey = primaryKey,
                    Date = DateTime.UtcNow
                };

                // For Modified, track which properties changed
                if (entry.State == EntityState.Modified)
                {
                    var changes = new List<string>();
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified && !prop.Metadata.IsPrimaryKey())
                        {
                            changes.Add($"{prop.Metadata.Name}: '{prop.OriginalValue}' -> '{prop.CurrentValue}'");
                        }
                    }
                    auditLog.Changes = string.Join("; ", changes);
                }

                auditLogs.Add(auditLog);
            }
        }

        if (auditLogs.Any())
        {
            context.Set<AuditLog>().AddRange(auditLogs);
        }
    }
}