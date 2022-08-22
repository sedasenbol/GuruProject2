using System;
using Game;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "StackSettings", menuName = "ScriptableObjects/StackSettings", order = 1)]
    public class StackSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private float stackSpawnOffset = 10f;

        public float StackSpawnOffset => stackSpawnOffset;
    }
}