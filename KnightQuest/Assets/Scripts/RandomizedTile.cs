using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiles
{
    /// <summary>
    /// A tile that picks a random option.
    /// </summary>
    public class RandomizedTile : TileBase
    {
        public RandomizedTileOption[] options;

        public override void GetTileData(
            Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (options.Length == 0)
            {
                Debug.LogWarning("No tile options.", this);
                base.GetTileData(position, tilemap, ref tileData);
                return;
            }

            var p1 = position.x > 0
                ? IntPowerN(2, (short)position.x)
                : IntPowerN(3, (short)position.x);
            var p2 = position.y > 0
                ? IntPowerN(5, (short)position.y)
                : IntPowerN(7, (short)position.y);
            var p3 = position.z > 0
                ? IntPowerN(11, (short)position.z)
                : IntPowerN(13, (short)position.z);
            var seed = p1 * p2 * p3;

            var randomDouble = new System.Random((int)seed).NextDouble();
            var frequencySum = options.Sum((o) => (double)o.frequency);

            var val = randomDouble * frequencySum;
            if (val < 0) val += frequencySum;

            var cumulative = 0.0;
            var idx = 0;
            foreach (var option in options)
            {
                cumulative += option.frequency;
                if (val < cumulative)
                {
                    tileData.sprite = option.sprite;
                    return;
                }
                ++idx;
            }
        }

        /// <summary>
        /// Computes an integer power.
        /// </summary>
        private static long IntPowerN(int x, short power)
        {
            // https://stackoverflow.com/a/384695/2640146
            if (power == 0) return 1;
            if (power == 1) return x;

            int n = 15;
            while ((power <<= 1) >= 0) n--;

            // Ignore overflow.
            unchecked
            {
                long tmp = x;
                while (--n > 0)
                    tmp = tmp * tmp *
                         (((power <<= 1) < 0) ? x : 1);
                return tmp;
            }
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Randomized Tile")]
        public static void CreateRandomizedTile()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                    "Save Randomized Tile",
                    "New Randomized Tile",
                    "Asset",
                    "Save Randomized Tile",
                    "Assets");

            if (path == "")
                return;
            AssetDatabase.CreateAsset(CreateInstance<RandomizedTile>(), path);
        }
#endif
    }

    [Serializable]
    public class RandomizedTileOption
    {
        [Tooltip("The sprite for this option.")]
        public Sprite sprite;

        [Tooltip("How frequently this option is used. This is a relative" +
            " number: the higher it is relative to the other options, the" +
            " more likely this option is to be used. This should be a positive" +
            " number.")]
        public float frequency;
    }
}