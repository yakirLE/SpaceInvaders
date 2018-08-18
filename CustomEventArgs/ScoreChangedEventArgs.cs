using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C16_Ex03_Yakir_201049475_Omer_300471430.Enums;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class ScoreChangedEventArgs : GameManagerEventArgs
    {
        public int EnemyPointsWorth { get; set; }

        public bool DidHitEnemy { get; set; }
    }
}
