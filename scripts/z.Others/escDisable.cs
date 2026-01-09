//disables escape menu logic on start
// used for other menus and dialogues

using UnityEngine;

public class escDisable : MonoBehaviour
{
    [SerializeField] private GameObject ESCbtn;

    void Start()
    {
        //hide escape UI
        ESCbtn.SetActive(false);
    }
}
