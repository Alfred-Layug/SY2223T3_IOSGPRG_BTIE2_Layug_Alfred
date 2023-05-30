using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsCoroutine : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private bool inRange;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CO_Timer());
    }

    private IEnumerator CO_Timer()
    {
        while (!inRange)
        {
            yield return new WaitForSeconds(0.3f);
            image.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        image.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
