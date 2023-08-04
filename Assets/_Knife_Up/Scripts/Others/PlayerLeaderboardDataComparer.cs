using System.Collections;
using System.Collections.Generic;

namespace OnefallGames
{
    public class PlayerLeaderboardDataComparer : IComparer<PlayerLeaderboardData>
    {
        public int Compare(PlayerLeaderboardData x, PlayerLeaderboardData y)
        {
            PlayerLeaderboardData data1 = x;
            PlayerLeaderboardData data2 = y;

            if (data1.Level < data2.Level)
                return 1;
            if (data1.Level > data2.Level)
                return -1;
            else
                return 0;
        }
    }
}
