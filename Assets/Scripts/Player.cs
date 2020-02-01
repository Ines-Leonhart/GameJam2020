using UnityEngine;
using DG.Tweening;

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
                    target = checkGround(transform.position + new Vector3(0, 0, -cellSize.z));
                    if (target != null)
                    { 
                        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
                    }
                }
                else
                {
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
                    target = checkGround(transform.position + new Vector3(-cellSize.x, 0, 0));
                    if (target != null)
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
                    }
                }
                else
                {
                    target = checkGround(transform.position + new Vector3(cellSize.x, 0, 0));
                    if (target != null)
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
                    }
                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            var iterable = Physics.RaycastAll(ray);
            System.Collections.Generic.List<RaycastHit> list = new System.Collections.Generic.List<RaycastHit>();

            for (int i = 0; i < iterable.Length; i++)
            {
                if(iterable[i].transform.tag == "cell")
                {
                    list.Add(iterable[i]);
                }
            }
            if(list.Count > 0)
            {
                RaycastHit chosen = new RaycastHit();
                chosen = list[0];
                list.ForEach(delegate (RaycastHit element)
                {
                    if (Vector3.Distance(chosen.point, fingerDownPosition) > Vector3.Distance(element.point, fingerDownPosition))
                    {
                        chosen = element;
                    }
                });
                var originalColor = chosen.transform.gameObject.GetComponent<Renderer>().material.color;
                chosen.transform.gameObject.GetComponent<Renderer>().material.DOColor(new Color(0, 255, 0, 0.2f), 0.1f).From();
            }
        }
    }
}
