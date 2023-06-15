using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTypes : Singleton<CharacterTypes>
{
    public int characterType;

    public void ChooseCharacter(int characterNumber)
    {
        characterType = characterNumber;
    }
}