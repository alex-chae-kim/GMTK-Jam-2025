using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputScore;
    [SerializeField] private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;
    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text,int.Parse(inputScore.text));
    }
}
