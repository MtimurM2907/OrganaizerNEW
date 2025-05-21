public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Priority { get; set; } = "Средний";
    public string? Category { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsNotified { get; set; }
    public string DueTime
    {
        get => DueDate.ToString("HH:mm");
        set
        {
            if (TimeSpan.TryParseExact(value, "hh\\:mm", null, out var time))
            {
                DueDate = DueDate.Date.Add(time);
            }
        }
    }
}