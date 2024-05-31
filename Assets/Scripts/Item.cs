using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public SubItem subItem;
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;
    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Garbage:
            case ItemData.ItemType.FeatherDuster:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Water:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            case ItemData.ItemType.Posion:
            case ItemData.ItemType.Magnetic:
                textDesc.text = string.Format(data.itemDesc);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;


        }
        
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level - 1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case ItemData.ItemType.Garbage:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon =  newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level - 1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.speed -= 0.3f;
                }
                level++;
                break;
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon =  newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level-1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.speed -= 0.1f;
                }
                level++;
                break;
            case ItemData.ItemType.Water:
                if(level == 0)
                {
                    GameObject newSubItem = new GameObject();
                    subItem = newSubItem.AddComponent<SubItem>();
                    subItem.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level - 1];
                    subItem.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.FeatherDuster:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level - 1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.speed -= 0.5f;
                    FeatherParticle.featherSize += 0.3f;
                    FeatherParticle.sphereRadiusSize += 0.3f;
                }
                level++;
                break;
            case ItemData.ItemType.Magnetic:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage + data.damages[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.MagneticUpgrade();
                }
                level++;
                break;
            case ItemData.ItemType.Posion:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
