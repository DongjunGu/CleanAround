using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubItem : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name = "SubItem " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplySub();
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
        }
    }
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate);
                    break;
                case 1:
                    weapon.speed = 0.3f * (1f - rate);
                    break;
            }
        }
    }
    void SpeedUp()
    {
        float speed = 5;
        GameManager.instance.player.playerSpeed = speed + speed * rate;
    }
}
