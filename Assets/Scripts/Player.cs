using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    public GameObject building;
    public GameObject soundManager;
    public GameObject uiManager;

    public List<Image> heartSprites;

    public int _playerHealth;
    public int _playerHealthMax;
    public bool isAlive;

    private Vector2 initialTouchPosition;
    private Vector2 endTouchPosition;

    private float dashTimer;
    private float currentDashTimer;
    public bool isDashing;

    private float dashTapTimer;
    private float currentDashTapTimer;
    public bool dashTapActive;

    public void Start()
    {
        building.GetComponent<VideoPlayer>().Play();

        if (CharacterTypes.Instance.characterType == 1)
        {
            _playerHealthMax = 5;
        }
        else
        {
            _playerHealthMax = 3;
        }
        _playerHealth = _playerHealthMax;
        isAlive = true;
        SpawnManager.Instance._playerIsAlive = true;

        UpdateUIHealth();

        transform.position = new Vector2(0.0f, 0.0f);

        dashTimer = 15.0f;
        currentDashTimer = dashTimer;
        isDashing = false;

        dashTapTimer = 1.0f;
        currentDashTapTimer = dashTapTimer;
        dashTapActive = false;

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (!isAlive) return;

        if (_playerHealth > 0 && SpawnManager.Instance.enemies.Count > 0)
        {
            GameObject currentEnemy = SpawnManager.Instance.enemies[0];
            Enemy enemyScript = currentEnemy.GetComponent<Enemy>();

            if (enemyScript.inRange == true && isDashing == false)
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
            else if (enemyScript.inRange == true && isDashing == true)  //Enemy automatically gets killed if player is dashing
            {
                UIManager UIScript = uiManager.GetComponent<UIManager>();

                if (SpawnManager.Instance.enemies.Count < 2)
                {
                    SpawnManager.Instance.SpawnEnemies(1);
                }

                enemyScript.DoDeath();
                soundManager.GetComponent<SoundManager>().PlayEnemyKilledSound();
                UIScript.UpdateScore(2);
            }

            if (enemyScript.damagePlayer == true)    //Player gets damaged if they take too long to swipe or not swipe at all
            {
                TakeDamage();
                enemyScript.DoDeath();
            }
        }

        if (isDashing == true && currentDashTimer > 0)
        {
            currentDashTimer -= Time.deltaTime;
        }
        else if (isDashing == true && currentDashTimer <= 0)
        {
            isDashing = false;
            currentDashTimer = dashTimer;
            Time.timeScale = 1.0f;
        }

        if (dashTapActive == true && currentDashTapTimer > 0)
        {
            currentDashTapTimer -= Time.deltaTime;
        }
        else if (dashTapActive == true && currentDashTapTimer <= 0)
        {
            UIManager UIScript = uiManager.GetComponent<UIManager>();

            currentDashTapTimer = dashTapTimer;
            dashTapActive = false;
            UIScript.dashTapIcon.enabled = false;
            Time.timeScale = 1.0f;
        }
    }

    private void CheckSwipe()
    {
        //Debug.Log("X " + Mathf.Abs(endTouchPosition.x - initialTouchPosition.x));
        //Debug.Log("Y " + Mathf.Abs(endTouchPosition.y - initialTouchPosition.y));
        if (_playerHealth > 0 && SpawnManager.Instance.enemies.Count > 0)
        {
            GameObject currentEnemy = SpawnManager.Instance.enemies[0];
            Enemy enemyScript = currentEnemy.GetComponent<Enemy>();

            if (Mathf.Abs(endTouchPosition.y - initialTouchPosition.y) >
            Mathf.Abs(endTouchPosition.x - initialTouchPosition.x) &&
            Mathf.Abs(endTouchPosition.y - initialTouchPosition.y) >= 480)
            {
                if (initialTouchPosition.y < endTouchPosition.y && enemyScript.arrowDirection == Enemy.Direction.Up)
                {
                    Debug.Log("Swiped Up");
                    enemyScript.DoDeath();
                    GivePlayerRewards();
                }
                else if (initialTouchPosition.y > endTouchPosition.y && enemyScript.arrowDirection == Enemy.Direction.Down)
                {
                    Debug.Log("Swiped Down");
                    enemyScript.DoDeath();
                    GivePlayerRewards();
                }
                else
                {
                    TakeDamage();
                    enemyScript.DoDeath();  //Player gets damaged and can no longer swipe after swiping in the wrong direction
                    Debug.Log("Wrong swipe, you got damaged.");
                }
            }
            else if (Mathf.Abs(endTouchPosition.x - initialTouchPosition.x) >= 480)
            {
                if (initialTouchPosition.x < endTouchPosition.x && enemyScript.arrowDirection == Enemy.Direction.Right)
                {
                    Debug.Log("Swiped Right");
                    enemyScript.DoDeath();
                    GivePlayerRewards();
                }
                else if (initialTouchPosition.x > endTouchPosition.x && enemyScript.arrowDirection == Enemy.Direction.Left)
                {
                    Debug.Log("Swiped Left");
                    enemyScript.DoDeath();
                    GivePlayerRewards();
                }
                else
                {
                    TakeDamage();
                    enemyScript.DoDeath();    //Player gets damaged and can no longer swipe after swiping in the wrong direction
                    Debug.Log("Wrong swipe, you got damaged.");
                }
            }
            else
            {
                Debug.Log("Swipe not counted");
            }
        }
        UIManager UIScript = uiManager.GetComponent<UIManager>();

        soundManager.GetComponent<SoundManager>().StopDashTapSound();
        currentDashTapTimer = dashTapTimer;
        dashTapActive = false;
        UIScript.dashTapIcon.enabled = false;
        Time.timeScale = 1.0f;
    }

    public void GrantExtraHealth()
    {
        int powerUpValue = Random.Range(0, 100);
        Debug.Log(powerUpValue);
        if (powerUpValue <= 3 && _playerHealth < _playerHealthMax)  //Health cannot go beyond the maximum health
        {
            _playerHealth++;
            Debug.Log("Extra Health Earned");
        }

        UpdateUIHealth();
    }

    public void TakeDamage()
    {
        if (_playerHealth > 0)
        {
            _playerHealth--;
            Debug.Log(_playerHealth);
        }
        soundManager.GetComponent<SoundManager>().PlayPlayerHurtSound();
        UpdateUIHealth();
    }

    public void GivePlayerRewards()
    {
        UIManager UIScript = uiManager.GetComponent<UIManager>();

        GrantExtraHealth();
        UIScript.FillDashMeter();
        UIScript.UpdateScore(2);
        soundManager.GetComponent<SoundManager>().PlayEnemyKilledSound();
    }

    public void UpdateUIHealth()
    {
        UIManager UIScript = uiManager.GetComponent<UIManager>();

        for (int i = 0; i < 5; i++)
        {
            if (_playerHealth > i)
            {
                heartSprites[i].enabled = true;
            }
            else
            {
                heartSprites[i].enabled = false;
            }
        }

        if (_playerHealth <= 0)
        {
            UIScript.DoPlayerDeath();
            UIScript.ShowGameOverUI();
        }
    }
}