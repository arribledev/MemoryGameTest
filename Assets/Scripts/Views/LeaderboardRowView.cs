using TMPro;
using UnityEngine;

namespace MemoryGame.Views
{
    public class LeaderboardRowView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI rowIndexText;
        [SerializeField] TextMeshProUGUI playerNameText;
        [SerializeField] TextMeshProUGUI scoreText;

        public void SetData(int rowIndex, string playerName, int score)
        {
            rowIndexText.text = $"{rowIndex}";
            playerNameText.text = $"{playerName}";
            scoreText.text = $"{score}";
        }
    }
}
