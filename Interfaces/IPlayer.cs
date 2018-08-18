using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C16_Ex03_Yakir_201049475_Omer_300471430.Enums;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces
{
    public interface IPlayer
    {
        int Score { get; set; }

        ePlayerType PlayerType { get; set; }

        bool IsDead { get; set; }

        void UpdateScore(ScoreChangedEventArgs args);

        void NotifyMe();
    }
}
