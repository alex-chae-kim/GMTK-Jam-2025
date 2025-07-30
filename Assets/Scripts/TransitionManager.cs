using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public Animator animator;
    public static TransitionManager Instance { get; private set; }

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

    public void enterScene()
    {
        animator.SetTrigger("enterScene");
    }

    public void exitScene()
    {
        animator.SetTrigger("exitScene");
    }
}
