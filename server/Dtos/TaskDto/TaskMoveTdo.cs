public class MoveTaskDto
{
    public Guid TaskId { get; set; }
    public Guid NewBoardId { get; set; } // Coloana în care a ajuns
    public int NewOrder { get; set; }    // Poziția (0, 1, 2...)
}