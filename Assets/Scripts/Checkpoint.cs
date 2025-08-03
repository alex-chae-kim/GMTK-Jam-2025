using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Checkpoint : MonoBehaviour
{
    private bool triggered = false;
    public bool left;
    public bool last;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            GameManager gm = FindObjectOfType<GameManager>();
            if (last)
            {
                gm.StartCoroutine(gm.FinalSequence(other.gameObject, left));
            }
            else
            {
                gm.StartCoroutine(gm.EndLevelSequence(other.gameObject, left));
            }
        }
    }
}
