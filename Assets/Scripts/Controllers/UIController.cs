using MemoryGame.Data;
using MemoryGame.Views;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame.Controllers
{
    public class UIController : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private Slider difficultySlider;
        [SerializeField] private TextMeshProUGUI difficultyText;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button startButton;

        [Header("GUI")]
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI movesCountText;
        [SerializeField] private Button restartButton;

        [Header("Win")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private float winShowTime;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private List<ParticleSystem> fireworks;

        [Header("Lose")]
        [SerializeField] private GameObject losePanel;
        [SerializeField] private float loseShowTime;

        [Header("Leaderboard")]
        [SerializeField] private GameObject leaderboardCanvas;
        [SerializeField] private LeaderboardController leaderboardController;
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private Button goToMenuButton;

        [Space]
        [SerializeField] private GameController gameController;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
            restartButton.onClick.AddListener(RestartGame);
            leaderboardButton.onClick.AddListener(OpenLeaderboard);
            goToMenuButton.onClick.AddListener(CloseLeaderboard);
            difficultySlider.onValueChanged.AddListener(UpdateDifficulty);

            gameController.OnGameFinished += ShowGameFinished;
            gameController.OnTimerValueChanged += UpdateTimer;
            gameController.OnMovesCountChanged += UpdateMovesCount;

            LoadData();
        }

        private void OnDestroy()
        {
            gameController.OnGameFinished -= ShowGameFinished;
            gameController.OnTimerValueChanged -= UpdateTimer;
            gameController.OnMovesCountChanged -= UpdateMovesCount;
        }

        private void StartGame()
        {
            gameCanvas.SetActive(true);
            menuCanvas.SetActive(false);

            gameController.StartGame((int)difficultySlider.value);
        }

        private void RestartGame()
        {
            menuCanvas.SetActive(true);
            gameCanvas.SetActive(false);

            gameController.StopGame();
        }

        #region GameFinish
        private void ShowGameFinished(bool win)
        {
            if (win)
            {
                leaderboardController.UpdateLeaderboard(nameInputField.text, gameController.Score);
                StartCoroutine(WinCoroutine());
            }
            else
            {
                StartCoroutine(LoseCoroutine());
            }
        }

        private IEnumerator WinCoroutine()
        {
            scoreText.text = $"{gameController.Score}";
            winPanel.SetActive(true);
            foreach (ParticleSystem firework in fireworks)
            {
                firework.Play(true);
            }

            yield return new WaitForSeconds(winShowTime);

            winPanel.SetActive(false);
            RestartGame();
        }

        private IEnumerator LoseCoroutine()
        {
            losePanel.SetActive(true);

            yield return new WaitForSeconds(loseShowTime);

            losePanel.SetActive(false);
            RestartGame();
        }
        #endregion

        #region GUI
        private void UpdateTimer(int secondsLeft)
        {
            timerText.text = $"{secondsLeft}";
        }

        private void UpdateMovesCount(int movesCount)
        {
            movesCountText.text = $"{movesCount}";
        }
        #endregion

        #region MenuSettings
        private void UpdateDifficulty(float difficultyValue)
        {
            difficultyText.text = $"{difficultyValue}";
        }
        
        private void SetPlayerName(string playerName)
        {
            if (playerName.Length > 0)
            {
                nameInputField.text = playerName;
            }
        }
        #endregion

        #region Leaderboard
        private void OpenLeaderboard()
        {
            leaderboardCanvas.SetActive(true);
            menuCanvas.SetActive(false);
            gameCanvas.SetActive(false);
        }

        private void CloseLeaderboard()
        {
            menuCanvas.SetActive(true);
            leaderboardCanvas.SetActive(false);
        }
        #endregion

        #region Save
        private void LoadData()
        {
            SaveData data = SaveController.LoadFromJson();

            if (data != null)
            {
                SetPlayerName(data.PlayerName);
                leaderboardController.SetLeaderboardData(data.LeaderboardRows);
            }
        }

        private void SaveData()
        {
            SaveData saveData = new SaveData
            {
                PlayerName = nameInputField.text,
                LeaderboardRows = leaderboardController.LeaderboardRows
            };
            SaveController.SaveIntoJson(saveData);
        }
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveData();
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
        #endregion
    }
}
