using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Part of the field
    /// </summary>
    public class Tile : MonoBehaviour
    {
        public int X;

        public int Y;

        public int TypeIndex;

        public void Initialize(int x, int y, int typeIndex)
        {
            X = x;
            Y = y;
            TypeIndex = typeIndex;
        }
    }
}
