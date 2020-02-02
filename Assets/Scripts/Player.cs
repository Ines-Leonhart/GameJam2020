using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] float minDistanceForSwipe;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] Transform center;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] GameObject waterVfxPrefab;
    [SerializeField] GameObject pulverizerVfxPrefab;
    [SerializeField] Transform vfxLocation;

    Vector3 fingerUpPosition;
    Vector3 fingerDownPosition;
    bool mouseDown;

    public enum Tools
    {
        Watercan,
        Pulverizer
    }

    public Tools currentTool = Tools.Watercan;
    public GameObject currentCell { get; set; }

    private void Start()
    {
        currentCell = checkGround(center.position).gameObject;
    }
    
    private void Update()
    {
        if(Singleton.Get<Game>() != null && Singleton.Get<Game>().CurrentState != Game.State.Play)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            fingerDownPosition = Input.mousePosition;
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }

        var tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
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

    public void ToggleMyTools()
    {
        if (currentTool == Tools.Watercan)
        {
            currentTool = Tools.Pulverizer;
        }
        else if (currentTool == Tools.Pulverizer)
        {
            currentTool = Tools.Watercan;
        }
    }

    public void UseMyTools()
    {
        if (currentTool == Tools.Watercan)
        {
            var plant = currentCell.GetComponent<cellScript>().Plant;
            if (plant != null)
            {
                plant.Water();
            }

            var vfx = Instantiate(waterVfxPrefab);
            vfx.transform.position = vfxLocation.position;
        }
        else if (currentTool == Tools.Pulverizer)
        {
            var plant = currentCell.GetComponent<cellScript>().Plant;
            if (plant != null)
            {
                plant.Heal();
            }

            var vfx = Instantiate(pulverizerVfxPrefab);
            vfx.transform.position = vfxLocation.position;
        }
    }

    private void MouseUp()
    {
        if (!mouseDown)
        {
            return;
        }

        Transform target;
        fingerUpPosition = Input.mousePosition;

        var verticalMovementDistance = Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        var horizontalMovementDistance = Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);

        var cellSize = cellPrefab.GetComponentInChildren<Renderer>().bounds.size;

        if (verticalMovementDistance > minDistanceForSwipe || horizontalMovementDistance > minDistanceForSwipe)
        {
            if (verticalMovementDistance > horizontalMovementDistance)
            {
                if (fingerDownPosition.y - fingerUpPosition.y > 0)
                {
                    target = checkGround(center.position + new Vector3(0, 0, -cellSize.z));
                    if (target != null)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
                        currentCell = target.gameObject;
                    }
                }
                else
                {
                    target = checkGround(center.position + new Vector3(0, 0, cellSize.z));
                    if (target != null)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
                        currentCell = target.gameObject;
                    }
                }
            }
            else
            {
                if (fingerDownPosition.x - fingerUpPosition.x > 0)
                {
                    target = checkGround(center.position + new Vector3(-cellSize.x, 0, 0));
                    if (target != null)
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
                        currentCell = target.gameObject;
                    }
                }
                else
                {
                    target = checkGround(center.position + new Vector3(cellSize.x, 0, 0));
                    if (target != null)
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
                        currentCell = target.gameObject;
                    }
                }
            }
        }

        mouseDown = false;
    }
}
