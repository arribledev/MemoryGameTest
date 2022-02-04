using System;
using System.Collections.Generic;

namespace MemoryGame.Data
{
    [Serializable]
    public class SaveData
    {
        public string PlayerName;
        public List<LeaderboardRow> LeaderboardRows;

        public struct LeaderboardRow
        {
            public string PlayerName;
            public int Score;
        }
    }
}
