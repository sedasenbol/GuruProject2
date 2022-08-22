using System;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        public static event Action OnPauseButtonClicked;
        public static event Action OnResumeButtonClicked;
        public static event Action OnTapToRestartButtonClicked;
        public static event Action OnTapToContinueButtonClicked;

        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject resumeButton;
        [SerializeField] private TMP_Text completedTMPText;
        [SerializeField] private GameObject successPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text nextLevelText;
        
        //Called by GameManager.cs when the main scene loads.
        public void Initialize(int bestScore, int currentLevelIndex)
        {
            currentLevelText.text = (currentLevelIndex + 1).ToString("F0");
            nextLevelText.text = (currentLevelIndex + 2).ToString("F0");
        }

        public void ShowFailScreen()
        {
            pauseButton.SetActive(false);
            resumeButton.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        public void ShowSuccessScreen()
        {
            pauseButton.SetActive(false);
            successPanel.SetActive(true);
        }

        public void HandlePauseButtonClick()
        {
            pauseButton.SetActive(false);
            resumeButton.SetActive(true);

            OnPauseButtonClicked?.Invoke();
        }

        public void HandleResumeButtonClick()
        {
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);

            OnResumeButtonClicked?.Invoke();
        }

        public void HandleRestart()
        {
            successPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }
        
        public void HandleTapToContinueClick()
        {
            OnTapToContinueButtonClicked?.Invoke();

            successPanel.SetActive(false);
            pauseButton.SetActive(true);
            currentLevelText.text = (GameManager.Instance.GameInformation.CurrentLevelIndex + 1).ToString("F0");
            nextLevelText.text = (GameManager.Instance.GameInformation.CurrentLevelIndex + 2).ToString("F0");
        }

        public void HandleTapToRestartClick()
        {
            OnTapToRestartButtonClicked?.Invoke();

            gameOverPanel.SetActive(false);
            pauseButton.SetActive(true);
            currentLevelText.text = (GameManager.Instance.GameInformation.CurrentLevelIndex + 1).ToString("F0");
            nextLevelText.text = (GameManager.Instance.GameInformation.CurrentLevelIndex + 2).ToString("F0");
        }

        private void OnEnable()
        {
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
            successPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            completedTMPText.gameObject.SetActive(false);
        }
    }
}
