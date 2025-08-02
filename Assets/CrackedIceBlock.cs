using UnityEngine;
using System.Collections;

public class CrackedIceBlock : MonoBehaviour
{
    public float breakDelay = 0.1f;  // Optional: delay before breaking
    public GameObject shatterEffect; // Optional: particle prefab
    public AudioClip shatterSound;   // Optional: sound
    public AudioSource audioSource;  // Optional: assign via inspector

    private bool isBreaking = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreaking) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isBreaking = true;
            StartCoroutine(BreakAfterDelay());
        }
    }

    private IEnumerator BreakAfterDelay()
    {
        yield return new WaitForSeconds(breakDelay);

        if (shatterSound != null && audioSource != null)
            audioSource.PlayOneShot(shatterSound);

        if (shatterEffect != null)
            Instantiate(shatterEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
