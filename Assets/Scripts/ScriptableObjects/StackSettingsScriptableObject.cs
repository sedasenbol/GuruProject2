using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "StackSettings", menuName = "ScriptableObjects/StackSettings", order = 1)]
    public class StackSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private float stackSpeed = 5f;
        [SerializeField] private float stackSpawnXPos = 10f;
        
        public float StackSpeed => stackSpeed;
        public float StackSpawnXPos => stackSpawnXPos;
    }
}