using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class UnitAnimation : MonoBehaviour
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Move = Animator.StringToHash("Move");
        public static readonly int Attack1 = Animator.StringToHash("Attack1");
        public static readonly int Attack2 = Animator.StringToHash("Attack2");
        public static readonly int Damaged1 = Animator.StringToHash("Damaged1");
        public static readonly int Damaged2 = Animator.StringToHash("Damaged2");
        public static readonly int Death = Animator.StringToHash("Death");
        private static readonly HashSet<int> _animationHashSet = new HashSet<int>()
        {
            Idle,
            Move,
            Attack1,
            Attack2,
            Damaged1,
            Damaged2,
            Death,
        };

        [SerializeField] private UnitObject _unit;
        [SerializeField] private Animator _animator;
        private int _currentState;
        
        private Queue<AnimationStep> _animationQueue;

        public void Initialize(UnitObject unit, AnimatorOverrideController animatorOverrideController)
        {
            _unit = unit;
            _animationQueue = new Queue<AnimationStep>();
            _animator.runtimeAnimatorController = animatorOverrideController;
        }

        public void AddAnimationStep(int state, float duration)
        {
            _animationQueue.Enqueue(AnimationStep.From(state, duration));
        }

        public void SwitchStateTo(int stateNameHash)
        {
            if (stateNameHash == _currentState) return;
            _animator.CrossFade(stateNameHash, 0, 0);
            _currentState = stateNameHash;
        }
        
        // back to idle on finished animation on default
        public void StartAnimation() => StartCoroutine(PlayAnimation());
        public void StartAnimation(Action callback) => StartCoroutine(PlayAnimation(callback));
        public IEnumerator PlayAnimation() => PlayAnimation(null);
        public IEnumerator PlayAnimation(Action callback)
        {
            yield return PlayAnimationStep();
            SwitchStateTo(Idle);
            callback?.Invoke();
        }

        private IEnumerator PlayAnimationStep()
        {
            if (_animationQueue.Count == 0) yield break;
            AnimationStep animationStep = _animationQueue.Dequeue();
            SwitchStateTo(animationStep.stateNameHash);
            yield return new WaitForSecondsRealtime(animationStep.duration);
            yield return PlayAnimationStep();
        }
        
        public IEnumerator MoveUnitOnBroad(Vector2 targetCoord, float time)
        {
            Vector2 startPosition = _unit.location;
            Vector2 currentPosition = startPosition;
            RaycastHit hit;
            float startTime = Time.time;

            while (Vector2.Distance(targetCoord, currentPosition) > 0.01f)
            {
                currentPosition = Vector2.Lerp(startPosition, targetCoord, (Time.time - startTime)/time);
                Physics.Raycast(new Ray(new Vector3(currentPosition.x + .5f, 10, currentPosition.y + .5f), Vector3.down), out hit, 20f);
                _unit.transform.position = currentPosition.GameV2ToV3() + hit.point.ExtractY();
                
                yield return null;
            }
        
            Physics.Raycast(new Ray(new Vector3(currentPosition.x + .5f, 10, currentPosition.y + .5f), Vector3.down), out hit, 20f);
            _unit.transform.position = currentPosition.GameV2ToV3() + hit.point.ExtractY();
            yield return null;
        }

        private struct AnimationStep
        {
            // use UnitAnimation.HASEDSTATE
            public int stateNameHash { get; private set; }
            public float duration { get; private set; }

            public static AnimationStep From(int _state, float _duration) => new AnimationStep()
            {
                stateNameHash = _state,
                duration = _duration,
            };
        }
    }
}