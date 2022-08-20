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
        private Queue<GameObject> itemPoolQueue;

        public Transform SpawnFromPool(Vector3 position, Quaternion rotation)
        {
            var objectSpawned = itemPoolQueue.Dequeue();
            objectSpawned.SetActive(true);
            objectSpawned.GetComponent<Stack>().Reset();
            
            var objectSpawnedTransform = objectSpawned.transform;
            objectSpawnedTransform.position = position;
            objectSpawnedTransform.rotation = rotation;
            
            objectSpawnedTransform.SetParent(containerTransform);

            itemPoolQueue.Enqueue(objectSpawned);
            
            return objectSpawnedTransform;
        }

        //Called by LevelManager.cs when a new level loads.
        public void InitializeItemPoolDict()
        {
            itemPoolQueue = new Queue<GameObject>(poolSettings.PoolSize);

            LoadMaterials();
            
            InitializeItemPool(poolSettings.PoolSize, itemPoolQueue);
        }

        private void LoadMaterials()
        {
            materialsArray = Resources.LoadAll<Material>("Materials");
        }

        private void InitializeItemPool(int poolSize, Queue<GameObject> newItemPool)
        {
            for (var j = 0; j < poolSize; j++)
            {
                var itemGO = Instantiate(poolSettings.ItemPrefab, new Vector3(0f, -100f, 0f), Quaternion.identity,
                    containerTransform).gameObject;
                itemGO.GetComponent<Renderer>().material = materialsArray[Random.Range(0, materialsArray.Length)];
                itemGO.SetActive(false);
                
                newItemPool.Enqueue(itemGO);
            }
        }

        private void OnDisable()
        {
            itemPoolQueue = null;
        }
    }
}