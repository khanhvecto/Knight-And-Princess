using System.Collections;
using UnityEngine;

public class BossFightAppear : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject boss;
    [SerializeField] protected Camera playerCamera;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==7)
        {
            this.boss.SetActive(true);
            AudioManager.Instance.PlayBgm(BgmType.Boss);
            StartCoroutine(this.zoomOutCamera());
        }
    }

    protected IEnumerator zoomOutCamera()
    {
        float timer = 0f;
        float zoomTime = 1f;
        while(timer <= zoomTime)
        {
            timer += Time.deltaTime;

            var newOrthographicSize = 5 + (2.5f * (timer / zoomTime));
            this.playerCamera.orthographicSize = newOrthographicSize;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
