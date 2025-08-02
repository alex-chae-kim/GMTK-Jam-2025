using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;

public class StartGameManager : MonoBehaviour
{
    public GameObject turtle;
    public Animator turtle1Animator;
    public GameObject startMenu;
    public CanvasGroup fadeCanvas;
    public CinemachineCamera virtualCamera;
    public GameManager gameManager;
    public GameObject healthBar;

    public float fallSpeed = 3f;
    public float groundY = -10f;
    public float fadeDuration = 1f;
    public float startMenuDisableDelay = 2f;

    public bool isFalling = false;

    public void Start(){
        turtle1Animator.SetTrigger("walk");
    }

    public void OnStartButtonClicked()
    {
        startMenu.GetComponent<CanvasGroup>().interactable = false;
        virtualCamera.Follow = turtle.transform;
        isFalling = true;
        StartCoroutine(HandleStartSequence());
    }

    IEnumerator HandleStartSequence()
    {
        yield return new WaitForSeconds(startMenuDisableDelay);
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
        yield return StartCoroutine(FadeCanvas(0f, 2f));
        Destroy(turtle);
        startMenu.SetActive(false);
        yield return StartCoroutine(FadeCanvas(2f, 0f));
        healthBar.SetActive(true);
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
