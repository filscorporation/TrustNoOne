using System;

namespace Assets.Source
{
    [Serializable]
    public class Level
    {
        public int Index;

        public int PlayersLife;

        public int[,] TileTypes;

        public NPC[] NPCs;
    }
}
