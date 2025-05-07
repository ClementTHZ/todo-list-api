public class Task
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool Is_Done { get; set; } = false;

    public DateTime Created_At { get; set; } = DateTime.Now;

    // public void Book(string title, bool is_done, DateTime created_at)
    // {
    //     Title = title;
    //     Is_Done = is_done;
    //     Created_At = created_at;
    // }
}