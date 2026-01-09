//tracks and displays skull currency

using UnityEngine;
using TMPro;

public class CurrencyHolder : MonoBehaviour
{
    public int skulls = 0;
    public TextMeshProUGUI skullsText;

    private void Start()
    {
        UpdateSkullsUI(); //show initial amount
    }

    public void AddSkulls(int amount)
    {
        if (amount > 0)
        {
            skulls += amount;
            Debug.Log("Added " + amount + " skulls. Total: " + skulls);
            UpdateSkullsUI();
        }
    }

    public bool RemoveSkulls(int amount)
    {
        if (amount > 0 && skulls >= amount)
        {
            skulls -= amount;
            Debug.Log("Spent " + amount + " skulls. Remaining: " + skulls);
            UpdateSkullsUI();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough skulls! Current: " + skulls);
            return false;
        }
    }

    public int GetSkulls()
    {
        return skulls;
    }

    private void UpdateSkullsUI()
    {
        if (skullsText != null)
        {
            skullsText.text = "" + skulls; //update text
        }
    }
}
