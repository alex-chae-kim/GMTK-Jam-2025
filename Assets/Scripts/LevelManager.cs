using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private TransitionManager transitionManager;
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // duplicate, destroy this scene copy
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        transitionManager = TransitionManager.Instance;
        if (transitionManager == null)
        {
            Debug.LogError("TransitionManager instance not found. Please ensure it is present in the scene.");
            return;
        }
    }
    public void loadNextScene()
    {
        StartCoroutine(loadNextSceneRoutine());
    }

    public void loadScene(string sceneToLoad)
    {
        StartCoroutine(loadSceneRoutine(sceneToLoad));
    }

    private IEnumerator loadNextSceneRoutine()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("No more scenes to load.");
            yield break;
        }

        yield return transitionManager.exitScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return transitionManager.enterScene();
    }

    private IEnumerator loadSceneRoutine(string sceneToLoad)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.LogError($"Scene \"{sceneToLoad}\" cannot be loaded. Check if it's added to Build Settings.");
            yield break;
        }

        yield return transitionManager.exitScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return transitionManager.enterScene();
    }
}
