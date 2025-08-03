using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Checkpoint : MonoBehaviour
{

    public GameManager gameManager;
    bool checkpointTriggered = false;

    private void OnTriggerEnter2D(Collider2D other){
        if (checkpointTriggered) return;
        EndLevelSequence(other.gameObject);
        checkpointTriggered = true;
    }

    public IEnumerator EndLevelSequence(GameObject turtle){
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        Animator anim = turtle.GetComponent<Animator>();
        
        turtleController.controlsEnabled = false;
        
        turtle.transform.localScale = new Vector3(1, 1, 1);
        anim.SetBool("forceWalk", true);

        float walkTime = 1.5f;
        float timer = 0f;

        while (timer < walkTime){
            turtleController.rb.linearVelocity = new Vector2(-gameManager.moveSpeed, 0f);
            timer += Time.deltaTime;
            yield return null;
        }

        turtleController.rb.linearVelocity = Vector2.zero;
        anim.SetBool("forceWalk", false);

        anim.Play("fadeOut");
        yield return new WaitForSeconds(2f);
        Destroy(turtle);
        gameManager.nextLevel();
    }
}
