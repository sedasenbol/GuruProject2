using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PoolSettings", menuName = "ScriptableObjects/PoolSettings", order = 1)]
    public class PoolSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private int poolSize;
        [SerializeField] private Transform stackPrefab;
        [SerializeField] private Vector3 initialSpawnPos = Vector3.one * 1000f;
        
        public int PoolSize => poolSize;
        public Transform StackPrefab => stackPrefab;
        public Vector3 InitialSpawnPos => initialSpawnPos;
    }
}
