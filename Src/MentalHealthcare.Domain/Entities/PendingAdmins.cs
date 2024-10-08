using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealthcare.Domain.Entities;

public class PendingAdmins
{
    public int PendingAdminsId { get; set; }

    [DataType(DataType.EmailAddress)]
    [MaxLength(100)]
    public string Email { get; set; } = default!;

    [ForeignKey(nameof(Entities.Admin))] public int AdminId { get; set; }
    public virtual Admin Admin { get; set; } = default!;
}