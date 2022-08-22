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
                HandleMissedStack();
                return;
            }

            LastStackTransform.GetComponent<Stack>().MyState = StackState.WaitingForThePlayer;

            if (playerFollowStackXMax < lastStackXMax + stackMatchTolerance &&
                playerFollowStackXMin > lastStackXMin - stackMatchTolerance)
            {
                HandleStackMatch();
                StackActivator.Instance.ActivateNewStack(PlayerFollowStackTransform.localScale, false);
                return;
            }
            
            HandleStackHalfMatch(playerFollowStackXMax, playerFollowStackXMin, lastStackXMax, lastStackXMin, playerFollowStackBounds);
        }

        private void HandleStackHalfMatch(float playerFollowStackXMax, float playerFollowStackXMin, float lastStackXMax,
            float lastStackXMin, Bounds playerFollowStackBounds)
        {
            var matchXMin = Mathf.Max(playerFollowStackXMin, lastStackXMin);
            var matchXMax = Mathf.Min(playerFollowStackXMax, lastStackXMax);

            var lastStackPos = SetLastStackPos(matchXMin, matchXMax);

            var matchSize = matchXMax - matchXMin;

            var playerFollowStackScale = PlayerFollowStackTransform.localScale;
            var newStackScale = SetNewStackScale(playerFollowStackBounds, playerFollowStackScale, matchSize);

            StackActivator.Instance.ActivateNewStack(newStackScale, false);

            float fallingStackXMax, fallingStackXMin;

            if (playerFollowStackXMax > lastStackXMax)
            {
                fallingStackXMax = Mathf.Max(playerFollowStackXMin, lastStackXMin);
                fallingStackXMin = Mathf.Min(playerFollowStackXMin, lastStackXMin);
            }
            else
            {
                fallingStackXMax = Mathf.Max(playerFollowStackXMax, lastStackXMax);
                fallingStackXMin = Mathf.Min(playerFollowStackXMax, lastStackXMax);
            }

            var fallingStackScale = SetFallingStackScale(playerFollowStackBounds, playerFollowStackScale, fallingStackXMax, fallingStackXMin);

            CreateAndSetFallingStack(fallingStackScale, fallingStackXMax, fallingStackXMin, lastStackPos);

            stackMatchCounter = 0;
        }

        private Vector3 SetLastStackPos(float matchXMin, float matchXMax)
        {
            var lastStackPos = LastStackTransform.position;
            LastStackTransform.position = new Vector3()
            {
                x = (matchXMin + matchXMax) / 2f,
                y = lastStackPos.y,
                z = lastStackPos.z
            };
            return lastStackPos;
        }

        private void CreateAndSetFallingStack(Vector3 fallingStackScale, float fallingStackXMax, float fallingStackXMin, Vector3 lastStackPos)
        {
            var fallingStackTransform = StackActivator.Instance.ActivateNewStack(fallingStackScale, true);
            var fallingStack = fallingStackTransform.GetComponent<Stack>();

            fallingStack.MyState = StackState.Falling;
            fallingStack.MyMeshRenderer.material = PlayerFollowStackTransform.GetComponent<Stack>().MyMeshRenderer.material;

            fallingStackTransform.position = new Vector3()
            {
                x = (fallingStackXMax + fallingStackXMin) / 2f,
                y = lastStackPos.y,
                z = lastStackPos.z
            };
        }

        private Vector3 SetNewStackScale(Bounds playerFollowStackBounds, Vector3 playerFollowStackScale, float matchSize)
        {
            var newStackScale = playerFollowStackScale;
            newStackScale.x = playerFollowStackScale.x * matchSize / playerFollowStackBounds.size.x;

            LastStackTransform.localScale = newStackScale;
            return newStackScale;
        }

        private static Vector3 SetFallingStackScale(Bounds playerFollowStackBounds, Vector3 playerFollowStackScale,
            float fallingStackXMax, float fallingStackXMin)
        {
            var fallingStackScale = playerFollowStackScale;
            fallingStackScale.x = playerFollowStackScale.x * (fallingStackXMax - fallingStackXMin) /
                                  playerFollowStackBounds.size.x;
            return fallingStackScale;
        }

        private void HandleMissedStack()
        {
            LastStackTransform.GetComponent<Stack>().MyState = StackState.Falling;
        }

        private void HandleStackMatch()
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