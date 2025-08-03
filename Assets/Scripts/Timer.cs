using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public GameManager gameManager;
    float elapsedTime;
    
    public bool gameStarted = false;
    public int score = 0;
    void Update()
    {
        if(gameStarted == false) return;
        if(gameManager.gameOver) return;
        elapsedTime += Time.deltaTime;        
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //print(score);
        score = Mathf.FloorToInt(elapsedTime);
    }
}
