using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "StackSettings", menuName = "ScriptableObjects/StackSettings", order = 1)]
    public class StackSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private Vector3 stackVelocity = Vector3.left * 5f;
        [SerializeField] private float stackSpawnXPos = 10f;
        
        public Vector3 StackVelocity => stackVelocity;
        public float StackSpawnXPos => stackSpawnXPos;
    }
}