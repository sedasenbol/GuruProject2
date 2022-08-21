using System;
using System.Collections.Generic;
using GameCore;
using InputHandler;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class StackManager : MonoBehaviour
    {
        public static event Action<int> OnStackMatch; 

        [SerializeField] private float stackMatchTolerance = 0.3f;

        private Transform LastStackTransform => StackActivator.Instance.LastStackTransform;
        private Transform PlayerFollowStackTransform => StackActivator.Instance.PlayerFollowStackTransform;

        private int stackMatchCounter;

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
            CheckLastTwoStacksPositions();
        }

        private void CheckLastTwoStacksPositions()
        {
            var lastStackBounds = LastStackTransform.GetComponent<Renderer>().bounds;
            var playerFollowStackBounds = PlayerFollowStackTransform.GetComponent<Renderer>().bounds;

            var lastStackXMax = lastStackBounds.max.x;
            var lastStackXMin = lastStackBounds.min.x;

            var playerFollowStackXMax = playerFollowStackBounds.max.x;
            var playerFollowStackXMin = playerFollowStackBounds.min.x;

            if (playerFollowStackXMin > lastStackXMax || playerFollowStackXMax < lastStackXMin)
            {
                LastStackTransform.GetComponent<Stack>().MyState = StackState.Falling;
                return;
            }

            if (playerFollowStackXMax < lastStackXMax + stackMatchTolerance &&
                playerFollowStackXMin > lastStackXMin - stackMatchTolerance)
            {
                HandleNewStackMatch();
                StackActivator.Instance.ActivateNewStack(PlayerFollowStackTransform.localScale);
                return;
            }
            
            var matchXMin = Mathf.Max(playerFollowStackXMin, lastStackXMin);
            var matchXMax = Mathf.Min(playerFollowStackXMax, lastStackXMax);
            
            var lastStackPos = LastStackTransform.position;
            LastStackTransform.position = new Vector3()
            {
                x = (matchXMin + matchXMax) / 2f,
                y = lastStackPos.y,
                z = lastStackPos.z
            };
            
            var matchSize = matchXMax - matchXMin;

            var playerFollowStackScale = PlayerFollowStackTransform.localScale;
            var newStackScale = playerFollowStackScale;
            newStackScale.x = playerFollowStackScale.x * matchSize / playerFollowStackBounds.size.x;

            LastStackTransform.localScale = newStackScale;

            StackActivator.Instance.ActivateNewStack(newStackScale);

            stackMatchCounter = 0;
        }

        private void HandleNewStackMatch()
        {
            stackMatchCounter++;
            OnStackMatch?.Invoke(stackMatchCounter);
            
            var lastStackPos = LastStackTransform.position;
            
            LastStackTransform.position = new Vector3()
            {
                x = PlayerFollowStackTransform.position.x,
                y = lastStackPos.y,
                z = lastStackPos.z
            };
        }
    }
}