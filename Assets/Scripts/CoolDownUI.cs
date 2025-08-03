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
    [SerializeField] Image icon;
    Color originalIconColor;
    [SerializeField] Image panel;
    [SerializeField] Image timerPanel;
    public bool doubleJump;
    public bool dash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onCooldown = false;
        player = GameObject.FindWithTag("Player");
        turtleController = player.GetComponent<TurtleController>();
        if(dash){
            timerPanel = dashCooldownUI.transform.GetChild(1).GetComponent<Image>();
            dashCooldownUI.SetActive(false);
        }
        originalIconColor = panel.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (turtleController.dashUnlocked)
        {
            if(dash){
                dashCooldownUI.SetActive(true);
                if (onCooldown)
                {
                    applyCooldown();
                }
            }
        }

        Debug.Log("Double Jump: " + doubleJump);
        Debug.Log("TurtleController.isGrounded: " + turtleController.isGrounded);
        if(!turtleController.isGrounded && doubleJump){
            icon.color = new Color(0.5f, 0.5f, 0.5f);
            panel.color = new Color(0.5f, 0.5f, 0.5f);
        }else{
            icon.color = new Color(1f, 1f, 1f);
            panel.color = originalIconColor;
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
        icon.color = new Color(0.5f, 0.5f, 0.5f);
        panel.color = new Color(0.5f, 0.5f, 0.5f);
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
        
        icon.color = new Color(1f, 1f, 1f);
        panel.color = originalIconColor;
    }
}
