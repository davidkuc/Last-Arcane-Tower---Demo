using UnityEngine;

public class TriggerFirstAnimationFinished : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    public void TriggerFirstAnimFinished()
    {
        animator.SetTrigger("firstAnimFinished");
    }
}
