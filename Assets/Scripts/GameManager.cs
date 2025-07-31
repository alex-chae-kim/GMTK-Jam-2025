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
        // prompt user for upgrades
        // instantiate turtle
    }
    public IEnumerator instantiateTurtle(Transform turtleSpawnPoint, Transform camSpawnPoint)
    {
        Debug.Log("instantiateTurtle called");
        GameObject turtle = Instantiate(turtlePrefab, turtleSpawnPoint.position, Quaternion.identity);
        GameObject cameraObject = Instantiate(cinemachineCameraPrefab.gameObject, camSpawnPoint.position, Quaternion.identity);
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        CinemachineCamera camera = cameraObject.GetComponent<CinemachineCamera>();
        camera.Target.TrackingTarget = turtle.transform;
        //disable player controlls
        turtleController.moveSpeed = moveSpeed;
        turtleController.jumpForce = jumpForce;
        turtleController.camera = cameraObject;
        turtleController.gameManager = this;
        yield return new WaitForSeconds(0.1f);

        
    }
}
