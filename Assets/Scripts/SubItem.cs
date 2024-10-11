using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubItem : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    float timer;
    public void Init(ItemData data)
    {
        name = "SubItem " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplySub();
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        switch (type)
        {
            case ItemData.ItemType.Water:
                break;
            case ItemData.ItemType.Iron:
                timer += Time.deltaTime;
                if (timer > 5)
                {
                    HealthRecharge();
                    timer = 0f;
                }
                break;
            default:
                break;
        }
    }
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplySub();
    }
    void ApplySub()
    {
        switch (type)
        {
            case ItemData.ItemType.Water:
                SpeedUp();
                break;
            case ItemData.ItemType.Bin:
                ExpIncrease();
                break;
            case ItemData.ItemType.DustPan:
                MaxHpIncrease();
                break;
        }
    }
    void SpeedUp()
    {
        float tempSpeed = GameManager.instance.player.playerSpeed;
        GameManager.instance.player.playerSpeed = tempSpeed + tempSpeed * rate;
    }
    void HealthRecharge()
    {
        if(GameManager.instance.health < GameManager.instance.maxHealth)
            GameManager.instance.health += rate;
    }
    void ExpIncrease()
    {
        GameManager.instance.increaseExp = rate;
    }
    void MaxHpIncrease()
    {
        GameManager.instance.maxHealth = rate;
    }
}
