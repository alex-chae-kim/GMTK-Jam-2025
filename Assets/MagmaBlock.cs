using UnityEngine;

public class MagmaBlock : MonoBehaviour
{
    public float damagePerSecond = 2f;
    private TurtleController turtle;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (turtle == null)
                turtle = other.GetComponent<TurtleController>();

            if (turtle != null && turtle.lifetime > 0)
            {
                turtle.lifetime -= damagePerSecond * Time.deltaTime;
                Debug.Log(turtle.lifetime);
                if (turtle.healthBar != null)
                {
                    Debug.Log("Damaing");
                    turtle.healthBar.value = Mathf.Clamp(turtle.lifetime, 0, turtle.healthBar.maxValue);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            turtle = null;
        }
    }
}
