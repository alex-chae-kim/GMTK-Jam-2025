using UnityEngine;

public class PlatformCarrier : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shell"))
        {
            Debug.Log("Is Shell On platform " + gameObject.CompareTag("Shell"));
            collision.transform.SetParent(this.transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shell"))
        {
            collision.transform.SetParent(null);
        }
    }
}
