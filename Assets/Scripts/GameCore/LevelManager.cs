using System;
using System.Collections;
using DG.Tweening;
using Pools;
using ScriptableObjects;
using UnityEngine;


namespace GameCore
{
    public class LevelManager : Singleton<LevelManager>
    {
        public static event Action OnNewLevelLoaded;
        public static event Action OnLevelFailed;
        public static event Action OnLevelCompleted;
        
        // Called by GameManager.cs when "Game" scene is loaded. 
        public void HandleNewLevel()
        {
            StackPool.Instance.InitializeItemPoolDict();
            
            OnNewLevelLoaded?.Invoke();
        }

        public void FailLevel()
        {
            DOTween.CompleteAll();
            OnLevelFailed?.Invoke();
        }
        
        public void CompleteLevel()
        {
            DOTween.CompleteAll();
            OnLevelCompleted?.Invoke();
        }
    }
}
