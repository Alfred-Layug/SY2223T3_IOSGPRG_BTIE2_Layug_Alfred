using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int _health;
    public int _attack;
    public int _defense;

    [SerializeField] private float startingXPosition;
    [SerializeField] private float startingYPosition;

    [SerializeField] private Image image;
    [SerializeField] private Sprite arrowSprite;

    [SerializeField] private bool inRange;

    private Vector2 initialTouchPosition;
    private Vector2 endTouchPosition;

    float timer;
    float movementSpeed;

    Quaternion originalRotation;

    public enum Direction { Left, Down, Right, Up }
    public enum Color { Green, Red }

    Direction arrowDirection;
    Color arrowColor;

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
        //Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;    // Enemy can no longer be killed if it gets too close to the player
    }

    void Start()
    {
        movementSpeed = 4.0f;

        transform.position = new Vector2 (startingXPosition, startingYPosition);

        inRange = false;
        arrowDirection = (Direction)Random.Range(0, 4);
        arrowColor = (Color)Random.Range(0, 2);
        StartCoroutine(CO_Timer());

        originalRotation = image.transform.rotation;

        Debug.Log(arrowDirection);
        Debug.Log(arrowColor);
    }

    private void Update()
    {
        startingYPosition = startingYPosition - (Time.deltaTime * movementSpeed);

        transform.Translate(Vector3.down * Time.deltaTime * movementSpeed, Space.World);

        if (inRange == true)
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
    }

    private IEnumerator CO_Timer()
    {
        while (!inRange)
        {
            yield return new WaitForSeconds(0.2f);
            image.sprite = arrowSprite;
            image.GetComponent<Image>().color = new Color32(255, 255, 0, 100);  //Arrow will be color yellow while still rotating
            image.transform.Rotate(0, 0, 90);
        }

        image.transform.rotation = originalRotation;
        image.sprite = arrowSprite;

        if (arrowColor == Color.Green)
        {
            image.GetComponent<Image>().color = new Color32(0, 255, 0, 100);

            if (arrowDirection == Direction.Down)
            {
                image.transform.Rotate(0, 0, 90);
            }
            else if (arrowDirection == Direction.Right)
            {
                image.transform.Rotate(0, 0, 180);
            }
            else if (arrowDirection == Direction.Up)
            {
                image.transform.Rotate(0, 0, -90);
            }
            else
            {
                image.transform.Rotate(0, 0, 0);
            }
        }
        else    // Rotations are opposite here since red arrows tell the player to swipe in the opposite direction
        {
            image.GetComponent<Image>().color = new Color32(255, 0, 0, 100);

            if (arrowDirection == Direction.Down)
            {
                image.transform.Rotate(0, 0, -90);
            }
            else if (arrowDirection == Direction.Left)
            {
                image.transform.Rotate(0, 0, 180);
            }
            else if (arrowDirection == Direction.Up)
            {
                image.transform.Rotate(0, 0, 90);
            }
            else
            {
                image.transform.Rotate(0, 0, 0);
            }
        }
    }

    private void CheckSwipe()
    {
        Debug.Log("X " + Mathf.Abs(endTouchPosition.x - initialTouchPosition.x));
        Debug.Log("Y " + Mathf.Abs(endTouchPosition.y - initialTouchPosition.y));

        if (Mathf.Abs(endTouchPosition.y - initialTouchPosition.y) >
            Mathf.Abs(endTouchPosition.x - initialTouchPosition.x) &&
            Mathf.Abs(endTouchPosition.y - initialTouchPosition.y) >= 480)
        {
            if (initialTouchPosition.y < endTouchPosition.y && arrowDirection == Direction.Up)
            {
                Debug.Log("Swiped Up");
                Destroy(gameObject);
            }
            else if (initialTouchPosition.y > endTouchPosition.y && arrowDirection == Direction.Down)
            {
                Debug.Log("Swiped Down");
                Destroy(gameObject);
            }
            else
            {
                inRange = false;    //Player can no longer swipe after swiping in the wrong direction
                Debug.Log("Wrong swipe, you got damaged.");
            }
        }
        else if (Mathf.Abs(endTouchPosition.x - initialTouchPosition.x) >= 480)
        {
            if (initialTouchPosition.x < endTouchPosition.x && arrowDirection == Direction.Right)
            {
                Debug.Log("Swiped Right");
                Destroy(gameObject);

            }
            else if (initialTouchPosition.x > endTouchPosition.x && arrowDirection == Direction.Left)
            {
                Debug.Log("Swiped Left");
                Destroy(gameObject);
            }
            else
            {
                inRange = false;    //Player can no longer swipe after swiping in the wrong direction
                Debug.Log("Wrong swipe, you got damaged.");
            }
        }
        else
        {
            Debug.Log("Swipe not counted");
        }
    }
}