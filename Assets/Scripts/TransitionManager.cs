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
        yield return new WaitForSecondsRealtime(1f);
    }

    public IEnumerator exitScene()
    {
        animator.SetTrigger("exitScene");
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        while (!info.IsName("Transition_Out"))
        {
            yield return null;
            info = animator.GetCurrentAnimatorStateInfo(0);
        }

        while (info.normalizedTime < 1.0f)
        {
            yield return null;
            info = animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}
