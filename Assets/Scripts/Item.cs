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
    public GameObject skillBox;
    public Transform skillImage;
    public GameObject levelDia;
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;
    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        //textLevel = texts[0];
        textName = texts[0];
        textDesc = texts[1];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        //textLevel.text = "Lv." + (level + 1);
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Garbage:
            case ItemData.ItemType.FeatherDuster:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Water:
            case ItemData.ItemType.Detergent:
            case ItemData.ItemType.Iron:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            case ItemData.ItemType.Posion:
            case ItemData.ItemType.Magnetic:
            case ItemData.ItemType.RobotVacuum:
                textDesc.text = string.Format(data.itemDesc);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;


        }
        
    }

    public void OnClick()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level - 1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Garbage:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon =  newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level - 1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.speed -= 0.3f;
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon =  newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level-1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.speed -= 0.1f;
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Water:
                if(level == 0)
                {
                    GameObject newSubItem = new GameObject();
                    subItem = newSubItem.AddComponent<SubItem>();
                    subItem.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextRate = data.damages[level - 1];
                    subItem.LevelUp(nextRate);
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.FeatherDuster:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);
                    LevelUPDia();
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
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Magnetic:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage + data.damages[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    weapon.MagneticUpgrade();
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Detergent:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level - 1];
                    nextCount += data.counts[level - 1];
                    weapon.LevelUp(nextDamage, nextCount);
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Iron:
                if (level == 0)
                {
                    GameObject newSubItem = new GameObject();
                    subItem = newSubItem.AddComponent<SubItem>();
                    subItem.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextRate = data.damages[level];
                    subItem.LevelUp(nextRate);
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Bin:
                if (level == 0)
                {
                    GameObject newSubItem = new GameObject();
                    subItem = newSubItem.AddComponent<SubItem>();
                    subItem.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextRate = data.damages[level];
                    subItem.LevelUp(nextRate);
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.DustPan:
                if (level == 0)
                {
                    GameObject newSubItem = new GameObject();
                    subItem = newSubItem.AddComponent<SubItem>();
                    subItem.Init(data);
                    MakeImage(data);
                    LevelUPDia();
                }
                else
                {
                    float nextRate = data.damages[level];
                    subItem.LevelUp(nextRate);
                    LevelUPDia();
                }
                level++;
                break;
            case ItemData.ItemType.Posion:
                if (GameManager.instance.health >= GameManager.instance.maxHealth * 0.9f)
                    GameManager.instance.health = GameManager.instance.maxHealth;
                else
                    GameManager.instance.health += GameManager.instance.maxHealth * 0.1f;
                break;

            case ItemData.ItemType.RobotVacuum:
            case ItemData.ItemType.Vacuum:
            case ItemData.ItemType.UpgradeFeather:
            case ItemData.ItemType.UpgradeDetergent:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    MakeImage(data);

                }
                level++;
                break;           
        }

        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    void MakeImage(ItemData data) // 획득한 아이템 이미지 추가
    {
        GameObject skills = Instantiate(skillBox, skillImage);
        skills.transform.GetChild(0).GetComponent<Image>().sprite = data.itemIcon; 
    }

    void LevelUPDia() //아이템 레벨 다이아로 표기
    {
        Transform levelUI;
        levelUI = transform.Find("LevelGrid");
        Instantiate(levelDia, levelUI);
    }
}
