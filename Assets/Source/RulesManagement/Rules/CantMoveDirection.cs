using System.Collections.Generic;
using System.Linq;

namespace Assets.Source.RulesManagement.Rules
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public class CantMoveDirection : RuleBase
    {
        public override string Name => nameof(CantMoveDirection);

        public const string DirectionParam = "Direction";

        public override bool Check(Tile[,] field, List<Step> steps, Dictionary<string, int> parameters)
        {
            if (steps.Count < 2)
                return false;
            return StepDirection(steps.Last()) == (Direction)parameters[DirectionParam];
        }
    }
}
