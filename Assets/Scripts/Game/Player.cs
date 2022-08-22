using System;
using System.Collections;
using GameCore;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerSettingsScriptableObject playerSettings;
        [SerializeField] private Animator animator;
        
        private bool isGameActive;
        private Transform myTransform;
        private Vector3 smoothDampVelocity;
        private Vector3 startPos;
        private int stackLayer;
        private int finishLayer;
        private Rigidbody rb;
        private GameObject stackUnder;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            myTransform = transform;
            
            startPos = myTransform.position;

            stackLayer = LayerMask.NameToLayer("Stack");
            finishLayer = LayerMask.NameToLayer("Finish");
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
            
            animator.SetBool("run", true);
            animator.SetBool("dance", false);
            
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
            
            animator.SetBool("dance", true);
            animator.SetBool("run", false);
        }

        private void OnLevelFailed()
        {
            isGameActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isGameActive) {return;}

            if (other.gameObject.layer == stackLayer)
            {
                stackUnder = other.gameObject;    
            }
            if (other.gameObject.layer != finishLayer) {return;}
            
            LevelManager.Instance.CompleteLevel();
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

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var myPos = myTransform.position;

            var playerFollowPos = StackActivator.Instance.PlayerFollowStackTransform.position;
            var smoothDampPosX =
                Vector3.SmoothDamp(myPos, playerFollowPos, ref smoothDampVelocity, playerSettings.SmoothDampSmoothTime, 
                playerSettings.SmoothDampMaxSpeed).x;

            var mixedPos = myPos + playerSettings.HorizontalSpeed * Time.deltaTime * Vector3.forward;
            mixedPos.x = smoothDampPosX;

            myTransform.position = mixedPos;
        }
    }
}