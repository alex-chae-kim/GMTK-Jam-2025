using UnityEngine;

public class ShellInteractable : MonoBehaviour
{
    public float interactionRange = 2.5f;
    public GameObject outline;
    public Texture2D pickaxeCursor;
    public Texture2D cursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;

    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Color originalColor;
    private bool isHovered;
    public GameManager gameManager;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No player with tag 'Player' found in scene.");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogWarning("No GameManager found in scene.");
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

        if(gameManager.canDestroy == false)
        {
            return;
        }

        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist <= interactionRange)
        {
            spriteRenderer.color = new Color(0.6f, 0f, 0f);
            isHovered = true;

            outline.SetActive(true);

            if (pickaxeCursor != null)
            {
                Cursor.SetCursor(pickaxeCursor, cursorHotspot, CursorMode.ForceSoftware);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            ResetVisuals();
        }
    }

    void OnMouseExit()
    {
        ResetVisuals();
    }

    void ResetVisuals()
    {
        spriteRenderer.color = originalColor;
        isHovered = false;
        outline.SetActive(false);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
