using System.Collections.Generic;
using UnityEngine;
using static MemoryGame.Data.SaveData;

namespace MemoryGame.Views
{
    public class LeaderboardController : MonoBehaviour
    {
        [SerializeField] LeaderboardTableView leaderboardTableView;
        [SerializeField] int maxRowsCount;

        public List<LeaderboardRow> LeaderboardRows { get; private set; }

        private void Awake()
        {
            LeaderboardRows = new List<LeaderboardRow>();
        }

        public void SetLeaderboardData(List<LeaderboardRow> rows)
        {
            if (rows != null)
            {
                LeaderboardRows = rows;
                leaderboardTableView.UpdateLeaderboard(LeaderboardRows);
            }
        }

        public void UpdateLeaderboard(string playerName, int score)
        {
            int rowIndex = 0;
            while (rowIndex < LeaderboardRows.Count && score <= LeaderboardRows[rowIndex].Score)
            {
                rowIndex++;
            }

            if (rowIndex >= LeaderboardRows.Count && LeaderboardRows.Count >= maxRowsCount)
            {
                return;
            }

            LeaderboardRow addedLeaderboardRow = new LeaderboardRow { PlayerName = playerName, Score = score };
            LeaderboardRows.Insert(rowIndex, addedLeaderboardRow);

            if (LeaderboardRows.Count > maxRowsCount)
            {
                LeaderboardRows.RemoveAt(LeaderboardRows.Count - 1);
            }

            leaderboardTableView.UpdateLeaderboard(LeaderboardRows);
        }
    }
}
