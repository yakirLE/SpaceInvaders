using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public interface IGameManager
    {
        int CurrentLevel { get; set; }

        int LevelRotation { get; set; }

        int PlayersAmount { get; set; }

        int IncreasedPointsPerEnemy { get; set; }

        int RemainderPlusLevelCycle { get; }

        bool WasGameOverInitiated { get; set; }

        void AddObjectToNotify(IPlayer i_PlayerScore);

        void AddObjectToMonitor(IUpdateGameManager i_UpdatePlayer);
    }
}
