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

    private int rotating;

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
        rotating = Random.Range(0, 2);  //0 = not rotating; 1 = rotating

        Debug.Log("Rotation: " + rotating);

        if (arrowColor == Color.Green && rotating == 1)
        {
            StartCoroutine(CO_Timer());
        }
        else if (arrowColor == Color.Red)
        {
            image.sprite = arrowSprite;
            image.GetComponent<Image>().color = new Color32(255, 0, 0, 50);     //Red arrow will be less opaque if not yet in range
            
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
        else if (arrowColor == Color.Green && rotating == 0)
        {
            image.sprite = arrowSprite;
            image.GetComponent<Image>().color = new Color32(0, 255, 0, 50);     //Red arrow will be less opaque if not yet in range

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

        damagePlayer = false;

        originalRotation = image.transform.rotation;

        Debug.Log(arrowDirection);
        Debug.Log(arrowColor);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * movementSpeed, Space.World);

        if (inRange == true && arrowColor == Color.Red)
        {
            image.GetComponent<Image>().color = new Color32(255, 0, 0, 100);
        }
        else if (inRange == true && arrowColor == Color.Green && rotating == 0)
        {
            image.GetComponent<Image>().color = new Color32(0, 255, 0, 100);
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

    public void DoDeath()
    {
        SpawnManager.Instance.RemoveEnemyFromList(gameObject);
        Destroy(gameObject);
    }
}