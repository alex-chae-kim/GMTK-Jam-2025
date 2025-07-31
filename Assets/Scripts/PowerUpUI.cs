using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;



public class PowerUpUI : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public PowerUp[] powerUps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject description = card1.gameObject.transform.GetChild(2).gameObject;
        description.GetComponent<TextMeshProUGUI>().text = "pridesogged";
        GameObject description2 = card2.gameObject.transform.GetChild(2).gameObject;
        description2.GetComponent<TextMeshProUGUI>().text = powerUps[0].description;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }
    private void generateCards()
    {
        
    }
}
