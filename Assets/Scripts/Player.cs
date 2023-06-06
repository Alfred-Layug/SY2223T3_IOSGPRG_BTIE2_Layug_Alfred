using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Image dashGauge;
    public Image dashButton;
    public float dashMeterFill;

    public Image firstHeart;
    public Image secondHeart;
    public Image thirdHeart;

    public Image gameOver;
    public TextMeshProUGUI gameOverText;
    public Image retryButton;
    public TextMeshProUGUI retryText;

    public int _playerHealth;
    public int _playerHealthMax;
    private bool isAlive;

    private Vector2 initialTouchPosition;
    private Vector2 endTouchPosition;

    private float dashTimer;
    private float currentDashTimer;
    private bool isDashing;

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
        _playerHealth = 1;
        _playerHealthMax = 3;
        isAlive = true;
        SpawnManager.Instance._playerIsAlive = true;
        SpawnManager.Instance._playerIsDashing = false;

        transform.position = new Vector2(0.0f, 0.0f);

        dashGauge.fillAmount = 0.0f;

        firstHeart.enabled = true; secondHeart.enabled = false; thirdHeart.enabled = false;
        dashButton.enabled = false;

        dashTimer = 5.0f;
        currentDashTimer = dashTimer;
        isDashing = false;

        gameOver.enabled = false;
        gameOverText.enabled = false;
        retryButton.enabled = false;
        retryText.enabled = false;
    }

    private void Update()
    {
        if (!isAlive) return;

        if (_playerHealth <= 0)
        {
            firstHeart.enabled = false; secondHeart.enabled = false; thirdHeart.enabled = false;

            SpawnManager.Instance._playerIsAlive = false;   //Enemies will stop spawning if player is dead

            gameOver.enabled = true;
            gameOverText.enabled = true;
            retryButton.enabled = true;
            retryText.enabled = true;

            DoDeath();

            Debug.Log("Game Over");
        }

        if (_playerHealth == 1)
        {
            firstHeart.enabled = true; secondHeart.enabled = false; thirdHeart.enabled = false;
        }
        else if (_playerHealth == 2)
        {
            secondHeart.enabled = true; thirdHeart.enabled = false;
        }
        else if ( _playerHealth == 3)
        {
            secondHeart.enabled = true; thirdHeart.enabled = true;
        }

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
                if (SpawnManager.Instance.enemies.Count < 2)
                {
                    SpawnManager.Instance.SpawnEnemies(1);
                }
                enemyScript.DoDeath();
            }

            if (enemyScript.damagePlayer == true)    //Player gets damaged if they take too long to swipe or not swipe at all
            {
                TakeDamage();
                enemyScript.DoDeath();
            }
        }

        if (dashGauge.fillAmount >= 1.0f)
        {
            dashButton.enabled = true;
        }

        if (isDashing == true && currentDashTimer > 0.0f)
        {
            currentDashTimer -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            currentDashTimer = dashTimer;
            SpawnManager.Instance._playerIsDashing = false;
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
    }

    public void GrantExtraHealth()
    {
        int powerUpValue = Random.Range(0, 100);
        if (powerUpValue < 3 && _playerHealth < _playerHealthMax)  //Health cannot go beyond the maximum health
        {
            _playerHealth++;
            Debug.Log("Extra Health Earned");
        }
    }

    public void TakeDamage()
    {
        if (_playerHealth > 0)
        {
            _playerHealth--;
            Debug.Log(_playerHealth);
        }
    }

    public void FillDashMeter()
    {
        if (dashGauge.fillAmount < 1.0f)
        {
            dashMeterFill = Random.Range(0.01f, 0.05f);
            dashGauge.fillAmount += dashMeterFill;
        }
    }

    public void GivePlayerRewards()
    {
        GrantExtraHealth();
        FillDashMeter();
    }

    public void ActivateDash()
    {
        if (dashButton.enabled == true)
        {
            isDashing = true;
            dashButton.enabled = false;
            dashGauge.fillAmount = 0.0f;
            SpawnManager.Instance._playerIsDashing = true;
        }
    }

    public void DoDeath()
    {
        isAlive = false;
    }

    public void RetryGame()
    {
        Start();
    }
}