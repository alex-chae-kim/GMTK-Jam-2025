using UnityEngine;

public class PulleyPlatform : MonoBehaviour
{
    public Vector3 localOffset = new Vector3(0, 3, 0); // offset for positionB
    public float moveSpeed = 2f;
    public bool isActivated = false;

    public Vector3 positionA;
    private Vector3 positionB;

    void Start()
    {
        positionA = transform.position;
        positionB = transform.position + localOffset;
    }

    void Update()
    {
        Vector3 target = isActivated ? positionB : positionA;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}
