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
        private SoundEffect m_coinPickedUp;
        private Song m_gameBackgroundSong;

        public SoundView(ContentManager content)
        {
            m_playerJump = content.Load<SoundEffect>("Jump");
            m_bombExplode = content.Load<SoundEffect>("BombExplode");
            m_levelCompleted = content.Load<SoundEffect>("LevelCompleted");
            m_coinPickedUp = content.Load<SoundEffect>("CoinPickedUp");
            m_gameBackgroundSong = content.Load<Song>("BackgroundSong");

            MediaPlayer.IsRepeating = true;
        }

        public bool BackGroundSongPlaying
        {
            get;
            set;
        }

        /// <summary>
        /// Start background song
        /// </summary>
        public void StartGameBackgroundSong() 
        {
            MediaPlayer.Play(m_gameBackgroundSong);
            BackGroundSongPlaying = true;
        }

        /// <summary>
        /// Stop background song
        /// </summary>
        public void StopGameBackgroundSong() 
        {
            MediaPlayer.Stop();
            BackGroundSongPlaying = false;
        }

        /// <summary>
        /// Plays bomb explosion if player collide with bomb
        /// </summary>
        public void BombExplode()
        {
            m_bombExplode.Play();
        }

        /// <summary>
        /// Plays jump sound if player jump
        /// </summary>
        public void PlayerJump() 
        {
            m_playerJump.Play();
        }

        /// <summary>
        /// Plays completed sound if player complete level
        /// </summary>
        public void LevelCompleted() 
        {
            m_levelCompleted.Play();
        }

        /// <summary>
        /// Plays coin sound if player pick up coin
        /// </summary>
        public void PlayerPickUpCoin() 
        {
            m_coinPickedUp.Play();
        }
    }
}
