namespace BusinessObjects.Models;

public partial class Semester
{
    public Guid SemesterId { get; set; }

    public string SemeterName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
