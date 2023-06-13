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

    public Image dashGauge;
    public Image dashButton;
    public Image dashTapButton;
    public float dashMeterFill;

    public List<Image> heartSprites;

    public Image gameOver;
    public TextMeshProUGUI gameOverText;
    public Image retryButton;
    public TextMeshProUGUI retryText;
    public TextMeshProUGUI scoreText;
    public Image dashTapIcon;
    public Image changeCharacter;
    public TextMeshProUGUI changeCharacterText;
    public Image firstCharacterOption;
    public Image secondCharacterOption;
    public TextMeshProUGUI firstCharacterText;
    public TextMeshProUGUI secondCharacterText;
    public Image quitButton;
    public TextMeshProUGUI quitText;
    public Image tankCharacterIcon;
    public Image speedCharacterIcon;

    public int _playerHealth;
    public int _playerHealthMax;
    private bool isAlive;

    private Vector2 initialTouchPosition;
    private Vector2 endTouchPosition;

    private float dashTimer;
    private float currentDashTimer;
    private bool isDashing;

    private float dashTapTimer;
    private float currentDashTapTimer;
    private bool dashTapActive;
    private int score;

    private void Start()
    {
        building.GetComponent<VideoPlayer>().Play();

        if (CharacterTypes.Instance.characterType == 1)     //Tank character
        {
            tankCharacterIcon.enabled = true;
            speedCharacterIcon.enabled = false;
        }
        else if (CharacterTypes.Instance.characterType == 2)    //Speed character
        {
            tankCharacterIcon.enabled = false;
            speedCharacterIcon.enabled = true;
        }
        else    //Default character
        {
            tankCharacterIcon.enabled = false;
            speedCharacterIcon.enabled = false;
        }

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

        score = 0;

        UpdateUIHealth();

        transform.position = new Vector2(0.0f, 0.0f);

        dashGauge.fillAmount = 0.0f;
        if (CharacterTypes.Instance.characterType == 2)
        {
            dashMeterFill = 0.1f;
        }
        else
        {
            dashMeterFill = 0.05f;
        }

        dashButton.enabled = false;

        dashTimer = 15.0f;
        currentDashTimer = dashTimer;
        isDashing = false;

        dashTapTimer = 1.0f;
        currentDashTapTimer = dashTapTimer;
        dashTapActive = false;

        Time.timeScale = 1.0f;

        gameOver.enabled = false;
        gameOverText.enabled = false;
        retryButton.enabled = false;
        retryText.enabled = false;
        dashTapIcon.enabled = false;
        changeCharacter.enabled = false;
        changeCharacterText.enabled = false;
        firstCharacterOption.enabled = false;
        secondCharacterOption.enabled = false;
        firstCharacterText.enabled = false;
        secondCharacterText.enabled = false;
        quitButton.enabled = false;
        quitText.enabled = false;

        scoreText.text = "Score: " + score;
    }

    private void Update()
    {
        if (!isAlive) return;

        if (_playerHealth <= 0)
        {

            SpawnManager.Instance._playerIsAlive = false;   //Enemies will stop spawning if player is dead

            gameOver.enabled = true;
            gameOverText.enabled = true;
            retryButton.enabled = true;
            retryText.enabled = true;
            changeCharacter.enabled = true;
            changeCharacterText.enabled = true;
            firstCharacterOption.enabled = true;
            secondCharacterOption.enabled = true;
            firstCharacterText.enabled = true;
            secondCharacterText.enabled = true;
            quitButton.enabled = true;
            quitText .enabled = true;

            DoDeath();

            Debug.Log("Game Over");
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
                soundManager.GetComponent<SoundManager>().PlayEnemyKilledSound();
                score += 2;
                scoreText.text = "Score: " + score;
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
            currentDashTapTimer = dashTapTimer;
            dashTapActive = false;
            dashTapIcon.enabled = false;
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

        soundManager.GetComponent<SoundManager>().StopDashTapSound();
        currentDashTapTimer = dashTapTimer;
        dashTapActive = false;
        dashTapIcon.enabled = false;
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

    public void FillDashMeter()
    {
        if (dashGauge.fillAmount < 1.0f)
        {
            dashGauge.fillAmount += dashMeterFill;
        }
    }

    public void GivePlayerRewards()
    {
        GrantExtraHealth();
        FillDashMeter();
        soundManager.GetComponent<SoundManager>().PlayEnemyKilledSound();
        score += 2;
        scoreText.text = "Score: " + score;
    }

    public void ActivateDash()
    {
        if (dashButton.enabled == true && dashTapActive == false && isAlive == true)
        {
            isDashing = true;
            dashButton.enabled = false;
            dashGauge.fillAmount = 0.0f;
            Time.timeScale = 3.0f;
            soundManager.GetComponent<SoundManager>().PlayDashingSound();
        }
    }

    public void DoDeath()
    {
        isAlive = false;

        if (CharacterTypes.Instance.characterType == 0)
        {
            firstCharacterText.text = "Tank";
            secondCharacterText.text = "Speed";
        }
        else if (CharacterTypes.Instance.characterType == 1)
        {
            firstCharacterText.text = "Default";
            secondCharacterText.text = "Speed";
        }
        else if (CharacterTypes.Instance.characterType == 2)
        {
            firstCharacterText.text = "Default";
            secondCharacterText.text = "Tank";
        }

        building.GetComponent<VideoPlayer>().Pause();
    }

    public void RetryGame()
    {
        Start();
    }

    public void UpdateUIHealth()
    {
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
    }

    public void DoDashTap()
    {
        if (isDashing == false && dashTapActive == false && isAlive == true)     //Player must not be able to dash tap if currently dashing
        {
            if (SpawnManager.Instance.enemies.Count > 0 && isAlive == true)
            {
                GameObject currentEnemy = SpawnManager.Instance.enemies[0];
                Enemy enemyScript = currentEnemy.GetComponent<Enemy>();

                if (enemyScript.inRange == false)
                {
                    score++;
                }
            }
            else if (SpawnManager.Instance.enemies.Count == 0 && isAlive == true)
            {
                score++;
            }

            soundManager.GetComponent<SoundManager>().PlayDashTapSound();
            Time.timeScale = 2.0f;
            dashTapActive = true;
            dashTapIcon.enabled = true;
            scoreText.text = "Score: " + score;
            Debug.Log("Score: " + score);
        }
    }

    public void ChangeToFirstCharacter()
    {
        if (CharacterTypes.Instance.characterType == 0)
        {
            CharacterTypes.Instance.characterType = 1;
        }
        else
        {
            CharacterTypes.Instance.characterType = 0;
        }

        RetryGame();
    }

    public void ChangeToSecondCharacter()
    {
        if (CharacterTypes.Instance.characterType == 0 || CharacterTypes.Instance.characterType == 1)
        {
            CharacterTypes.Instance.characterType = 2;
        }
        else
        {
            CharacterTypes.Instance.characterType = 1;
        }

        RetryGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}