using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherParticle : MonoBehaviour
{
    public new ParticleSystem particleSystem;
    public static float featherSize =2f;
    public static float sphereRadiusSize;

    CircleCollider2D circleCollider;
    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        sphereRadiusSize = circleCollider.radius;
    }

    void Update()
    {
        ChangeParticleSize(featherSize);
        RadiusSize(sphereRadiusSize);
        HandleRotationInput();

    }
    public void RadiusSize(float size)
    {
        circleCollider.radius = size;
    }
    public void ChangeParticleSize(float size)
    {
        if (particleSystem == null)
        {
            return;
        }
        var mainModule = particleSystem.main;

        mainModule.startSize = size;
    }
    private void HandleRotationInput()
    {
        if (!GameManager.instance.isLive)
            return;
        if (GameManager.instance.player.inputVector.x == -1)
        {
            SetRotation(0f);
        }
        else if (GameManager.instance.player.inputVector.y == -1)
        {
            SetRotation(75f);
        }
        else if (GameManager.instance.player.inputVector.x == 1)
        {
            SetRotation(165f);
        }
        else if (GameManager.instance.player.inputVector.y == 1)
        {
            SetRotation(255f);
        }
    }

    private void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
