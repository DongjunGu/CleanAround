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
        Time.timeScale = 0;
        Next();
        GameManager.instance.isLive = false;
        StartCoroutine(WaitAndScale(0.1f, Vector3.one, 0.2f));
    }
    public void HideUI()
    {
        GameManager.instance.isLive = true;
        StartCoroutine(WaitAndScale(0.1f, Vector3.zero, 0.2f));
        
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
            Time.timeScale = 1;
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
            //리스트로만들어서 랜덤
            //ran[0] = Random.Range(0, items.Length);
            //ran[1] = Random.Range(0, items.Length);
            //ran[2] = Random.Range(0, items.Length);
            ran[0] = RandomItem(0, items.Length, new int[] { 10, 12 });
            ran[1] = RandomItem(0, items.Length, new int[] { 10, 12 });
            ran[2] = RandomItem(0, items.Length, new int[] { 10, 12 });

            if (items[4].level == items[4].data.damages.Length && items[8].level == items[8].data.damages.Length
                && items[10].level != items[10].data.damages.Length)
            {
                Debug.Log("Can R-VacuumActive");
                ran[0] = 10;
            }

            if (items[0].level == items[0].data.damages.Length && items[9].level == items[9].data.damages.Length
                && items[12].level != items[12].data.damages.Length)
            {
                Debug.Log("Can VacuumActive");
                ran[0] = 12;
            }

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
            
        }
        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];
            //만렙일경우
            if (ranItem.level == ranItem.data.damages.Length)
            {                
                items[3].gameObject.SetActive(true); //현재 3 Posion 이라
                //items[Random.Range(4, 7)].gameObject.SetActive(true); 랜덤
            }
            else
            {
                //만약 0 1 둘다 만렙일경우 특수 아이템활성화
                //만약 노출되는게 2개라면 만렙이 아닌것들 노출
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