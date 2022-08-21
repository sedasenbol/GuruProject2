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

        public void ActivateNewStack(Vector3 scale)
        {
            playerFollowStackTransform = lastStackTransform;
            
            var bounds = lastStackTransform.GetComponent<Renderer>().bounds;
            var oldStackPos = lastStackTransform.position;

            var newStackInitialPos = new Vector3()
            {
                x = stackSettings.StackSpawnXPos,
                y = oldStackPos.y,
                z = oldStackPos.z + bounds.size.z
            };

            lastStackTransform = StackPool.Instance.SpawnFromPool(newStackInitialPos, Quaternion.identity);
            lastStackTransform.localScale = scale;
        }

        public Transform PlayerFollowStackTransform => playerFollowStackTransform;
        public Transform LastStackTransform => lastStackTransform;
    }
}