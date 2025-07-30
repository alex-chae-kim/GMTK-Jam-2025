using UnityEngine;

public class ManagerManager : MonoBehaviour
{
    public TransitionManager transitionManager;
    public LevelManager levelManager;
    public AudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transitionManager = TransitionManager.Instance;
        if (transitionManager == null)
        {
            Debug.LogError("TransitionManager instance not found. Please ensure it is present in the scene.");
            return;
        }
        levelManager = LevelManager.Instance;
        if (levelManager == null)
        {
            Debug.LogError("LevelManager instance not found. Please ensure it is present in the scene.");
            return;
        }
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager instance not found. Please ensure it is present in the scene.");
            return;
        }
    }

    public void loadNextScene()
    {
        levelManager.loadNextScene();
    }

    public void loadScene(string sceneName)
    {
        levelManager.loadScene(sceneName);
    }

    public Sound Play(string name)
    {
        return audioManager.Play(name);
    }
}
