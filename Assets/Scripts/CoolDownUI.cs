using System.Collections;
using UnityEngine;

using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour
{
    GameObject player;
    TurtleController turtleController;
    private bool onCooldown;
    private float cooldownTimer;
    private float totalCooldown;

    [SerializeField] GameObject dashCooldownUI;
    [SerializeField] Image timerPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onCooldown = false;
        player = GameObject.FindWithTag("Player");
        turtleController = player.GetComponent<TurtleController>();
        timerPanel = dashCooldownUI.transform.GetChild(1).GetComponent<Image>();
        dashCooldownUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (turtleController.dashUnlocked)
        {
            dashCooldownUI.SetActive(true);
            if (onCooldown)
            {
                applyCooldown();
            }
        }
        
    }

    public void startCooldown(float cooldownTime)
    {
        
        onCooldown=true;
        cooldownTimer = cooldownTime;
        totalCooldown = cooldownTime; 

    }

    private void applyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0) 
        {
            onCooldown = false;
            timerPanel.fillAmount = 0.0f;
        }
        else
        {
            timerPanel.fillAmount = cooldownTimer / totalCooldown;
        }
    }
}
