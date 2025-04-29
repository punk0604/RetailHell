using UnityEngine;

public class LightSwitch : MonoBehaviour, ShiftTask
{
    public GameObject lightsParent;
    public ShiftSystem shiftSystem;

    private bool lightsOn = false;
    private bool playerInRange = false;

    public ShiftSystem.ShiftPhase TaskPhase => shiftSystem.currentPhase;

    private void Start()
    {
        // Ensure lights start OFF
        SetLights(false);
        lightsOn = false;
    }

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(KeyCode.E)) return;

        if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Opening && !lightsOn)
        {
            lightsOn = true;
            SetLights(true);
            Debug.Log("✅ Lights turned ON.");
        }
        else if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing && lightsOn)
        {
            lightsOn = false;
            SetLights(false);
            Debug.Log("✅ Lights turned OFF.");
        }
    }

    private void SetLights(bool state)
    {
        if (lightsParent != null)
        {
            foreach (Light light in lightsParent.GetComponentsInChildren<Light>())
            {
                light.enabled = state;
            }
        }
    }

    public bool IsTaskComplete()
    {
        return shiftSystem.currentPhase switch
        {
            ShiftSystem.ShiftPhase.Opening => lightsOn,
            ShiftSystem.ShiftPhase.Closing => !lightsOn,
            _ => true
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}





