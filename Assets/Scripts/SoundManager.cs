using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource dashTapSound;
    public AudioSource dashingSound;
    public AudioSource playerHurtSound;
    public AudioSource enemyKilledSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDashTapSound()
    {
        dashTapSound.Play();
    }

    public void StopDashTapSound()
    {
        dashTapSound.Stop();
    }

    public void PlayDashingSound()
    {
        dashingSound.Play();
    }

    public void PlayPlayerHurtSound()
    {
        playerHurtSound.Play();
    }

    public void PlayEnemyKilledSound()
    {
        enemyKilledSound.Play();
    }
}
