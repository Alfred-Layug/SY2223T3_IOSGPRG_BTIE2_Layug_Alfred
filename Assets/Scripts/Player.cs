using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private bool inRange;

    float timer;

    private Vector2 initialTouchPosition;
    private Vector2 endTouchPosition;

    private bool swipedLeft;
    private bool swipedRight;
    private bool swipedUp;
    private bool swipedDown;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if ((image.sprite == sprites[1] || image.sprite == sprites[6]) && swipedLeft == true)    //Player must swipe left
        {
            Destroy(other.gameObject);
            swipedLeft = false;
        }

        if ((image.sprite == sprites[2] || image.sprite == sprites[5]) && swipedRight == true)    //Player must swipe right
        {
            Destroy(other.gameObject);
            swipedRight = false;
        }

        if ((image.sprite == sprites[0] || image.sprite == sprites[7]) && swipedDown == true)    //Player must swipe down
        {
            Destroy(other.gameObject);
            swipedDown = false;
        }

        if ((image.sprite == sprites[3] || image.sprite == sprites[4]) && swipedUp == true)    //Player must swipe up
        {
            Destroy(other.gameObject);
            swipedUp = false;
        }

        inRange = false;
        StartCoroutine(CO_Timer());

        //SpawnManager.Instance.RemoveEnemyFromList(other.gameObject);
    }

    private void Start()
    {
        transform.position = new Vector2(0.0f, 0.0f);

        inRange = false;
        StartCoroutine(CO_Timer());

        swipedLeft = false;
        swipedRight = false;
        swipedUp = false;
        swipedDown = false;
    }

    private void Update()
    {

        if (inRange == true)    //This is to prevent the player from swiping ahead of time
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
            image.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        image.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    private void CheckSwipe()
    {
        if (image.sprite == sprites[0] || image.sprite == sprites[7] || image.sprite == sprites[3] || image.sprite == sprites[4])
        {
            if (initialTouchPosition.y < endTouchPosition.y)
            {
                Debug.Log("Swiped Up");
                swipedRight = false;
                swipedLeft = false;
                swipedDown = false;
                swipedUp = true;
            }
            else
            {
                Debug.Log("Swiped Down");
                swipedRight = false;
                swipedLeft = false;
                swipedDown = true;
                swipedUp = false;
            }
        }
        else
        {
            if (initialTouchPosition.x < endTouchPosition.x)
            {
                Debug.Log("Swiped Right");
                swipedRight = true;
                swipedLeft = false;
                swipedDown = false;
                swipedUp = false;
            }
            else
            {
                Debug.Log("Swiped Left");
                swipedRight = false;
                swipedLeft = true;
                swipedDown = false;
                swipedUp = false;
            }
        }
    }
}
