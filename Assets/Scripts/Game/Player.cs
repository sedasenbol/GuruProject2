using System;
using System.Collections;
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
        private int stackLayer;
        private Rigidbody rb;
        private GameObject stackUnder;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            myTransform = transform;
            
            startPos = myTransform.position;

            stackLayer = LayerMask.NameToLayer("Stack");
        }

        private void OnDestroy()
        {
            rb = null;
            myTransform = null;
            stackUnder = null;
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
            StartCoroutine(ActivateTheGameNextFrame());
                        
            rb.useGravity = false;
            rb.isKinematic = true;
            
            myTransform.position = startPos;
        }

        private IEnumerator ActivateTheGameNextFrame()
        {
            yield return null;
            
            isGameActive = true;
        }

        private void OnLevelCompleted()
        {
            isGameActive = false;
        }

        private void OnLevelFailed()
        {
            isGameActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isGameActive) {return;}
            
            if (other.gameObject.layer != stackLayer) {return;}

            stackUnder = other.gameObject;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isGameActive) {return;}
            
            if (other.gameObject.layer != stackLayer) {return;}

            if (other.gameObject != stackUnder) {return;}
            
            StartFalling();

            LevelManager.Instance.FailLevel();
        }

        private void StartFalling()
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        private void Update()
        {
            if (!isGameActive) {return;}

            var myPos = myTransform.position;
            var targetPos = StackActivator.Instance.PlayerFollowStackTransform.position;

            targetPos.y = myPos.y;
            
            myTransform.position = Vector3.SmoothDamp(myPos, targetPos + targetPosOffset,
                ref velocity, smoothDampSmoothTime, maxSpeed);
        }
    }
}