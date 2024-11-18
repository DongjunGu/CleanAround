using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRange : MonoBehaviour
{
    Animator anim;
    BoxCollider2D col;
    public GameObject VirusObj;
    public GameObject particles;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }
    void OnEnable()
    {
        anim.SetTrigger("Notice");
    }

    public IEnumerator ActiveVirus()
    {
        VirusObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        col.enabled = true;
        yield return new WaitForSeconds(1f);
        VirusObj.SetActive(false);
        col.enabled = false;
    }
    private void OnDisable()
    {
        particles.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.health -= GameManager.instance.maxHealth * 0.35f;
        }
    }
}
