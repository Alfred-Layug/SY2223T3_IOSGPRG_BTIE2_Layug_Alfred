using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTypes : Singleton<CharacterTypes>
{
    public int characterType;

    public void ChooseDefaultCharacter()
    {
        characterType = 0;
    }

    public void ChooseTankCharacter()
    {
        characterType = 1;
    }

    public void ChooseSpeedCharacter()
    {
        characterType = 2;
    }
}
