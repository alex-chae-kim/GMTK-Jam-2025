using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EggTilt : MonoBehaviour, IPointerEnterHandler
{
    public Image eggImage;
    public float tiltAngle = 20f;
    public float tiltDelay = 0.2f;
    private bool isTilting = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isTilting)
        {
            Debug.Log("Already tilting — skipping");
            return;
        }

        Debug.Log("Starting tilt");
        StartCoroutine(TiltEgg());
    }

    private IEnumerator TiltEgg()
    {
        isTilting = true;

        Quaternion startRot = eggImage.rectTransform.localRotation;
        Quaternion leftTilt = Quaternion.Euler(0, 0, -tiltAngle);
        Quaternion rightTilt = Quaternion.Euler(0, 0, tiltAngle);

        eggImage.rectTransform.localRotation = leftTilt;
        Debug.Log("Egg tilted left");
        yield return new WaitForSecondsRealtime(tiltDelay);

        eggImage.rectTransform.localRotation = rightTilt;
        Debug.Log("Egg tilted right");
        yield return new WaitForSecondsRealtime(tiltDelay);

        eggImage.rectTransform.localRotation = startRot;
        Debug.Log("Egg tilted back to original position");
        isTilting = false;
    }
}