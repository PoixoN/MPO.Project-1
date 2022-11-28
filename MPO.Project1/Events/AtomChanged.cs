namespace MPO.Project1.Events;

public class AtomChanged : EventArgs
{
    public int from { get; set; }
    public int to { get; set; }
}