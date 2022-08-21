using System;
using System.Collections;
using System.Collections.Generic;
using InputHandler;
using ScriptableObjects;
using UnityEngine;

namespace Game
{
    public enum StackState
    {
        BehindThePlayer,
        CollisionWithThePlayer,
        WaitingForThePlayer,
        Moving,
    }
    
    
    public class Stack : MonoBehaviour
    {
        [SerializeField] private StackSettingsScriptableObject stackSettings;
        [SerializeField] private StackState state = StackState.Moving;
        
        private Transform myTransform;
        private int playerLayer;
        private bool shouldMove;
        private MeshRenderer myMeshRenderer;
        
        private void Awake()
        {
            myTransform = transform;
            myMeshRenderer = GetComponent<MeshRenderer>();
            
            playerLayer = LayerMask.NameToLayer("Player");
        }

        private void OnDestroy()
        {
            myTransform = null;
            myMeshRenderer = null;
        }

        private void OnEnable()
        {
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                Destroy(rb);
            }
            
            TouchController.OnPlayerTapped += OnPlayerTapped;
        }

        private void OnDisable()
        {
            TouchController.OnPlayerTapped -= OnPlayerTapped;
        }

        private void OnPlayerTapped()
        {
            if (state != StackState.Moving) {return;}

            StartCoroutine(ChangeStateNextFrame());
        }

        private IEnumerator ChangeStateNextFrame()
        {
            yield return null;
            
            state = StackState.WaitingForThePlayer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != playerLayer) {return;}
            
            if (state != StackState.WaitingForThePlayer) {return;}

            state = StackState.CollisionWithThePlayer;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != playerLayer) {return;}
            
            if (state != StackState.CollisionWithThePlayer) {return;}
            
            state = StackState.BehindThePlayer;
        }

        private void Update()
        { 
            if (state != StackState.Moving) {return;}
            
            myTransform.position += stackSettings.StackVelocity * Time.deltaTime;
        }

        public void Reset(Material newMaterial)
        {
            state = StackState.Moving;

            myMeshRenderer.material = newMaterial;
        }
    }
}