using UnityEngine;

namespace Assets.Source.RulesManagement
{
    /// <summary>
    /// Players step
    /// </summary>
    public class Step
    {
        public Vector2Int From;

        public Vector2Int To;

        public Step(int fromX, int fromY, int toX, int toY)
        {
            From = new Vector2Int(fromX, fromY);
            To = new Vector2Int(toX, toY);
        }

        public Step(int toX, int toY)
        {
            To = new Vector2Int(toX, toY);
        }
    }
}
