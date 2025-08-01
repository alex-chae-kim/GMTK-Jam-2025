using UnityEngine;

public class SpecialPowerUp : MonoBehaviour
{
    public PowerUp specialAbility;
    public GameObject UI;
    public PowerUpUI powerUpUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI = GameObject.Find("Game UI");
        powerUpUI = UI.gameObject.transform.GetChild(1).GetComponent<PowerUpUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        powerUpUI.special = true;
        powerUpUI.generateSpecial(specialAbility);
    }
}
