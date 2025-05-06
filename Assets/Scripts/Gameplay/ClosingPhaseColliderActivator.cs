using UnityEngine;

public class ClosingPhaseColliderActivator : MonoBehaviour
{
    [Header("References")]
    public ShiftSystem shiftSystem;             // Assign in inspector
    public Collider targetCollider;             // The collider to enable/disable

    void Start()
    {
        if (targetCollider == null)
            targetCollider = GetComponent<Collider>();

        if (targetCollider != null)
            targetCollider.enabled = false; // Initially disabled
    }

    void Update()
    {
        if (shiftSystem != null && targetCollider != null)
        {
            targetCollider.enabled = shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing;
        }
    }
}