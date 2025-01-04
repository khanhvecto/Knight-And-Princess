using UnityEngine;

public abstract class TutorialActivate : MonoBehaviour
{
    protected int playerLayer = 7;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == this.playerLayer)
        {
            this.ActivateFeatures();
            gameObject.SetActive(false);
        }
    }

    protected abstract void ActivateFeatures();
}
