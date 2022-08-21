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
        [SerializeField] private float stackSpawnMaxZDistanceFromCamera = 40f;

        private Transform mainCamTransform;

        private void Start()
        { 
            mainCamTransform = Camera.main.transform;
        }

        private void OnDestroy()
        {
            mainCamTransform = null;
        }

        public void ActivateNewStack(Vector3 scale)
        {
            if (playerFollowStackTransform.position.z - mainCamTransform.position.z > stackSpawnMaxZDistanceFromCamera)
            {
                LevelManager.Instance.FailLevel();
                return ;
            }
            
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