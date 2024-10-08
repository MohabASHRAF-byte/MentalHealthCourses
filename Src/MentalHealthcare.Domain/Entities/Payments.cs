namespace MentalHealthcare.Domain.Entities;
// written by marcellino , No review yet

public class Payments
{
    public int PaymentId { get; set; }
    public string Type { get; set; }
    public long Card_Number { get; set; }

    public DateTime month { get; set; }
    public DateOnly Year { get; set; }
    public string Name { get; set; }

    public User users { get; set; }
}