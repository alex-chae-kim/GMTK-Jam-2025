using UnityEngine;

public class ShellInteractable : MonoBehaviour
{
    public float interactionRange = 2.5f;
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Color originalColor;
    private bool isHovered;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No player with tag 'Player' found in scene.");
        }
    }

    void OnMouseOver()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Player Null");
            return;
        }


        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist <= interactionRange)
        {
            spriteRenderer.color = new Color(0.6f, 0f, 0f); // Deep red
            isHovered = true;

            if (Input.GetMouseButtonDown(0)) // Left click
            {
                Destroy(gameObject);
            }
        }
        else
        {
            spriteRenderer.color = originalColor;
            isHovered = false;
        }
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
        isHovered = false;
    }
}
