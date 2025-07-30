using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene to load
    public TransitionManager transitionManager;

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
