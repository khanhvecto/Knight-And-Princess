using System.Collections;
using UnityEngine;

public class BossFightFinish : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject boss;
    [SerializeField] protected GameObject block;
    [SerializeField] protected Camera playerCamera;

    protected KnightBossStats bossStats;

    protected void Start()
    {
        this.bossStats = this.boss.GetComponentInChildren<KnightBossStats>();
    }

    void Update()
    {
        if(!this.boss.activeSelf) 
            return;

        if(bossStats.isDead)
        {
            this.block.SetActive(false);
            AudioManager.Instance.PlayBgm(BgmType.Lvl1);
            StartCoroutine(this.zoomInCamera());
        }
    }

    protected IEnumerator zoomInCamera()
    {
        float timer = 0f;
        float zoomTime = 1f;
        while (timer <= zoomTime)
        {
            timer += Time.deltaTime;

            var newOrthographicSize = 7.5f - (2.5f * (timer / zoomTime));
            this.playerCamera.orthographicSize = newOrthographicSize;

            yield return null;
        }

        NavigatorManager.Instance.ShowNavigator("You defeated the boss!");
        gameObject.SetActive(false);
    }
}
