// boosts player's jump height by a percentage when activated

using UnityEngine;

public class JumpBoostPAB : MonoBehaviour
{
    public float percent = 20f;

    // increases player's jump height by given percent
    void Start()
    {
        var move = FindObjectOfType<PlayerMovement>();
        if (move != null)
        {
            move.jumpHeight += move.jumpHeight * (percent / 100f);
        }
    }
}
