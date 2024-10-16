using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void SwitchCharacter(string characterName)
    {
        anim.SetTrigger(characterName);
    }
}
