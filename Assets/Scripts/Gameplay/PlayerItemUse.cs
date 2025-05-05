using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject energyDrinkEffect;
    public GameObject zenSodaEffect;
    public GameObject snackBarEffect;

    private bool noStressActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1 = Energy Drink
        {
            if (GameManager.Instance.energyDrinksOwned > 0)
            {
                GameManager.Instance.energyDrinksOwned--;
                playerMovement.ApplySpeedBoost(60f);
                Instantiate(energyDrinkEffect, transform.position, Quaternion.identity);
                Debug.Log("Used Energy Drink: +Speed for 1 minute");
            }
            else
            {
                Debug.Log("No Energy Drinks.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // 2 = Zen Soda
        {
            if (!noStressActive && GameManager.Instance.zenSodasOwned > 0)
            {
                GameManager.Instance.zenSodasOwned--;
                noStressActive = true;
                Instantiate(zenSodaEffect, transform.position, Quaternion.identity);
                TaskStressManager stressManager = FindObjectOfType<TaskStressManager>();
                if (stressManager != null) stressManager.DisableStressForShift();
                Debug.Log("Used Zen Soda: No stress for the rest of shift.");
            }
            else
            {
                Debug.Log("No Zen Sodas.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // 3 = Snack Bar
        {
            if (GameManager.Instance.snackBarsOwned > 0)
            {
                GameManager.Instance.snackBarsOwned--;
                TaskStressManager stressManager = FindObjectOfType<TaskStressManager>();
                Instantiate(snackBarEffect, transform.position, Quaternion.identity);
                if (stressManager != null) stressManager.IncreaseStressCap(5);
                Debug.Log("Used Snack Bar: +5 stress cap.");
            }
            else
            {
                Debug.Log("No Snack Bars.");
            }
        }
    }
}
