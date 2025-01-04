using System.Collections;
using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Animator animator;

    protected void Awake()
    {
        this.LoadReferences();
    }
    protected void LoadReferences()
    {
        this.animator = GetComponent<Animator>();
    }

    public IEnumerator StartLoad()
    {
        this.animator.SetBool("Changing", true);
        yield return new WaitForSeconds(this.animator.GetCurrentAnimatorClipInfo(0).Length);
    }

    public IEnumerator EndLoad()
    {
        this.animator.SetBool("Changing", false);
        yield return new WaitForSeconds(this.animator.GetCurrentAnimatorClipInfo(0).Length);
    }
}