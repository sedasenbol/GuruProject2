using System;
using GameCore;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private CameraSettingsScriptableObject cameraSettings;

        private Transform myTransform;
        private Transform targetTransform;
        private Vector3 offset;

        private bool shouldFollowTarget;
        
        private void OnNewLevelLoaded()
        {
            myTransform.position = cameraSettings.CameraStartPosition;
            targetTransform = FindObjectOfType<TemplatePlayer>().transform;
            
            shouldFollowTarget = true;
            
            offset = myTransform.position - targetTransform.position;
        }

        private void OnLevelEnded()
        {
            shouldFollowTarget = false;

            targetTransform = null;
        }

        private void LateUpdate()
        {
            if (!shouldFollowTarget) {return;}

            var targetPosition = targetTransform.position + offset;
            var myPosition = myTransform.position;
            
            if (targetPosition.y + cameraSettings.CameraMovementThreshold > myPosition.y) {return;}

            myTransform.position = Vector3.Lerp(myPosition, targetPosition, cameraSettings.CameraLerpRatio);
        }

        private void OnEnable()
        {
            myTransform = transform;
            
            LevelManager.OnNewLevelLoaded += OnNewLevelLoaded;
            LevelManager.OnLevelFailed += OnLevelEnded;
            LevelManager.OnLevelCompleted += OnLevelEnded;
        }

        private void OnDisable()
        {
            myTransform = null;
            
            LevelManager.OnNewLevelLoaded -= OnNewLevelLoaded;
            LevelManager.OnLevelFailed -= OnLevelEnded;
            LevelManager.OnLevelCompleted -= OnLevelEnded;
        }
    }
}
