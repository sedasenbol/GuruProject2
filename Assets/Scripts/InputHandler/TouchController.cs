using System;
using GameCore;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputHandler
{
    public class TouchController : MonoBehaviour, IPointerDownHandler
    {
        public static event Action OnPlayerTapped;

        private Camera mainCam;
        private Vector3 lastDragWorldPosition;


        private void SetMainCamera()
        {
            mainCam = Camera.main;
            
            if (mainCam != null) {return;}
            
            Debug.LogError("Tag the main camera.");
        }

        private void OnEnable()
        {
            SetMainCamera();
        }

        private void OnDisable()
        {
            mainCam = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!LevelManager.Instance.IsGameActive) {return;}
            
            OnPlayerTapped?.Invoke();
        }
    }
}
