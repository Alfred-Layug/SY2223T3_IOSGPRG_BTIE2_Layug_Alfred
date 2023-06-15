using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject soundManager;

    public Image dashGauge;
    public Image dashButton;
    public Image dashTapButton;
    public float dashMeterFill;

    public Image gameOver;
    public TextMeshProUGUI gameOverText;
    public Image retryButton;
    public TextMeshProUGUI retryText;
    public Image dashTapIcon;
    public Image changeCharacter;
    public TextMeshProUGUI changeCharacterText;
    public Image firstCharacterOption;
    public Image secondCharacterOption;
    public TextMeshProUGUI firstCharacterText;
    public TextMeshProUGUI secondCharacterText;
    public Image quitButton;
    public TextMeshProUGUI quitText;

    public TextMeshProUGUI scoreText;
    public int score;
    public Image tankCharacterIcon;
    public Image speedCharacterIcon;

    // Start is called before the first frame update
    void Start()
    {
        dashGauge.fillAmount = 0.0f;
        dashButton.enabled = false;

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

        score = 0;
        scoreText.text = "Score: " + score;

        if (CharacterTypes.Instance.characterType == 2)
        {
            dashMeterFill = 0.1f;
        }
        else
        {
            dashMeterFill = 0.05f;
        }

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
    }

    // Update is called once per frame
    void Update()
    {
        if (dashGauge.fillAmount >= 1.0f)
        {
            dashButton.enabled = true;
        }
    }

    public void FillDashMeter()
    {
        if (dashGauge.fillAmount < 1.0f)
        {
            dashGauge.fillAmount += dashMeterFill;
        }
    }

    public void ActivateDash()
    {
        Player playerScript = player.GetComponent<Player>();

        if (dashButton.enabled == true && playerScript.dashTapActive == false && playerScript.isAlive == true)
        {
            playerScript.isDashing = true;
            dashButton.enabled = false;
            dashGauge.fillAmount = 0.0f;
            Time.timeScale = 3.0f;
            soundManager.GetComponent<SoundManager>().PlayDashingSound();
        }
    }

    public void ShowGameOverUI()
    {
        Player playerScript = player.GetComponent<Player>();
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
        quitText.enabled = true;
    }

    public void DoPlayerDeath()
    {
        Player playerScript = player.GetComponent<Player>();

        playerScript.isAlive = false;

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

        playerScript.building.GetComponent<VideoPlayer>().Pause();
    }

    public void UpdateScore(int pointsEarned)
    {
        score += pointsEarned;
        scoreText.text = "Score: " + score;
    }

    public void RetryGame()
    {
        Player playerScript = player.GetComponent<Player>();

        playerScript.Start();
        Start();
    }

    public void DoDashTap()
    {
        Player playerScript = player.GetComponent<Player>();

        if (playerScript.isDashing == false && playerScript.dashTapActive == false && playerScript.isAlive == true)     //Player must not be able to dash tap if currently dashing
        {
            if (SpawnManager.Instance.enemies.Count > 0 && playerScript.isAlive == true)
            {
                GameObject currentEnemy = SpawnManager.Instance.enemies[0];
                Enemy enemyScript = currentEnemy.GetComponent<Enemy>();

                if (enemyScript.inRange == false)
                {
                    UpdateScore(1);
                }
            }
            else if (SpawnManager.Instance.enemies.Count == 0 && playerScript.isAlive == true)
            {
                UpdateScore(1);
            }

            soundManager.GetComponent<SoundManager>().PlayDashTapSound();
            Time.timeScale = 2.0f;
            playerScript.dashTapActive = true;
            dashTapIcon.enabled = true;
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