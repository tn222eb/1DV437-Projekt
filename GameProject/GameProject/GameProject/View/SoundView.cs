using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using GameProject.Controller;
using Microsoft.Xna.Framework.Media;

namespace GameProject.View
{
    class SoundView : ISoundObserver
    {
        private SoundEffect m_playerJump;
        private SoundEffect m_bombExplode;
        private SoundEffect m_levelCompleted;
        private Song m_gameBackgroundSong;

        public SoundView(ContentManager content)
        {
            m_playerJump = content.Load<SoundEffect>("Jump");
            m_bombExplode = content.Load<SoundEffect>("BombExplode");
            m_levelCompleted = content.Load<SoundEffect>("LevelCompleted");
            m_gameBackgroundSong = content.Load<Song>("BackgroundSong");

            MediaPlayer.IsRepeating = true;
        }

        public bool BackGroundSongPlaying
        {
            get;
            set;
        }

        public void StartGameBackgroundSong() 
        {
            MediaPlayer.Play(m_gameBackgroundSong);
            BackGroundSongPlaying = true;
        }

        public void StopGameBackgroundSong() 
        {
            MediaPlayer.Stop();
            BackGroundSongPlaying = false;
        }

        public void BombExplode()
        {
            m_bombExplode.Play();
        }

        public void PlayerJump() 
        {
            m_playerJump.Play();
        }

        public void LevelCompleted() 
        {
            m_levelCompleted.Play();
        }
    }
}
