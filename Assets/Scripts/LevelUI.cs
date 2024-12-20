using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    Item[] items;
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }
    public void PopUI()
    {
        GameManager.instance.StopGame();
        GameManager.instance.joystickUI.localScale = Vector3.zero;
        GameManager.instance.stopButton.SetActive(false);
        Next();
        GameManager.instance.isLive = false;
        StartCoroutine(WaitAndScale(0.1f, Vector3.one, 0.2f));
    }
    public void HideUI()
    {
        GameManager.instance.isLive = true;
        GameManager.instance.stopButton.SetActive(true);
        StartCoroutine(WaitAndScale(0.1f, Vector3.zero, 0.2f));
        GameManager.instance.joystickUI.localScale = Vector3.one;

    }

    public IEnumerator WaitAndScale(float waitTime, Vector3 targetScale, float duration)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        yield return StartCoroutine(ScaleOverTime(targetScale, duration));
    }
    public IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = rect.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rect.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        rect.localScale = targetScale;
        if(GameManager.instance.isLive)
            GameManager.instance.ResumeGame();
    }

    void Next()
    {
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        //랜덤3개
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = RandomItem(0, items.Length, new int[] { 10, 12, 13, 14 });
            ran[1] = RandomItem(0, items.Length, new int[] { 10, 12, 13, 14 });
            ran[2] = RandomItem(0, items.Length, new int[] { 10, 12, 13, 14 });

            //RobotVacuum
            if (items[4].level == items[4].data.damages.Length && items[8].level > 0
                && items[10].level != items[10].data.damages.Length)
                ran[0] = 10;
            //Vacuum
            if (items[0].level == items[0].data.damages.Length && items[9].level > 0
                && items[12].level != items[12].data.damages.Length)
                ran[0] = 12;
            //RedDuster
            if (items[5].level == items[5].data.damages.Length && items[11].level > 0
                && items[13].level != items[13].data.damages.Length)
                ran[0] = 13;
            //ToxicAcid
            if (items[7].level == items[7].data.damages.Length && items[2].level > 0
                && items[14].level != items[14].data.damages.Length)
                ran[0] = 14;
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }
        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];
            //만렙일경우
            if (ranItem.level == ranItem.data.damages.Length)
            {                
                items[3].gameObject.SetActive(true); //Posion
            }
            else
            {
                //만약 0 1 둘다 만렙일경우 특수 아이템활성화
                ranItem.gameObject.SetActive(true);
            }
        }
    }
    int RandomItem(int min, int max, int[] excludes)
    {
        int randomIndex;
        List<int> excludeList = new List<int>(excludes);

        do
        {
            randomIndex = Random.Range(min, max);
        } while (excludeList.Contains(randomIndex));

        return randomIndex;
    }
}