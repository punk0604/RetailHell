public interface ShiftTask
{
    ShiftSystem.ShiftPhase TaskPhase { get; }
    bool IsTaskComplete();
    void ResetTask();
}


