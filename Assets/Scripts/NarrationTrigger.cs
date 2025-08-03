using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    private bool played = false;

    private void Update()
    {
        if (played)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!AudioManager.Instance.isNarrating)
            {
                played = AudioManager.Instance.playRandomNarration();
            }
        }
    }
}
