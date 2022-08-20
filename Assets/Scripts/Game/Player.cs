using System;
using GameCore;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float smoothDampSmoothTime = 0.3f;
        [SerializeField] private float maxSpeed = 1f;
        [SerializeField] private Vector3 targetPosOffset = Vector3.forward * 10f;

        private bool isGameActive;
        private Transform myTransform;
        private Vector3 velocity;
        private Vector3 startPos;
        
        private void Start()
        {
            myTransform = transform;
            startPos = myTransform.position;
        }

        private void OnDestroy()
        {
            myTransform = null;
        }

        private void OnEnable()
        {
            LevelManager.OnNewLevelLoaded += OnNewLevelLoaded;
            LevelManager.OnLevelCompleted += OnLevelCompleted;
            LevelManager.OnLevelFailed += OnLevelFailed;
        }
        
        private void OnDisable()
        {
            LevelManager.OnNewLevelLoaded -= OnNewLevelLoaded;
            LevelManager.OnLevelCompleted -= OnLevelCompleted;
            LevelManager.OnLevelFailed -= OnLevelFailed;
        }

        private void OnNewLevelLoaded()
        {
            isGameActive = true;
            
            myTransform.position = startPos;
        }

        private void OnLevelCompleted()
        {
            isGameActive = false;
        }

        private void OnLevelFailed()
        {
            isGameActive = false;
        }

        private void Update()
        {
            if (!isGameActive) {return;}

            var myPos = myTransform.position;
            var targetPos = StackActivator.Instance.PlayerStack.position;

            targetPos.y = myPos.y;
            
            myTransform.position = Vector3.SmoothDamp(myPos, targetPos + targetPosOffset,
                ref velocity, smoothDampSmoothTime, maxSpeed);
        }
    }
}