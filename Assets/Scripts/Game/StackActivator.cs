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
        [SerializeField] private Transform playerFollowStack;
        [SerializeField] private float stackSpawnMaxZDistanceFromCamera = 40f;

        private Transform mainCamTransform;

        private void Start()
        {
            if (Camera.main != null) {mainCamTransform = Camera.main.transform;}
            else
            {
                
            }
        }

        private void OnDestroy()
        {
            mainCamTransform = null;
        }

        public bool TryActivatingNewStack(Vector3 stackPos)
        {
            if (stackPos.z - mainCamTransform.position.z > stackSpawnMaxZDistanceFromCamera) { return false;}
            
            playerFollowStack = lastStackTransform;
            
            var bounds = lastStackTransform.GetComponent<Renderer>().bounds;
            var oldStackPos = lastStackTransform.position;

            var newStackInitialPos = new Vector3()
            {
                x = stackSettings.StackSpawnXPos,
                y = oldStackPos.y,
                z = oldStackPos.z + bounds.size.z
            };

            lastStackTransform = StackPool.Instance.SpawnFromPool(newStackInitialPos, Quaternion.identity);

            return true;
        }

        public Transform PlayerFollowStack => playerFollowStack;
    }
}