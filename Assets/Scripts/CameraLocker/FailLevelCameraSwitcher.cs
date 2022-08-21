using System;
using Cinemachine;
using GameCore;
using UnityEngine;

namespace CameraLocker
{
    public class FailLevelCameraSwitcher : MonoBehaviour
    {
        [SerializeField] private Transform gameVirtualCameraTransform;
        [SerializeField] private CinemachineVirtualCamera failVirtualCamera;
        [SerializeField] private int priorityOnFail = 11;
        [SerializeField] private int priorityOnNewLevel = 9;
        [SerializeField] private Vector3 gameVirtualCameraOffset = Vector3.up * 5f;
        
        private Transform myTransform;

        private void Start()
        {
            myTransform = transform;
        }

        private void OnDestroy()
        {
            myTransform = null;
        }

        private void OnEnable()
        {
            LevelManager.OnNewLevelLoaded += OnNewLevelLoaded;
            LevelManager.OnLevelFailed += OnLevelFailed;
        }

        private void OnDisable()
        {
            LevelManager.OnNewLevelLoaded -= OnNewLevelLoaded;
            LevelManager.OnLevelFailed -= OnLevelFailed;
        }

        private void OnNewLevelLoaded()
        {
            failVirtualCamera.Priority = priorityOnNewLevel;
        }

        private void OnLevelFailed()
        {
            myTransform.position = gameVirtualCameraTransform.position + gameVirtualCameraOffset;
            myTransform.rotation = gameVirtualCameraTransform.rotation;
            
            failVirtualCamera.Priority = priorityOnFail;
        }
    }
}