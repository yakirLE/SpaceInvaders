using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces
{
    public interface ISoundManager
    {
        void ToggleMute();

        void SetBackgroundMusicVolume(float i_Volume);

        void SetSoundEffectsVolume(float i_Volume);

        void PlayCue(string i_CueName);
    }
}
