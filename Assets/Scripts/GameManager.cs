using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject turtlePrefab;
    public GameObject cinemachineCameraPrefab;

    public Transform turtleSpawnPoint;
    public Transform camSpawnPoint;

    private float moveSpeed = 2f;
    private float jumpForce = 5f;
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
        // get info about current level
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
