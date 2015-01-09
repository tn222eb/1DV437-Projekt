using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using GameProject.Controller;
using Microsoft.Xna.Framework.Media;
using GameProject.Model;

namespace GameProject.View
{
    class SoundView : ISoundObserver
    {
        private SoundEffect m_playerJump;
        private SoundEffect m_bombExplode;
        private SoundEffect m_levelCompleted;
        private SoundEffect m_coinPickedUp;
        private Song m_gameBackgroundSong;
        private SoundEffect m_playerFall;
        private Song m_gameBackgroundSong1;
        private Song m_gameBackgroundSong2;
        private Song m_gameBackgroundSong3;
        private GameModel m_gameModel;

        public SoundView(ContentManager content, GameModel gameModel)
        {
            m_gameModel = gameModel;
            m_playerJump = content.Load<SoundEffect>("Jump");
            m_bombExplode = content.Load<SoundEffect>("BombExplode");
            m_levelCompleted = content.Load<SoundEffect>("LevelCompleted");
            m_coinPickedUp = content.Load<SoundEffect>("CoinPickedUp");
            m_gameBackgroundSong1 = content.Load<Song>("BackgroundSong");
            m_gameBackgroundSong2 = content.Load<Song>("BackgroundSong2");
            m_gameBackgroundSong3 = content.Load<Song>("BackgroundSong3");
            m_playerFall = content.Load<SoundEffect>("PlayerFall");

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
            WhatSongToPlay();

            MediaPlayer.Play(m_gameBackgroundSong);
            BackGroundSongPlaying = true;
        }

        private void WhatSongToPlay()
        {
            if (m_gameModel.GetLevel.CurrentLevel == Level.Levels.ONE)
            {
                m_gameBackgroundSong = m_gameBackgroundSong1;
            }

            else if (m_gameModel.GetLevel.CurrentLevel == Level.Levels.TWO)
            {
                m_gameBackgroundSong = m_gameBackgroundSong2;
            }

            else if (m_gameModel.GetLevel.CurrentLevel == Level.Levels.THREE)
            {
                m_gameBackgroundSong = m_gameBackgroundSong3;
            }
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

        /// <summary>
        /// Player falls out of screen
        /// </summary>
        public void PlayerFallInHole() 
        {
            m_playerFall.Play();
        }
    }
}
