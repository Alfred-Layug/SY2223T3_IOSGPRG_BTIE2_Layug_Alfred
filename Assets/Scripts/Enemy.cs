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

    float timer;
    float movementSpeed;

    void Start()
    {
        movementSpeed = 4.0f;
    }

    private void Update()
    {
        startingYPosition = startingYPosition - (Time.deltaTime * movementSpeed);

        transform.position = new Vector2 (startingXPosition, startingYPosition);
    }
}