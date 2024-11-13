using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BossController : MonoBehaviour
{
    public GameObject wall;
    public GameObject boss;

    private PixelPerfectCamera pixelPerfectCamera;

    void OnEnable()
    {
        StartCoroutine(BossStageStart());
        Instantiate(GameManager.instance.bossGrid, GameManager.instance.player.transform.position, Quaternion.identity);
    }
    IEnumerator BossStageStart()
    {
        GameManager.instance.player.enabled = false;
        yield return StartCoroutine(CameraMove());

        Instantiate(wall, GameManager.instance.player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        //���� ��ȯ
        GameObject bossObject = Instantiate(boss, GameManager.instance.player.transform.position + Vector3.up * 5f + Vector3.right * 7f, Quaternion.identity);
        bossObject.GetComponent<Boss>().enabled = false;
        bossObject.GetComponent<Animator>().SetTrigger("Spawn");
        //ȭ�� �۰�
        yield return StartCoroutine(CameraMove2());
        bossObject.GetComponent<Boss>().enabled = true;
        bossObject.GetComponent<Animator>().SetBool("Walk", true);
        GameManager.instance.player.enabled = true;
    }

    IEnumerator CameraMove()
    {
        pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();

        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeRefSolX(pixelPerfectCamera.refResolutionX, 300, 0.5f));
        StartCoroutine(ChangeRefSolY(pixelPerfectCamera.refResolutionY, 300, 0.5f));
        yield return StartCoroutine(ChangeAssetsPPUOverTime(pixelPerfectCamera.assetsPPU, 15, 0.5f));
    }
    IEnumerator CameraMove2()
    {
        pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeRefSolX(pixelPerfectCamera.refResolutionX, 250, 0.5f));
        StartCoroutine(ChangeRefSolY(pixelPerfectCamera.refResolutionY, 250, 0.5f));
        yield return StartCoroutine(ChangeAssetsPPUOverTime(pixelPerfectCamera.assetsPPU, 20, 0.5f));
    }
    IEnumerator ChangeAssetsPPUOverTime(int startPPU, int endPPU, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(Mathf.Lerp(startPPU, endPPU, t));

            yield return null;
        }

        pixelPerfectCamera.assetsPPU = endPPU;
    }
    IEnumerator ChangeRefSolX(int startRef, int endRef, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            pixelPerfectCamera.refResolutionX = Mathf.RoundToInt(Mathf.Lerp(startRef, endRef, t));
            yield return null;
        }

        pixelPerfectCamera.refResolutionX = endRef;
    }
    IEnumerator ChangeRefSolY(int startRef, int endRef, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            pixelPerfectCamera.refResolutionY = Mathf.RoundToInt(Mathf.Lerp(startRef, endRef, t));
            yield return null;
        }

        pixelPerfectCamera.refResolutionY = endRef;
    }
}
