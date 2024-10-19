using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject area;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void SwitchCharacter(string characterName)
    {
        anim.SetTrigger(characterName);
    }
    public static float Health
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }
    public static float Speed
    {
        get { return GameManager.instance.playerId == 2 ? 1.1f : 1f; }
    }
    public static float UIMove
    {
        get { return GameManager.instance.playerId == 1 ? 1.3f : 1f; }
    }

    public void ScaleOrigin()
    {
        GameManager.instance.player.transform.localScale = new Vector3(0.7f, 0.7f, transform.localScale.z);
    }
    public void ScaleChange()
    {
        GameManager.instance.player.transform.localScale = new Vector3(1, 1, transform.localScale.z);
    }
    
    public void AreaScaleChange()
    {        
        area.transform.localScale = new Vector3(1.166f,1.166f, 1f);
    }
    public void OriginAreaScaleChange()
    {
        area.transform.localScale = new Vector3(1.66f, 1.66f, 1f);
    }


}
