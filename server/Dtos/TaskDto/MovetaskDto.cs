namespace server.Dtos.TaskDto
{
    public class MoveTaskDto
    {
        public Guid NewColumnId { get; set; }
        public int NewOrder { get; set; }
        public string NewStatus { get; set; } = string.Empty;
    }
}