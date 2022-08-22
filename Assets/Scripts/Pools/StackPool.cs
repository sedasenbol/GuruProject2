using System.Collections.Generic;
using Game;
using ScriptableObjects;
using UnityEngine;

namespace Pools
{
    public class StackPool : Singleton<StackPool>
    {
        [SerializeField] private PoolSettingsScriptableObject poolSettings;
        [SerializeField] private Transform containerTransform;
        
        private Material[] materialsArray;
        private Queue<GameObject> stackPoolQueue;

        public Transform SpawnFromPool(Vector3 position, Quaternion rotation)
        {
            var dequeuedStack = DequeueObjectFromPool();

            SetStackMaterial(dequeuedStack);

            var dequeuedStackTransform = dequeuedStack.transform;
            dequeuedStackTransform.position = position;
            dequeuedStackTransform.rotation = rotation;
            
            dequeuedStackTransform.SetParent(containerTransform);

            stackPoolQueue.Enqueue(dequeuedStack);
            
            return dequeuedStackTransform;
        }

        private GameObject DequeueObjectFromPool()
        {
            var dequeuedStack = stackPoolQueue.Dequeue();
            dequeuedStack.SetActive(true);
            return dequeuedStack;
        }

        private void SetStackMaterial(GameObject dequeuedStack)
        {
            var randomMaterial = materialsArray[Random.Range(0, materialsArray.Length)];
            dequeuedStack.GetComponent<Stack>().Reset(randomMaterial);
        }

        private void Start()
        {
            stackPoolQueue = new Queue<GameObject>(poolSettings.PoolSize);

            containerTransform = Instantiate(containerTransform);
            
            LoadMaterials();
            
            InitializeItemPool(poolSettings.PoolSize, stackPoolQueue);
        }

        private void InitializeItemPool(int poolSize, Queue<GameObject> newStackPool)
        {
            for (var j = 0; j < poolSize; j++)
            {
                var stackGo = Instantiate(poolSettings.StackPrefab, poolSettings.InitialSpawnPos, Quaternion.identity,
                    containerTransform).gameObject;
                stackGo.SetActive(false);
                
                newStackPool.Enqueue(stackGo);
            }
        }

        private void LoadMaterials()
        {
            materialsArray = Resources.LoadAll<Material>("Materials");
        }
        private void OnDestroy()
        {
            stackPoolQueue = null;
        }
    }
}