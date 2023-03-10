using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Abstractions;

public class BaseDomainModel
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }

    [Timestamp]
    public uint Version { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public BaseDomainModel()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public BaseDomainModel(bool isEmpty)
    {
        Id = Guid.Empty;
        CreatedAt = DateTime.MinValue;
        UpdatedAt = DateTime.MinValue;
    }
}