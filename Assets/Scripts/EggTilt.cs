using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EggTilt : MonoBehaviour, IPointerEnterHandler
{
    public RectTransform eggImage;
    public float tiltAngle = 15f;
    public float tiltSpeed = 0.1f;

    private bool isTilting = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isTilting && eggImage != null)
        {
            StartCoroutine(TiltOnce());
        }
    }

    private System.Collections.IEnumerator TiltOnce()
    {
        isTilting = true;

        Quaternion originalRotation = eggImage.localRotation;
        Quaternion leftTilt = Quaternion.Euler(0, 0, tiltAngle);
        Quaternion rightTilt = Quaternion.Euler(0, 0, -tiltAngle);

        yield return RotateTo(eggImage, leftTilt, tiltSpeed);
        yield return RotateTo(eggImage, rightTilt, tiltSpeed);
        yield return RotateTo(eggImage, originalRotation, tiltSpeed);

        isTilting = false;
    }

    private System.Collections.IEnumerator RotateTo(RectTransform target, Quaternion toRotation, float duration)
    {
        Debug.Log($"Rotating {target.name} to {toRotation.eulerAngles} over {duration} seconds");
        Quaternion fromRotation = target.localRotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.localRotation = Quaternion.Slerp(fromRotation, toRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localRotation = toRotation;
    }
}
