using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;



public class PowerUpUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] cards;
    public PowerUp[] powerUps;
    public GameObject player;
    PowerUp[] currentPowers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameObject description = cards[0].gameObject.transform.GetChild(2).gameObject;
        //description.GetComponent<TextMeshProUGUI>().text = "pridesogged";
        //GameObject description2 = cards[1].gameObject.transform.GetChild(2).gameObject;
        //description2.GetComponent<TextMeshProUGUI>().text = powerUps[0].description;

        player = GameObject.FindWithTag("Player");
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        currentPowers = new PowerUp[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {

            int rand = Random.Range(0, powerUps.Length);
            PowerUp chosenPower = powerUps[rand];
            print(chosenPower);
            currentPowers[i] = chosenPower;

            GameObject name = cards[i].gameObject.transform.GetChild(3).gameObject;
            name.GetComponent<TextMeshProUGUI>().text = chosenPower.name;
            
            GameObject description = cards[i].gameObject.transform.GetChild(2).gameObject;
            description.GetComponent<TextMeshProUGUI>().text = chosenPower.description;

            GameObject image = cards[i].gameObject.transform.GetChild(4).gameObject;
            image.GetComponent<Image>().sprite = chosenPower.image;

            GameObject button = cards[i].gameObject.transform.GetChild(1).gameObject;
            button.GetComponentInChildren<TextMeshProUGUI>().text = chosenPower.name;


        }
    }
    private void generateCards()
    {
        
    }

    public void selectPower(int index)
    {
        TurtleController playerController = player.GetComponent<TurtleController>();
        print(currentPowers[index]);
        // Apply the buffs to the active turtle
        playerController.jumpForce += currentPowers[index].jumpBuff;
        playerController.lifetime += currentPowers[index].lifeBuff;
        playerController.moveSpeed += currentPowers[index].speedBuff;

        // Apply the buffs to the game manager so future turtles inherit the buffs
        gameManager.jumpForce += currentPowers[index].jumpBuff;
        gameManager.turtleHealth += currentPowers[index].lifeBuff;
        gameManager.moveSpeed += currentPowers[index].speedBuff;

        this.gameObject.SetActive(false);
    }
}
