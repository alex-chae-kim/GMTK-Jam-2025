using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    
    public bool gameStarted = false;
    public int score = 0;
    void Update()
    {
        if(gameStarted == false) return;
        elapsedTime += Time.deltaTime;        
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //print(score);
        score = Mathf.FloorToInt(elapsedTime);
    }
}
