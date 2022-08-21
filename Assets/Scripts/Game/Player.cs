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
        [SerializeField] private float smoothDampMaxSpeed = 1f;
        [SerializeField] private float horizontalSpeed = 1f;
        [SerializeField] private Animator animator;
        
        private bool isGameActive;
        private Transform myTransform;
        private Vector3 smoothDampVelocity;
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
            
            var playerFollowPos = StackActivator.Instance.PlayerFollowStackTransform.position;
            var smoothDampPosX = Vector3.SmoothDamp(myPos, playerFollowPos, ref smoothDampVelocity, smoothDampSmoothTime, smoothDampMaxSpeed).x;

            var mixedPos = myPos + horizontalSpeed * Time.deltaTime * Vector3.forward;
            mixedPos.x = smoothDampPosX;

            myTransform.position = mixedPos;
        }
    }
}