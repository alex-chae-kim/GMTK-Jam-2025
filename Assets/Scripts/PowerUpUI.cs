using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;



public class PowerUpUI : MonoBehaviour
{
    
    public GameObject[] cards;
    public PowerUp[] powerUps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameObject description = cards[0].gameObject.transform.GetChild(2).gameObject;
        //description.GetComponent<TextMeshProUGUI>().text = "pridesogged";
        //GameObject description2 = cards[1].gameObject.transform.GetChild(2).gameObject;
        //description2.GetComponent<TextMeshProUGUI>().text = powerUps[0].description;

        PowerUp[] currentPowers = new PowerUp[cards.Length];
        for(int i = 0;i<cards.Length; i++) 
        {
            
            int rand = Random.Range(0,powerUps.Length);
            PowerUp chosenPower = powerUps[rand];
            currentPowers[i] = chosenPower;

            GameObject name = cards[i].gameObject.transform.GetChild(3).gameObject;
            name.GetComponent<TextMeshProUGUI>().text = chosenPower.name;
            print(chosenPower.name);
            GameObject description = cards[i].gameObject.transform.GetChild(2).gameObject;
            description.GetComponent<TextMeshProUGUI>().text = chosenPower.description;

            GameObject image = cards[i].gameObject.transform.GetChild(4).gameObject;
            image.GetComponent<Image>().sprite = chosenPower.image;

            GameObject button = cards[i].gameObject.transform.GetChild(1).gameObject;
            button.GetComponentInChildren<TextMeshProUGUI>().text = chosenPower.name;




        }

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
