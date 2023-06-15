using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public void SelectDefaultCharacter()
    {
        CharacterTypes.Instance.ChooseCharacter(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectTankCharacter()
    {
        CharacterTypes.Instance.ChooseCharacter(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectSpeedCharacter()
    {
        CharacterTypes.Instance.ChooseCharacter(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}