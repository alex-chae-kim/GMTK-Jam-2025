using UnityEngine;
using System.Collections;

public class CrackedIceBlock : MonoBehaviour
{
    public float breakDelay = 0.1f;  // Optional: delay before breaking
    public float respawnTimer = 5f; // Optional: time before respawning the block
    public GameObject shatterEffect; // Optional: particle prefab
    public AudioClip shatterSound;   // Optional: sound
    public AudioSource audioSource;  // Optional: assign via inspector
    public SpriteRenderer spriteRenderer; // Optional: assign via inspector
    public BoxCollider2D boxCollider; // Optional: assign via inspector
    public GameObject light2D; // Optional: assign via inspector

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

        spriteRenderer.enabled = false; // Hide the sprite
        boxCollider.enabled = false; // Disable the collider
        light2D.SetActive(false); // Disable the light
        yield return new WaitForSeconds(respawnTimer);
        spriteRenderer.enabled = true; // Hide the sprite
        boxCollider.enabled = true; // Disable the collider
        light2D.SetActive(true); // Disable the light
        isBreaking = false; // Reset the breaking state
    }
}
