using UnityEngine;

public class Turtle_Pickup : MonoBehaviour
{
    public GameObject powerUpUI;
    public PowerUpUI powerUpUIComponent;
    //public GameObject powerUpUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerUpUIComponent = powerUpUI.GetComponent<PowerUpUI>();
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
            if(powerUpUIComponent.powerUps[0].count == powerUpUIComponent.maxLevel && powerUpUIComponent.powerUps[1].count == powerUpUIComponent.maxLevel && powerUpUIComponent.powerUps[2].count == powerUpUIComponent.maxLevel){
                return;
            }
            powerUpUI.SetActive(true);


        }
    }
}
