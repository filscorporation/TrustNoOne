using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.RulesManagement.Rules
{
    public class CantMoveOn : RuleBase
    {
        public override string Name => nameof(CantMoveOn);

        private const string tileTypeParam = "TileType";

        public override bool Check(Tile[,] field, List<Step> steps, Dictionary<string, int> parameters)
        {
            Vector2Int lastTo = steps.Last().To;
            Tile lastTile = field[lastTo.x, lastTo.y];
            return lastTile.TypeIndex == parameters[tileTypeParam];
        }
    }
}
