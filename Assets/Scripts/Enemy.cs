using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int _health;
    public int _attack;
    public int _defense;

    [SerializeField] private Image image;
    [SerializeField] private Sprite arrowSprite;

    [SerializeField] public bool inRange;

    public bool damagePlayer;

    float timer;
    float movementSpeed;

    Quaternion originalRotation;

    public enum Direction { Left, Down, Right, Up }
    public enum Color { Green, Red }

    public Direction arrowDirection;
    public Color arrowColor;

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
        //Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        damagePlayer = true;
    }

    void Start()
    {
        movementSpeed = 4.0f;

        inRange = false;
        arrowDirection = (Direction)Random.Range(0, 4);
        arrowColor = (Color)Random.Range(0, 2);
        StartCoroutine(CO_Timer());

        damagePlayer = false;

        originalRotation = image.transform.rotation;

        Debug.Log(arrowDirection);
        Debug.Log(arrowColor);
    }

    private void Update()
    {
        if (SpawnManager.Instance._playerIsDashing)     //Enemies move faster if player is dashing to make it look like the player really is moving faster
        {
            transform.Translate(Vector3.down * Time.deltaTime * movementSpeed * 2.0f, Space.World);
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * movementSpeed, Space.World);
        }

        if (SpawnManager.Instance._playerIsAlive == false)  //Destroys all enemies and clears the enemies list in SpawnManager if player is killed
        {
            DoDeath();
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

    public void DoDeath()
    {
        SpawnManager.Instance.RemoveEnemyFromList(gameObject);
        Destroy(gameObject);
    }
}