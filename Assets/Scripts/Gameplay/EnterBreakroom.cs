using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBreakroom : MonoBehaviour
{
    private bool playerInRange = false;
    //public GameObject PauseCan;
    //public static FOVUI fovui;
    //public static EnterBreakroom instance;

    [Header("Scene Settings")]
    public string breakroomSceneName = "Breakroom";

    [Header("References")]
    public ShiftSystem shiftSystem;

    /*private void Awake() //make sure the sound can play across scenes
    {
        if (instance != null)
        {
            //Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/
    /*private void Start()
    {
        fovui = PauseCan.GetComponent<FOVUI>();
    }*/



    private void Update()
    {
        if (!playerInRange || shiftSystem == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Break)
            {
                Debug.Log("✅ Entering Breakroom...");
                SceneManager.LoadScene(breakroomSceneName);
                //fovui.Refresh();
            }
            else
            {
                Debug.Log("🚫 Cannot enter breakroom yet. Closing tasks not done.");
                //temp:
                //SceneManager.LoadScene(breakroomSceneName);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (shiftSystem != null && shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing && AreClosingTasksComplete())
            {
                InteractionPromptUI.Instance?.Show("Press E to enter breakroom");
            }
            else
            {
                InteractionPromptUI.Instance?.Show("Cannot enter breakroom yet!");
            }

            Debug.Log("Player entered breakroom door range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionPromptUI.Instance?.Hide();
            Debug.Log("Player left breakroom door range.");
        }
    }

    //double-check if closing tasks are done
    private bool AreClosingTasksComplete()
    {
        if (shiftSystem.closingTasks == null) return false;

        ShiftTask[] tasks = shiftSystem.closingTasks.GetComponentsInChildren<ShiftTask>();
        foreach (ShiftTask task in tasks)
        {
            if (!task.IsTaskComplete())
                return false;
        }

        return true;
    }
}
