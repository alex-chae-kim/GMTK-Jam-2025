using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 0;
    public GameObject turtlePrefab;
    public GameObject cinemachineCameraPrefab;

    [SerializeField]
    public LevelSpawnPoints[] levelSpawnPoints;

    private float moveSpeed = 2f;
    private float jumpForce = 5f;
    private float turtleHealth = 10f;

    private float runOutDistance = 10f; // distance to run out before next turtle life
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void initiateNextTurtleLife()
    {
        Transform turtleSpawn = levelSpawnPoints[currentLevel].turtleSpawnPoint;
        Transform camSpawn = levelSpawnPoints[currentLevel].camSpawnPoint;
        StartCoroutine(instantiateTurtle(turtleSpawn, camSpawn));
    }
    public IEnumerator instantiateTurtle(Transform turtleSpawnPoint, Transform camSpawnPoint)
    {
        Debug.Log("instantiateTurtle called");
        GameObject turtle = Instantiate(turtlePrefab, turtleSpawnPoint.position, Quaternion.identity);
        GameObject cameraObject = Instantiate(cinemachineCameraPrefab.gameObject, camSpawnPoint.position, Quaternion.identity);
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        CinemachineCamera camera = cameraObject.GetComponent<CinemachineCamera>();
        camera.Target.TrackingTarget = turtle.transform;
        turtleController.controlsEnabled = false; //disable player controls
        yield return new WaitForSeconds(2f);
        // open upgrade menu
        //wait for user to select upgrades
        turtleController.moveSpeed = moveSpeed;
        turtleController.jumpForce = jumpForce;
        turtleController.lifetime = turtleHealth;
        turtleController.camera = cameraObject;
        turtleController.gameManager = this;
        // run out animation
        float time = runOutDistance / moveSpeed;
        bool breaked = false;
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
