using System;
using GameCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class TouchController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler
    {
        public static event Action<Vector3> OnPlayerDragged;
        public static event Action<Vector3> OnPlayerTapped;

        private UnityEngine.Camera mainCam;
        private Vector3 lastDragWorldPosition;

        private bool isGameActive;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isGameActive) {return;}
            
            lastDragWorldPosition = ConvertScreenToWorldPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isGameActive) {return;}
            
            var currentDragWorldPosition = ConvertScreenToWorldPosition(eventData);
            
            OnPlayerDragged?.Invoke(currentDragWorldPosition - lastDragWorldPosition);

            lastDragWorldPosition = currentDragWorldPosition;
        }

        private Vector3 ConvertScreenToWorldPosition(PointerEventData eventData)
        {
            var screenPos = new Vector3(eventData.position.x, eventData.position.y, mainCam.nearClipPlane);

            return mainCam.ScreenToWorldPoint(screenPos);
        }

        private void SetMainCamera()
        {
            mainCam = UnityEngine.Camera.main;
            
            if (mainCam != null) {return;}
            
            Debug.LogError("Tag the main camera.");
        }

        private void OnNewLevelLoaded()
        {
            isGameActive = true;
        }

        private void OnLevelEnded()
        {
            isGameActive = false;
        }
        
        private void OnEnable()
        {
            SetMainCamera();

            LevelManager.OnNewLevelLoaded += OnNewLevelLoaded;
            LevelManager.OnLevelCompleted += OnLevelEnded;
            LevelManager.OnLevelFailed += OnLevelEnded;
        }

        private void OnDisable()
        {
            mainCam = null;
            
            LevelManager.OnNewLevelLoaded -= OnNewLevelLoaded;
            LevelManager.OnLevelCompleted -= OnLevelEnded;
            LevelManager.OnLevelFailed -= OnLevelEnded;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isGameActive) {return;}

            var currentTapPosition = eventData.pointerCurrentRaycast.worldPosition;
            
            OnPlayerTapped?.Invoke(currentTapPosition);
        }
    }
}
