using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame.Controllers
{
    public class UIController : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private GameObject gameCanvas;

        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button restartButton;

        [Header("MenuSettings")]
        [SerializeField] private Slider difficultySlider;
        [SerializeField] private TextMeshProUGUI difficultyText;
        [SerializeField] private TMP_InputField nameInputField;

        [Header("GUI")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI movesCountText;

        [Header("Win")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private float winShowTime;

        [Header("Lose")]
        [SerializeField] private GameObject losePanel;
        [SerializeField] private float loseShowTime;

        [Space]
        [SerializeField] private GameController gameController;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
            restartButton.onClick.AddListener(RestartGame);
            difficultySlider.onValueChanged.AddListener(UpdateDifficulty);
            LoadPlayerName();

            gameController.OnGameFinished += ShowGameFinished;
            gameController.OnTimerValueChanged += UpdateTimer;
            gameController.OnMovesCountChanged += UpdateMovesCount;
        }

        private void OnDestroy()
        {
            gameController.OnGameFinished -= ShowGameFinished;
            gameController.OnTimerValueChanged -= UpdateTimer;
            gameController.OnMovesCountChanged -= UpdateMovesCount;
        }

        private void StartGame()
        {
            SavePlayerName(nameInputField.text);
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
                StartCoroutine(WinCoroutine());
            }
            else
            {
                StartCoroutine(LoseCoroutine());
            }
        }

        private IEnumerator WinCoroutine()
        {
            winPanel.SetActive(true);
            //fireworks

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
        
        private void LoadPlayerName()
        {
            string name = SaveController.PlayerName;

            if (SaveController.PlayerName.Length > 0)
            {
                nameInputField.text = name;
            }
        }

        private void SavePlayerName(string name)
        {
            SaveController.PlayerName = name;
        }
        #endregion
    }
}
