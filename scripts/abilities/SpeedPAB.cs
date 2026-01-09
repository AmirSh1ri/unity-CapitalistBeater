//adds a speed boost to the player when the game starts

using UnityEngine;

public class SpeedPAB : MonoBehaviour
{
    public float percent = 10f; //percent to boost speed

    void Start()
    {
        var move = FindObjectOfType<PlayerMovement>();
        if (move != null)
        {
            move.speed += move.speed * (percent / 100f); 
        }
    }
}
