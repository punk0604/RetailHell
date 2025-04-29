public interface ShiftTask
{
    ShiftSystem.ShiftPhase TaskPhase { get; }   // Which phase this task belongs to
    bool IsTaskComplete();                      // Has it been completed?
}
