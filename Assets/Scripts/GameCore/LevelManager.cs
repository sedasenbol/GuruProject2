using System;
using System.Collections;
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

        private bool isGameActive;

        // Called by GameManager.cs when "Game" scene is loaded. 
        public void HandleNewLevel()
        {
            isGameActive = true;
            
            OnNewLevelLoaded?.Invoke();
        }

        public void FailLevel()
        {
            if (!isGameActive) {return;}

            isGameActive = false;
            
            OnLevelFailed?.Invoke();
        }
        
        public void CompleteLevel()
        {
            if (!isGameActive) {return;}

            isGameActive = false;

            OnLevelCompleted?.Invoke();
        }

        public bool IsGameActive => isGameActive;
    }
}
