using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Managers
{
    public class SoundManager : GameComponent, ISoundManager
    {
        private const int k_Decrease = -1;
        private const int k_Increase = 1;
        private AudioEngine m_AudioEngine;
        private WaveBank m_WaveBank;
        private SoundBank m_SoundBank;
        private Cue m_Music;
        private bool m_Mute;
        private float m_BackgroundMusicVolume;
        private float m_SoundEffectsVolume;
        private IInputManager m_InputManager;

        public SoundManager(Game i_Game)
            : base(i_Game)
        {
            m_InputManager = i_Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            i_Game.Services.AddService(typeof(ISoundManager), this);
            m_BackgroundMusicVolume = 1;
            m_SoundEffectsVolume = 1;
            m_SoundEffectsVolume = 1;
            i_Game.Components.Add(this);
        }

        public void ToggleMute()
        {
            m_Mute = !m_Mute;
            if(m_Mute)
            {
                m_AudioEngine.GetCategory("Music").SetVolume(0);
                m_AudioEngine.GetCategory("SoundEffects").SetVolume(0);
            }
            else
            {
                m_AudioEngine.GetCategory("Music").SetVolume(m_BackgroundMusicVolume);
                m_AudioEngine.GetCategory("SoundEffects").SetVolume(m_SoundEffectsVolume);
            }
        }

        private void setVolume(string i_CategoryName, float i_Volume)
        {
            if (!m_Mute)
            {
                i_Volume = MathHelper.Clamp(i_Volume, 0, 1);
                m_AudioEngine.GetCategory(i_CategoryName).SetVolume(i_Volume);
            }
        }

        public void SetBackgroundMusicVolume(float i_Volume)
        {
            setVolume("Music", i_Volume);
            m_BackgroundMusicVolume = i_Volume;
        }

        public void SetSoundEffectsVolume(float i_Volume)
        {
            setVolume("SoundEffects", i_Volume);
            m_SoundEffectsVolume = i_Volume;
        }

        public void PlayCue(string i_CueName)
        {
            m_SoundBank.GetCue(i_CueName).Play();
        }

        public override void Initialize()
        {
            m_AudioEngine = new AudioEngine(@"Content\Audio\Win\SpaceInvadersAudio.xgs");
            m_WaveBank = new WaveBank(m_AudioEngine, @"Content\Audio\Win\Wave Bank.xwb");
            m_SoundBank = new SoundBank(m_AudioEngine, @"Content\Audio\Win\Sound Bank.xsb");
            m_Music = m_SoundBank.GetCue("BGMusic");
            m_Music.Play();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            m_AudioEngine.Update();
            if(m_InputManager.IsKeyPressed(Keys.M))
            {
                ToggleMute();
            }

            base.Update(gameTime);
        }
    }
}
