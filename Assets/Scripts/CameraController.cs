using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _followTarget;
        [SerializeField] private float _followSpeed;
        
        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
        }

        private void FollowTarget()
        {

        }
    }
}