using System;
using System.Collections.Generic;
using Assets.Source.RulesManagement.Rules;

namespace Assets.Source.RulesManagement
{
    public abstract class RuleBase
    {
        public abstract string Name { get; }

        /// <summary>
        /// Returns if players last step breaks this rule or not
        /// </summary>
        /// <param name="field"></param>
        /// <param name="steps"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract bool Check(Tile[,] field, List<Step> steps, Dictionary<string, int> parameters);

        protected Direction StepDirection(Step step)
        {
            if (step.From.x < step.To.x)
                return Direction.Right;
            if (step.From.x > step.To.x)
                return Direction.Left;
            if (step.From.y > step.To.y)
                return Direction.Up;
            if (step.From.y < step.To.y)
                return Direction.Down;
            throw new Exception("Step on the same tile");
        }
    }
}
