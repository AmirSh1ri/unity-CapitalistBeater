// handles weapon purchase when player is nearby and presses F

using UnityEngine;

public class ProximityInteract : MonoBehaviour
{
    public GameObject interactButtonUI;
    public Attack attackScript;
    public CurrencyHolder currencyHolder;

    private bool isPlayerInRange = false;
    private string weaponName;

    void Start()
    {
        weaponName = gameObject.name;
    }

    void Update()
    {
        // check for F key press when in range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TryPurchaseWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactButtonUI.SetActive(true);
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactButtonUI.SetActive(false);
            isPlayerInRange = false;
        }
    }

    private void TryPurchaseWeapon()
    {
        if (attackScript.weaponPriceDict.TryGetValue(weaponName, out int price))
        {
            if (currencyHolder.skulls >= price)
            {
                currencyHolder.RemoveSkulls(price);

                GameObject targetWeapon = FindWeaponByName(weaponName);
                if (targetWeapon != null)
                    attackScript.SetActiveWeapon(targetWeapon);

                attackScript.weaponPriceDict[weaponName] = 0;
                attackScript.UpdateWeaponPriceUI();
            }
        }
    }

    // finds the weapon GameObject by name
    private GameObject FindWeaponByName(string name)
    {
        foreach (GameObject weapon in attackScript.weapons)
        {
            if (weapon.name == name)
                return weapon;
        }
        return null;
    }
}
