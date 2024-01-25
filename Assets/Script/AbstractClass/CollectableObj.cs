using System.Collections;
using UnityEngine;

/*This should be place at the 1st child of root object*/

public abstract class CollectableObj : MonoBehaviour
{
    [Header("--- COLLECTABLE OBJECT ---")]
    [Header("Stats")]
    protected float collectSpeed = 2.5f;
    protected float minDistance = 0.6f;

    [Header("States")]
    protected bool beingCollected = false;
    protected bool collected = false;
    protected bool afterCollecting = false;

    [Header("Targeting")]
    protected Collider2D targetCollider;

    protected virtual void Update()
    {
        if (!beingCollected) return;

        this.MoveObj();
        this.CheckCollectObj();
    }

    protected virtual void MoveObj()    //Move obj closer to knight
    {
        Vector3 newPos = this.targetCollider.transform.position;
        transform.parent.position = Vector3.Slerp(transform.position, newPos, collectSpeed * Time.deltaTime);
    }

    protected virtual void CheckCollectObj()  //If close enough, knight can collect obj
    {
        Vector3 newPos = this.targetCollider.transform.position;
        if (Vector3.Distance(transform.parent.position, newPos) < this.minDistance && !afterCollecting)
        {
            transform.parent.GetComponent<SpriteRenderer>().enabled = false;    
            StartCoroutine(this.OnCollectObj());
        }
    }

    protected IEnumerator OnCollectObj()
    {
        this.afterCollecting = true;
        yield return StartCoroutine(this.CollectObj());
        transform.parent.gameObject.SetActive(false);
    }

    protected abstract IEnumerator CollectObj();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)    //Knight layer
        {
            this.targetCollider = collision;
            this.beingCollected = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.layer == 7)    //Knight layer
        {
            this.beingCollected = false;
        }
    }
}
