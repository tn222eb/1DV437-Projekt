using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace GameProject.Model
{
    class ImportLevel
    {
        /// <summary>
        /// Read all string from text file to make level
        /// </summary>
        /// <param name="levelIndex"></param>
        public static string ReadLevel(int levelIndex)
        {
            bool levelStringFixed = false;
            string levelPath = String.Format(@"..\..\..\Levels\Level{0}.txt", levelIndex);
            string fullPath = Path.GetFullPath(levelPath);

            using (StreamReader reader = new StreamReader(fullPath))
            {
                string levelString = reader.ReadToEnd();

                while (!levelStringFixed)
                {
                    int indexOfN = levelString.IndexOf("\n");
                    int indexOfR = levelString.IndexOf("\r");

                    if (indexOfN != -1)
                    {
                        levelString = levelString.Remove(indexOfN, 1);
                    }

                    if (indexOfR != -1)
                    {
                        levelString = levelString.Remove(indexOfR, 1);
                    }

                    if (indexOfN == -1 && indexOfR == -1)
                    {
                        levelStringFixed = true;
                    }
                }

                return levelString;
            }
        }
    }

}
