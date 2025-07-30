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

    public IEnumerator enterScene()
    {
        animator.SetTrigger("enterScene");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public IEnumerator exitScene()
    {
        animator.SetTrigger("exitScene");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
