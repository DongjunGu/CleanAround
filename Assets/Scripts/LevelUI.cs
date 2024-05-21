using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    Item[] items;
    RectTransform rect;
    bool isResume;
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
        isResume = false;
        StartCoroutine(WaitAndScale(0.1f, Vector3.one, 0.5f));
    }
    public void HideUI()
    {
        isResume = true;
        StartCoroutine(WaitAndScale(0.1f, Vector3.zero, 0.5f));
        
    }

    IEnumerator WaitAndScale(float waitTime, Vector3 targetScale, float duration)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        yield return StartCoroutine(ScaleOverTime(targetScale, duration));
    }
    IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
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
        if(isResume)
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
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
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
                //만약 노출되는게 2개라면 만렙이 아닌것들 노출
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
