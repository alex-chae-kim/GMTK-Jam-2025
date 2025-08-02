using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 0;
    public Slider healthBar;
    public GameObject turtlePrefab;
    public GameObject cinemachineCameraPrefab;
    public GameObject powerUpUIPrefab;
    public PowerUpUI powerUpUI;

    [SerializeField]
    public LevelSpawnPoints[] levelSpawnPoints;

    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float turtleHealth = 10f;

    private float runOutDistance = 4f; // distance in # of tiles the turtle runs out of cave on its own
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseGame()
    {
        Time.timeScale = 0f; // pause the game
    }

    public void resumeGame()
    {
        Time.timeScale = 1f; // resume the game
    }

    public void initiateNextTurtleLife()
    {
        // get the spawn points for the current level and starts the turtle instantiation coroutine
        Transform turtleSpawn = levelSpawnPoints[currentLevel].turtleSpawnPoint;
        Transform camSpawn = levelSpawnPoints[currentLevel].camSpawnPoint;
        StartCoroutine(instantiateTurtle(turtleSpawn, camSpawn));
    }
    public IEnumerator instantiateTurtle(Transform turtleSpawnPoint, Transform camSpawnPoint)
    {
        // instantiate the turtle and camera at the specified spawn points and get references to their components
        Debug.Log("instantiateTurtle called");
        GameObject turtle = Instantiate(turtlePrefab, turtleSpawnPoint.position, Quaternion.identity);
        GameObject cameraObject = Instantiate(cinemachineCameraPrefab.gameObject, camSpawnPoint.position, Quaternion.identity);
        Turtle_Pickup turtlePickup = turtle.GetComponent<Turtle_Pickup>();
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        CinemachineCamera camera = cameraObject.GetComponent<CinemachineCamera>();
        Animator anim = turtle.GetComponent<Animator>();

        // set the camera's target to the turtle and disable player controls for a short time
        camera.Target.TrackingTarget = turtle.transform;
        turtleController.controlsEnabled = false; //disable player controls

        // set the turtle's camera and game manager references
        turtleController.camera = cameraObject;
        turtleController.gameManager = this;
        turtlePickup.powerUpUI = powerUpUIPrefab;
        turtleController.healthBar = healthBar;
        powerUpUI.player = turtle;
        // wait for a short time to allow the camera to focus on the turtle
        yield return new WaitForSeconds(2f);

        // set the player reference in the powerUpUI script
        
        powerUpUIPrefab.SetActive(true);
        if (healthBar != null) // fill health bar bc it looks nice
        {
            healthBar.maxValue = turtleHealth;
            healthBar.value = turtleHealth;
        }
        while (powerUpUIPrefab.activeSelf )
        {
            yield return null; // wait until the powerUpUI is turned off
        }

        // set the turtle's properties
        turtleController.moveSpeed = moveSpeed;
        turtleController.jumpForce = jumpForce;
        turtleController.lifetime = turtleHealth;
        

        if (healthBar != null) // fill health bar again bc it might have been updated
        {
            healthBar.maxValue = turtleHealth;
            healthBar.value = turtleHealth;
        }


        // make the turtle run out of the cave a set distance (this is SUPER jank but it works). It is intended behavior that the player can prematurely end
        // the turtle's run out by pressing a movement key
        float time = runOutDistance / moveSpeed;
        bool breaked = false;
        turtle.transform.localScale = new Vector3(-1, 1, 1); // Facing right
        anim.SetTrigger("forceRun"); // Set running animation
        while (time > 0)
        {
            float currentVelX = moveSpeed;
            turtleController.rb.linearVelocity = new Vector2(currentVelX, 0f);
            time -= Time.deltaTime;
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                turtleController.controlsEnabled = true; // enable player controls
                turtleController.moveInput = Input.GetAxisRaw("Horizontal");
                breaked = true;
                break;
            }
            yield return null;
        }
        if (!breaked)
        {
            turtleController.rb.linearVelocity = new Vector2(0f, 0f);
            turtleController.controlsEnabled = true;
        }
        
        

    }
}
