using UnityEngine;

namespace Sonosthesia
{
    public class AnimationTriggerOnEnable : MonoBehaviour
    {
        [SerializeField] private string _triggerName;

        [SerializeField] private Animator _animator;

        protected virtual void OnEnable()
        {
            if (!_animator)
            {
                return;
            }
            
            _animator.SetTrigger(_triggerName);
        }
    }    
}


