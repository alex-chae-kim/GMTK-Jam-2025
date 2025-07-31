using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;



public class PowerUpUI : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject description = card1.gameObject.transform.GetChild(2).gameObject;
        description.GetComponent<TextMeshProUGUI>().text = "pridesogged";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
