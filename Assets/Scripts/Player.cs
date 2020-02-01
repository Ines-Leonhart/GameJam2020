using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float minDistanceForSwipe;
    [SerializeField] GameObject cellPrefab;

    Vector3 fingerUpPosition;
    Vector3 fingerDownPosition;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fingerDownPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    private Transform checkGround(Vector3 position)
    {
        var iterable = Physics.RaycastAll(position, Vector3.down);
        for (int i=0; i < iterable.Length; i++)
        {
            if(iterable[i].transform.tag == "cell")
            {
                return iterable[i].transform;
            }
        }
        return null;
    }

    private void MouseUp()
    {
        Transform target;
        fingerUpPosition = Input.mousePosition;

        var verticalMovementDistance = Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        var horizontalMovementDistance = Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);

        var cellSize = cellPrefab.GetComponent<Renderer>().bounds.size;
        var myHeight = GetComponent<Renderer>().bounds.size.y;

        if (verticalMovementDistance > minDistanceForSwipe || horizontalMovementDistance > minDistanceForSwipe)
        {
            if (verticalMovementDistance > horizontalMovementDistance)
            {
                if (fingerDownPosition.y - fingerUpPosition.y > 0)
                {
                    Debug.Log("Swipe down");
                    Debug.DrawRay(transform.position + new Vector3(0, 0, -cellSize.z), Vector3.down, Color.red, 3);
                    Debug.Log(transform.position);
                    target = checkGround(transform.position + new Vector3(0, 0, -cellSize.z));
                    if (target != null)
                    { 
                        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
                    }
                }
                else
                {
                    Debug.Log("Swipe up");
                    Debug.DrawRay(transform.position + new Vector3(0, 0, cellSize.z), Vector3.down, Color.red, 3);
                    Debug.Log(transform.position);
                    target = checkGround(transform.position + new Vector3(0, 0, cellSize.z));
                    if (target != null)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
                    }
                }
            }
            else
            {
                if (fingerDownPosition.x - fingerUpPosition.x > 0)
                {
                    Debug.Log("Swipe left");
                    Debug.DrawRay(transform.position + new Vector3(-cellSize.x, 0, 0), Vector3.down, Color.red, 3);
                    Debug.Log(transform.position);
                    target = checkGround(transform.position + new Vector3(-cellSize.x, 0, 0));
                    if (target != null)
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
                    }
                }
                else
                {
                    Debug.Log("Swipe right");
                    Debug.DrawRay(transform.position + new Vector3(cellSize.x, 0, 0), Vector3.down, Color.red, 3);
                    Debug.Log(transform.position);
                    target = checkGround(transform.position + new Vector3(cellSize.x, 0, 0));
                    if (target != null)
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
                    }
                }
            }
        }
    }
}
