using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Manages field and tiles
    /// </summary>
    public class FieldManager : MonoBehaviour
    {
        private static FieldManager instance;
        public static FieldManager Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<FieldManager>())
            : instance;

        public Tile[,] Field { get; private set; }

        [SerializeField] private GameObject tilePrefab;
        [SerializeField] public Sprite[] TileTypes;

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
                    GameObject go = Instantiate(tilePrefab, transform.position + new Vector3(i + 0.5F, -j - 0.5F), Quaternion.identity, transform);
                    Tile tile = go.GetComponent<Tile>();
                    SpriteRenderer sr = go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    sr.sprite = TileTypes[tileType];
                    sr.material = go.GetComponent<SpriteRenderer>().material;
                    tile.Initialize(i, j, tileType);
                    Field[i, j] = tile;
                }
            }
        }
    }
}
