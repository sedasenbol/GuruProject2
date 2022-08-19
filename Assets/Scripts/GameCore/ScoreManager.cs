using System;
using ScriptableObjects;
using UI;
using UnityEngine;

namespace GameCore
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public static event Action<int> OnBestScoreChanged;
    
        private int currentScore;
        private int bestScore;

        //Called by GameManager.cs when the main scene loads.
        public void Initialize(int bestScore)
        {
            this.bestScore = bestScore;
        }

        //Called by BallProgressTracker.cs when a platform group is broken. 
        public void IncreaseScore(int consecutiveBallProgressCounter)
        {
            currentScore += (GameManager.Instance.GameInformation.CurrentLevelIndex + 1) * consecutiveBallProgressCounter;

            CheckBestScore();
            
            UIManager.Instance.UpdateScoreTexts(currentScore, bestScore);
        }

        private void CheckBestScore()
        {
            if (bestScore >= currentScore) {return;}

            bestScore = currentScore;
            OnBestScoreChanged?.Invoke(bestScore);
        }

        //Called by GameManager.cs when the Game scene is loading.
        public void ResetCurrentScore()
        {
            currentScore = 0;
            
            UIManager.Instance.UpdateScoreTexts(currentScore, bestScore);
        }
    }
}
