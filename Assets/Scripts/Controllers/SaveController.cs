using UnityEngine;

namespace MemoryGame.Controllers
{
    public static class SaveController
    {
        private static string playerNameKey = "playerName";
        public static string PlayerName
        {
            get
            {
                return PlayerPrefs.GetString("playerNameKey", "");
            }

            set
            {
                PlayerPrefs.SetString("playerNameKey", value);
            }
        }
    }
}
