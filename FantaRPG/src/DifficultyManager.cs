using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class DifficultyManager
    {
        private static DifficultyManager instance;
        public static DifficultyManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        public readonly GameTime startTime;
        double difficulty = 0;
        private DifficultyManager(GameTime startTime)
        {
            this.startTime = startTime;
            difficulty = 0;
        }
        public static DifficultyManager GetNewDifficultyManager(GameTime startTime)
        {
            DifficultyManager difficultyManager = new(startTime);
            return difficultyManager;
        }
        public void Update(GameTime gameTime)
        {
            difficulty += gameTime.GetElapsedSeconds();
        }
        public void LevelTransferHappened(double extraTimeinSeconds = 0, double extraTimeInSecondsMultiplier = 2.5, double levelTransferMultiplier = 1.2)
        {
            difficulty += extraTimeinSeconds * extraTimeInSecondsMultiplier;
            difficulty *= levelTransferMultiplier;
        }
    }
}
