//moves shopkeeper/env/ship away and back with L/K keys

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopMoveAway : MonoBehaviour
{
    public Transform Ship;
    public Transform ShopKeeper;
    public Transform ENV;
    public List<GameObject> purchaseables;

    public float moveSpeed = 1f;
    public float moveDuration = 10f;
    public float envMoveDuration = 10f;

    private bool hasStartedMoving = false;
    private bool shipMoving = false;
    private bool reverseSequenceStarted = false;

    //check for input to start or reverse movement
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !hasStartedMoving)
        {
            hasStartedMoving = true;
            StartCoroutine(MoveSequence());
        }

        if (Input.GetKeyDown(KeyCode.K) && !reverseSequenceStarted)
        {
            reverseSequenceStarted = true;
            StartCoroutine(ReverseSequence());
        }
    }

    //moves shopkeeper back, hides items, moves env down
    public IEnumerator MoveSequence()
    {
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            if (ShopKeeper != null)
                ShopKeeper.position += new Vector3(-moveSpeed * Time.deltaTime * 30, 0f, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (GameObject obj in purchaseables)
            if (obj != null) obj.SetActive(false);

        shipMoving = true;

        elapsedTime = 0f;
        while (elapsedTime < envMoveDuration)
        {
            if (ENV != null)
                ENV.position += new Vector3(0f, -moveSpeed * Time.deltaTime, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //moves ship forward if triggered
    void FixedUpdate()
    {
        if (shipMoving && Ship != null && Ship.position.z > -1020)
            Ship.position += new Vector3(0, 0, -moveSpeed * Time.deltaTime * 30);
        else
            shipMoving = false;
    }

    //moves env up, resets ship, moves shopkeeper right, shows items
    public IEnumerator ReverseSequence()
    {
        float elapsedTime = 0f;
        while (ENV != null && ENV.position.y < 19.5)
        {
                ENV.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
            yield return null;
        }

        if (Ship != null)
        {
            Ship.gameObject.SetActive(false);
            Ship.position = new Vector3(Ship.position.x, Ship.position.y, 1035);
            Ship.gameObject.SetActive(true);
        }

        while (Ship != null && Ship.position.z > 155)
        {
            Ship.position += new Vector3(0, 0, -moveSpeed * Time.deltaTime * 30);
            yield return null;
        }

        elapsedTime = 0f;
        while (ShopKeeper != null && ShopKeeper.position.x < -189)
        {
            ShopKeeper.position += new Vector3(moveSpeed * Time.deltaTime * 30, 0f, 0f);
            yield return null;
        }

        foreach (GameObject obj in purchaseables)
            if (obj != null) obj.SetActive(true);

        reverseSequenceStarted = false;
    }
}
