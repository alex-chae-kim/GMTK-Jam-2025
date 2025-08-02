using UnityEngine;

public class Turtle_Pickup : MonoBehaviour
{
    public GameObject powerUpUI;
    //public GameObject powerUpUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pickup")
        {
            AudioManager.Instance.Play("Powerup");
            Destroy(collision.gameObject);
            powerUpUI.SetActive(true);


        }
    }
}
