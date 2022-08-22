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
        WillStartToMove,
        MovingLeft,
        WillStop,
        Falling
    }
    
    
    public class Stack : MonoBehaviour
    {
        [SerializeField] private StackSettingsScriptableObject stackSettings;
        [SerializeField] private StackState state = StackState.MovingLeft;
        
        private Transform myTransform;
        private int playerLayer;
        private int finishLayer;
        private bool shouldMove;
        private MeshRenderer myMeshRenderer;
        
        private void Awake()
        {
            myTransform = transform;
            myMeshRenderer = GetComponent<MeshRenderer>();
            
            playerLayer = LayerMask.NameToLayer("Player");
            finishLayer = LayerMask.NameToLayer("Finish");
        }

        private void OnDestroy()
        {
            myTransform = null;
            myMeshRenderer = null;
        }

        private void OnEnable()
        {
            TouchController.OnPlayerTapped += OnPlayerTapped;
        }

        private void OnDisable()
        {
            TouchController.OnPlayerTapped -= OnPlayerTapped;
        }

        private void OnPlayerTapped()
        {
            if (state != StackState.MovingLeft) {return;}

            state = StackState.WillStop;
            
            StartCoroutine(ChangeStateNextFrame());
        }

        private IEnumerator ChangeStateNextFrame()
        {
            yield return null;
            
            state = StackState.WaitingForThePlayer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == finishLayer)
            {
                state = StackState.Falling;
            }
            
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
            switch (state)
            {
                case StackState.MovingLeft:
                    myTransform.position += Vector3.left * (stackSettings.StackSpeed * Time.deltaTime);
                    break;
                case StackState.Falling:
                    myTransform.position += Vector3.down * (stackSettings.StackSpeed * Time.deltaTime);
                    break;
                case StackState.WillStartToMove:
                    state = StackState.MovingLeft;
                    break;
            }
        }

        public void Reset(Material newMaterial)
        {
            state = StackState.WillStartToMove;

            myMeshRenderer.material = newMaterial;
        }
        
        public StackState MyState 
        {
            set => state = value;
        }
        
        public MeshRenderer MyMeshRenderer
        {
            get => MyMeshRenderer = myMeshRenderer;
            private set => myMeshRenderer = value;
        }
    }
}