using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PoolSettings", menuName = "ScriptableObjects/PoolSettings", order = 1)]
    public class PoolSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private int poolSize;
        [SerializeField] private Transform itemPrefab;
    
        public int PoolSize => poolSize;
        public Transform ItemPrefab => itemPrefab;
    }
}
