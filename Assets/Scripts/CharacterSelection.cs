using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectDefaultCharacter()
    {
        CharacterTypes.Instance.ChooseDefaultCharacter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectTankCharacter()
    {
        CharacterTypes.Instance.ChooseTankCharacter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectSpeedCharacter()
    {
        CharacterTypes.Instance.ChooseSpeedCharacter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
