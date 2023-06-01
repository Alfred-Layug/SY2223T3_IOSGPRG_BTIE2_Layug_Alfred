using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        //SpawnManager.Instance.RemoveEnemyFromList(other.gameObject);
    }

    private void Start()
    {
        transform.position = new Vector2(0.0f, 0.0f);
    }

    private void Update()
    {
        
    }
}
