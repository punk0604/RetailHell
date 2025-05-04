using UnityEngine;

public class StatsMenuController : MonoBehaviour
{
    public GameObject statsMenuPrefab;
    private static GameObject statsMenuInstance;

    void Awake()
    {
        if (statsMenuInstance == null)
        {
            statsMenuInstance = Instantiate(statsMenuPrefab);
            statsMenuInstance.SetActive(false);
            DontDestroyOnLoad(statsMenuInstance);
        }
        else
        {
            Destroy(gameObject); // prevent duplicate controller
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && statsMenuInstance != null)
        {
            StatsMenuUI menu = statsMenuInstance.GetComponent<StatsMenuUI>();
            if (menu != null)
            {
                menu.ToggleVisibility();
            }
        }
    }
}
