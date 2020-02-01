using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float minDistanceForSwipe;

    Vector3 fingerUpPosition;
    Vector3 fingerDownPosition;

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

    private void MouseUp()
    {
        fingerUpPosition = Input.mousePosition;

        var verticalMovementDistance = Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        var horizontalMovementDistance = Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);

        if (verticalMovementDistance > minDistanceForSwipe || horizontalMovementDistance > minDistanceForSwipe)
        {
            if (verticalMovementDistance > horizontalMovementDistance)
            {
                if (fingerDownPosition.y - fingerUpPosition.y > 0)
                {
                    Debug.Log("Swipe down");
                }
                else
                {
                    Debug.Log("Swipe up");
                }
            }
            else
            {
                if (fingerDownPosition.x - fingerUpPosition.x > 0)
                {

                    Debug.Log("Swipe left");
                }
                else
                {
                    Debug.Log("Swipe right");
                }
            }
        }
    }
}
