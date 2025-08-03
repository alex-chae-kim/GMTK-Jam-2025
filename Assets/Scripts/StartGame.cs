using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
    public GameObject turtle;
    public Transform turtleSpawnPoint;
    public Animator turtleAnimator;
    public GameObject startMenu;
    public CanvasGroup fadeCanvas;
    public CinemachineCamera virtualCamera;
    public GameManager gameManager;
    public GameObject healthBar;
    public GameObject timer;
    public Timer timerScript;

    public float fallSpeed = 3f;
    public float groundY = -10f;
    public float fadeDuration = 1f;
    public float startMenuDisableDelay = 10f;

    public bool isFalling = false;

    public void Start(){
        turtleAnimator.Play("walk");
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
        fadeCanvas.transform.position = turtle.transform.position;
        yield return StartCoroutine(FadeCanvas(0f, 2f));
        Destroy(turtle);
        startMenu.SetActive(false);
        yield return StartCoroutine(FadeCanvas(2f, 0f));
        healthBar.SetActive(true);
        timer.SetActive(true);
        timerScript.gameStarted = true;
        gameManager.initiateNextTurtleLife();
        Debug.Log("Game Started!");
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
