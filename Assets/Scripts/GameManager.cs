using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject turtlePrefab;

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
    
    public void instantiateTurtle(Transform spawnPoint)
    {
        GameObject turtle = Instantiate(turtlePrefab, spawnPoint.position, Quaternion.identity);
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        
    }
}
