using UnityEngine;

public class BossFightEnter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject blockEntrance;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            this.blockEntrance.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
