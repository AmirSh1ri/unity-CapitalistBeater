using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
    public float duration = 1;
    public AnimationCurve curve;
    public float resetSpeed = 5f;

    private bool isResetting = false;

    //handles shake effect using animation curve
    public IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);

            Vector3 shakeOffset = Random.insideUnitSphere * strength;
            transform.localPosition += new Vector3(shakeOffset.x, 0, shakeOffset.z);

            yield return null;
        }

        StartCoroutine(SmoothReset());
    }

    //smoothly resets object position after shake
    private IEnumerator SmoothReset()
    {
        if (isResetting) yield break;
        isResetting = true;

        Vector3 targetPosition = new Vector3(0, 0.5f, 0);

        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, resetSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localPosition = targetPosition;
        isResetting = false;
    }
}
