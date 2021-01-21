using UnityEngine;

sealed class RadialHeatSourceData : MonoBehaviour
{
    public float centerHeat;

    [Range(0, 1)]
    public float constantHeatRadiusRatio;
}
