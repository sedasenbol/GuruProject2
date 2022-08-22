using System;
using Pools;
using ScriptableObjects;
using UnityEngine;

namespace Game
{
    public class StackSpeedSetter : Singleton<StackSpeedSetter>
    {
        [SerializeField] private PoolSettingsScriptableObject stackPoolSettings;
        [SerializeField] private PlayerSettingsScriptableObject playerSettings;
        [SerializeField] private StackSettingsScriptableObject stackSettings;
        
        private float stackSpeed;
        
        private void Start()
        {
            var oneStackDuration = stackPoolSettings.StackPrefab.GetComponent<Renderer>().bounds.size.z /
                                   playerSettings.HorizontalSpeed;
            stackSpeed = stackSettings.StackSpawnOffset / oneStackDuration;
        }

        public float StackSpeed => stackSpeed;
    }
}