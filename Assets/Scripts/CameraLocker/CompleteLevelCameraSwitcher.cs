using System;
using Cinemachine;
using GameCore;
using UnityEngine;

namespace CameraLocker
{
    public class CompleteLevelCameraSwitcher : MonoBehaviour
    {
        [SerializeField] private Transform gameVirtualCameraTransform;
        [SerializeField] private CinemachineVirtualCamera completeLevelVirtualCamera;
        [SerializeField] private int priorityOnCompletedLevel = 11;
        [SerializeField] private int priorityOnNewLevel = 9;
        [SerializeField] private float rotatingSpeed = 5f;
        
        private Transform completeLevelVirtualCameraTransform;

        private bool rotate;
        private Transform myTransform;
        
        private void Start()
        {
            myTransform = transform;
            completeLevelVirtualCameraTransform = completeLevelVirtualCamera.transform;
        }

        private void OnDestroy()
        {
            myTransform = null;
            completeLevelVirtualCameraTransform = null;
        }

        private void OnEnable()
        {
            LevelManager.OnNewLevelLoaded += OnNewLevelLoaded;
            LevelManager.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            LevelManager.OnNewLevelLoaded -= OnNewLevelLoaded;
            LevelManager.OnLevelCompleted -= OnLevelCompleted;
        }

        private void OnNewLevelLoaded()
        {
            rotate = false;
            
            completeLevelVirtualCamera.Priority = priorityOnNewLevel;
        }

        private void OnLevelCompleted()
        {
            rotate = true;
            
            completeLevelVirtualCameraTransform.position = gameVirtualCameraTransform.position;
            completeLevelVirtualCameraTransform.rotation = gameVirtualCameraTransform.rotation;
            
            completeLevelVirtualCamera.Priority = priorityOnCompletedLevel;
        }

        private void Update()
        {
            if (!rotate) {return;}
            
            myTransform.Rotate(0f, rotatingSpeed * Time.deltaTime, 0f);
        }
    }
}