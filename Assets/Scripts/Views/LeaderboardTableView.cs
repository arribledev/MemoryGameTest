using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MemoryGame.Data.SaveData;

namespace MemoryGame.Views
{
    public class LeaderboardTableView : MonoBehaviour
    {
        [SerializeField] Transform tableRoot;
        [SerializeField] LeaderboardRowView tableRowPrefab;

        private List<LeaderboardRowView> tableRows = new List<LeaderboardRowView>();

        public void UpdateLeaderboard(List<LeaderboardRow> LeaderboardRows)
        {
            if (LeaderboardRows == null)
            {
                return;
            }

            for (int i = tableRows.Count; i < LeaderboardRows.Count; i++)
            {
                tableRows.Add(Instantiate(tableRowPrefab, tableRoot));
            }

            for (int i = 0; i < LeaderboardRows.Count; i++)
            {
                tableRows[i].SetData(i + 1, LeaderboardRows[i].PlayerName, LeaderboardRows[i].Score);
            }
        }
    }
}
