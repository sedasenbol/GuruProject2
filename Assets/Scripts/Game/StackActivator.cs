using System;
using GameCore;
using Input;
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
        [SerializeField] private Transform playerStack;


        public void ActivateNewStack()
        {
            playerStack = lastStackTransform;
            
            var bounds = lastStackTransform.GetComponent<Renderer>().bounds;
            var oldStackPos = lastStackTransform.position;

            var newStackInitialPos = new Vector3()
            {
                x = stackSettings.StackSpawnXPos,
                y = oldStackPos.y,
                z = oldStackPos.z + bounds.size.z
            };

            lastStackTransform = StackPool.Instance.SpawnFromPool(newStackInitialPos, Quaternion.identity);
        }

        public Transform PlayerStack => playerStack;
    }
}