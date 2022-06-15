using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows to activating several animations. Each animation have delay for activation.
//First animation will start after first delay finished, second animation start after second delay finished etc.

namespace Game.Common.Animating
{
    public class AnimationDelayedActivation : MonoBehaviour
    {
        [SerializeField] private Animation _animation;
        [SerializeField] private float[] _delays;
        [SerializeField] private string[] _animationNames;

        private float _elapsedTime;
        private int _currentAnimation;

        private void OnEnable()
        {
            _elapsedTime = 0f;
            _currentAnimation = 0;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (_animation == null)
            {
                Debug.LogError($"AnimationDelayedActivation: animation on object {gameObject.name} is not set");
                return;
            }

            if (_animationNames == null)
            {
                Debug.LogError($"AnimationDelayedActivation: animation names on object {gameObject.name} is not set");
                return;
            }

            if (_delays == null)
            {
                Debug.LogError($"AnimationDelayedActivation: animation delays on object {gameObject.name} is not set");
            }
#endif
        }

        private void Update()
        {
            if (_elapsedTime >= _delays[_currentAnimation]) return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _delays[_currentAnimation])
            {
                _animation.Play(_animationNames[_currentAnimation]);

                if (_currentAnimation < _animationNames.Length - 1)
                    _currentAnimation++;
            }
        }
    }
}
