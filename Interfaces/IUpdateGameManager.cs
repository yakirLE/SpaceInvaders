using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces
{
    public interface IUpdateGameManager
    {
        event EventHandler<ScoreChangedEventArgs> ScoreChanged;
        
        event EventHandler<EventArgs> GameOver;

        event EventHandler<EventArgs> LevelPassed;

        event EventHandler<EventArgs> RemoveMeAsNotifier;

        event EventHandler<GameManagerEventArgs> PlayerDown;

        void NotifyGameManagerAboutMe();
    }
}
