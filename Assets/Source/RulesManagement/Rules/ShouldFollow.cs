using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.RulesManagement.Rules
{
    public class ShouldFollow : RuleBase
    {
        public override string Name => nameof(ShouldFollow);

        public const string TileTypeStartParam = "TileTypeStart";
        public const string TileTypeEndParam = "TileTypeEnd";

        public override bool Check(Tile[,] field, List<Step> steps, Dictionary<string, int> parameters)
        {
            if (steps.Count == 1)
                return false;

            Vector2Int lastTo = steps.Last().To;
            Vector2Int lastFrom = steps.Last().From;
            Tile lastTileStart = field[lastFrom.x, lastFrom.y];
            Tile lastTileEnd = field[lastTo.x, lastTo.y];
            return lastTileStart.TypeIndex == parameters[TileTypeStartParam]
                   && lastTileEnd.TypeIndex != parameters[TileTypeEndParam];
        }
    }
}
