using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    private Vector2 initialTouchPosition;
    private Vector2 endTouchPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            initialTouchPosition = touch.position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Touch touch = Input.GetTouch(0);
            endTouchPosition = touch.position;

            CheckSwipe();
        }
    }

    private void CheckSwipe()
    {
        if (initialTouchPosition.x < endTouchPosition.x)
        {
            debugText.text = "Swiped Right";
        }
        else
        {
            debugText.text = "Swiped Left";
        }
    }

    private bool PointIsOnLeft(Vector2 point1, Vector2 point2, Vector2 point3)
    {
        return (point2.x - point1.x) * (point3.y - point1.y) - (point2.y - point1.y) * (point3.x - point1.x) > 0;
    }
}
