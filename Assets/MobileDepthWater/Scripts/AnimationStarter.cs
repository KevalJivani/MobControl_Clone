namespace Assets.MobileOptimizedWater.Scripts
{
    using UnityEngine;

    public class AnimationStarter : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Motion motionAnim;

        public void Awake()
        {
            animator.Play(motionAnim.name);
        }
    }
}
