using System;
using GameCore;
using Pools;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Analytics;

namespace Game
{
    public class StackActivator : Singleton<StackActivator>
    {
        [SerializeField] private StackSettingsScriptableObject stackSettings;
        [SerializeField] private Transform lastStackTransform;
        [SerializeField] private Transform playerFollowStackTransform;

        public Transform ActivateNewStack(Vector3 scale, bool isToFall)
        {
            var newStackInitialPos = FindNewStackInitialPos();

            var newStack = StackPool.Instance.SpawnFromPool(newStackInitialPos, Quaternion.identity);
            newStack.localScale = scale;

            if (isToFall) { return newStack;}
            
            playerFollowStackTransform = lastStackTransform;
            lastStackTransform = newStack; 

            return lastStackTransform;
        }

        private Vector3 FindNewStackInitialPos()
        {
            var bounds = lastStackTransform.GetComponent<Renderer>().bounds;
            var oldStackPos = lastStackTransform.position;

            var newStackInitialPos = new Vector3()
            {
                x = stackSettings.StackSpawnXPos,
                y = oldStackPos.y,
                z = oldStackPos.z + bounds.size.z
            };
            return newStackInitialPos;
        }

        public Transform PlayerFollowStackTransform => playerFollowStackTransform;
        public Transform LastStackTransform => lastStackTransform;
    }
}