using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
    public GameObject turtle;
    public Animator turtleAnimator;
    public GameObject startMenu;
    public CanvasGroup fadeCanvas;
    public GameObject bg;
    public CinemachineCamera virtualCamera;
    public GameManager gameManager;
    public GameObject healthBar;
    public GameObject timer;
    public Timer timerScript;
    public AudioManager audioManager;
    public GameObject skipText;
    public GameObject canvas;

    public float fallSpeed = 3f;
    public float groundY = -10f;
    public float fadeDuration = 1f;
    public float startMenuDisableDelay = 10f;

    public bool isFalling = false;

    public void Start(){
        turtleAnimator.Play("walk");
        canvas.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start Game Button Clicked!");
        startMenu.GetComponent<CanvasGroup>().interactable = false;
        virtualCamera.Follow = turtle.transform;
        isFalling = true;
        StartCoroutine(HandleStartSequence());
    }

    IEnumerator HandleStartSequence()
    {
        AudioManager.Instance.Play("Beginning Narration");
        skipText.SetActive(true);
        yield return new WaitForSecondsRealtime(startMenuDisableDelay);
        startMenu.SetActive(false);
    }

    void Update()
    {
        if (!isFalling) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            isFalling = false;
            StartCoroutine(HandleFadeTransition());
            return;
        }

        if (turtle.transform.position.y > groundY)
        {
            turtle.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else
        {
            isFalling = false;
            StartCoroutine(HandleFadeTransition());
        }
    }

    IEnumerator HandleFadeTransition()
    {
        AudioManager.Instance.fadeOutHelper("Beginning Narration", 0);
        bg.transform.position = turtle.transform.position;
        skipText.SetActive(false);
        yield return StartCoroutine(FadeCanvas(0f, 2f));
        Destroy(turtle);
        startMenu.SetActive(false);
        gameManager.initiateNextTurtleLife();
        yield return StartCoroutine(FadeCanvas(2f, 0f));
        healthBar.SetActive(true);
        timer.SetActive(true);
        timerScript.gameStarted = true;
        Debug.Log("Game Started!");
        Destroy(fadeCanvas.gameObject);
    }

    IEnumerator FadeCanvas(float from, float to)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, time / fadeDuration);
            fadeCanvas.alpha = alpha;
            yield return null;
        }
        fadeCanvas.alpha = to;
        yield break;
    }
}
