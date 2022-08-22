using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace GameCore
{
    public class GameManager : Singleton<GameManager>
    {
        private readonly GameInfo gameInfo = new GameInfo();

        private void Start()
        {
            gameInfo.CurrentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            gameInfo.BestScore = PlayerPrefs.GetInt("BestScore", 0);
            
            gameInfo.CurrentScene = GameInfo.Scene.MainMenu;
            gameInfo.CurrentState = GameInfo.State.Start;
            
            UIManager.Instance.Initialize(gameInfo.BestScore, gameInfo.CurrentLevelIndex);

            LoadGameScene();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.R)) {return;}

            RestartGame();
        }

        private void RestartGame()
        {
            SceneManager.UnloadSceneAsync((int)gameInfo.CurrentScene);

            UIManager.Instance.HandleRestart();
            
            LoadGameScene();
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene((int) GameInfo.Scene.Game, LoadSceneMode.Additive);
        }

        private void OnLevelFailed()
        {
            gameInfo.CurrentState = GameInfo.State.Over;
            
            UIManager.Instance.ShowFailScreen();
        }

        private void OnLevelCompleted()
        {
            gameInfo.CurrentState = GameInfo.State.Success;
            
            UIManager.Instance.ShowSuccessScreen();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene == SceneManager.GetSceneByBuildIndex(0)) {return;}

            SceneManager.SetActiveScene(scene);
            
            gameInfo.CurrentScene = GameInfo.Scene.Game;
            gameInfo.CurrentState = GameInfo.State.Play;
            
            LevelManager.Instance.HandleNewLevel();
        }
        
        private void PauseGame()
        {
            Time.timeScale = 0f;
            
            gameInfo.CurrentState = GameInfo.State.Paused;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            
            gameInfo.CurrentState = GameInfo.State.Play;
        }
        
        private void OnTapToContinueButtonClicked()
        {
            SceneManager.UnloadSceneAsync((int)gameInfo.CurrentScene);
            
            gameInfo.CurrentLevelIndex++;
            
            LoadGameScene();
        }

        private void OnTapToRestartButtonClicked()
        {
            SceneManager.UnloadSceneAsync((int)gameInfo.CurrentScene);
            
            LoadGameScene();
        }

        private void OnBestScoreChanged(int bestScore)
        {
            gameInfo.BestScore = bestScore;
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", gameInfo.CurrentLevelIndex);
            PlayerPrefs.SetInt("BestScore", gameInfo.BestScore);
            PlayerPrefs.Save();
        }
        
        private void OnEnable()
        {
            UIManager.OnPauseButtonClicked += PauseGame;
            UIManager.OnResumeButtonClicked += ResumeGame;
            UIManager.OnTapToContinueButtonClicked += OnTapToContinueButtonClicked;
            UIManager.OnTapToRestartButtonClicked += OnTapToRestartButtonClicked;
            
            LevelManager.OnLevelFailed += OnLevelFailed;
            LevelManager.OnLevelCompleted += OnLevelCompleted;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            UIManager.OnPauseButtonClicked -= PauseGame;
            UIManager.OnResumeButtonClicked -= ResumeGame;
            UIManager.OnTapToContinueButtonClicked -= OnTapToContinueButtonClicked;
            UIManager.OnTapToRestartButtonClicked -= OnTapToRestartButtonClicked;
            
            LevelManager.OnLevelFailed -= OnLevelFailed;
            LevelManager.OnLevelCompleted -= OnLevelCompleted;
            
            SceneManager.sceneLoaded -= OnSceneLoaded;

            SaveData();
        }

        public GameInfo GameInformation => gameInfo;
    }
}
