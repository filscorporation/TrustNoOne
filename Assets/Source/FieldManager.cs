using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Manages field and tiles
    /// </summary>
    public class FieldManager : MonoBehaviour
    {
        private static FieldManager instance;
        public static FieldManager Instance => instance ?? (instance = FindObjectOfType<FieldManager>());

        public Tile[,] Field { get; private set; }

        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Sprite[] tileTypes;

        /// <summary>
        /// Generate level from its data
        /// </summary>
        public void Generate(Level level)
        {
            Field = new Tile[level.TileTypes.GetLength(0), level.TileTypes.GetLength(1)];

            for (int i = 0; i < level.TileTypes.GetLength(0); i++)
            {
                for (int j = 0; j < level.TileTypes.GetLength(1); j++)
                {
                    int tileType = level.TileTypes[i, j];
                    GameObject go = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity, transform);
                    Tile tile = go.GetComponent<Tile>();
                    go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tileTypes[tileType];
                    tile.Initialize(i, j, tileType);
                    Field[i, j] = tile;
                }
            }
        }
    }
}
