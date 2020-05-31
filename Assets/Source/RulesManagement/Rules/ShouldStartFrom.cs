using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.RulesManagement.Rules
{
    public class ShouldStartFrom : RuleBase
    {
        public override string Name => nameof(ShouldStartFrom);

        public const string TileTypeParam = "TileType";

        public override bool Check(Tile[,] field, List<Step> steps, Dictionary<string, int> parameters)
        {
            Vector2Int firstTo = steps.First().To;
            Tile firstTile = field[firstTo.x, firstTo.y];
            return firstTile.TypeIndex == parameters[TileTypeParam];
        }
    }
}
