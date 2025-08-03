using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public PulleyPlatform platform; // Assign in Inspector
    private int objectCount = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shell"))
        {
            objectCount++;
            platform.isActivated = true;
            Debug.Log(other.CompareTag("Shell"));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shell"))
        {
            objectCount--;
            if (objectCount <= 0)
                platform.isActivated = false;
        }
    }
}
