using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    public Image targetImage;
    public Sprite resumeSprite;
    public Sprite playSprite;

    public bool isStop = true;

    public void ChangeImage()
    {
        if (isStop)
        {
            targetImage.sprite = playSprite;
            GameManager.instance.StopGame();
        }
        else
        {
            targetImage.sprite = resumeSprite;
            GameManager.instance.ResumeGame();
        }
        isStop = !isStop; 
    }
}
