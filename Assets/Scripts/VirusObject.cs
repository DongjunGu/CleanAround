using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusObject : MonoBehaviour
{
    public GameObject particles;
    public void ActiveParticles()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Explosion);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Explosion);
        particles.SetActive(true);
    }
}
