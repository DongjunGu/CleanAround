using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    public Image targetImage;
    public Sprite resumeSprite;
    public Sprite playSprite;
    public GameObject settingPage;
    public bool isStop = true;

    public void ChangeImage()
    {
        if (isStop)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            targetImage.sprite = playSprite;
            GameManager.instance.StopGame();
            settingPage.SetActive(true);

        }
        else
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            targetImage.sprite = resumeSprite;
            GameManager.instance.ResumeGame();
            settingPage.SetActive(false);
        }
        isStop = !isStop; 
    }
}
