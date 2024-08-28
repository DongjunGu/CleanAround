using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    protected override void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Dead", false);
        isHit = false;
        health = maxHealth;
        foreach (SpriteRenderer entireRenderer in entireRenderers)
        {
            entireRenderer.color = Color.white;
            entireRenderer.enabled = true;
        }
    }

    void Update()
    {
        
    }

    protected override IEnumerator KnockBack()
    {
        isHit = true;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 knockbackDir = transform.position - playerPos;
        rigid.AddForce(knockbackDir.normalized * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        isHit = false;
    }

    //Àâ¸÷ ¼ÒÈ¯
    
    //ÆòÅ¸

    //ÃÑ¾Ë

    //¹üÀ§
}
