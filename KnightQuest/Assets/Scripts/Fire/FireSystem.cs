using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A game singleton that processes heat effects.
/// </summary>
public sealed class FireSystem : MonoBehaviour
{
    /// <summary>
    /// If the <paramref name="target"/> happens to have a <see cref="HeatReceiver"/>, this heats it
    /// up by the <paramref name="heatSource"/> during the next Update() call.
    /// </summary>
    public void AddHeat(GameObject target, HeatSource heatSource)
    {
        if (m_heatUpdates.TryGetValue(target, out var updates))
        {
            updates.Add(heatSource);
        }
        else
        {
            m_heatUpdates[target] = new HashSet<HeatSource>() { heatSource };
        }
    }

    void Update()
    {
        // Update is called after the physics step(s). Importantly, it happens after
        // OnTriggerStay2D()
        foreach (var entry in m_heatUpdates)
        {
            // Object could have been destroyed
            if (entry.Key == null)
                continue;

            // Call GetComponent() in Update() instead of in AddHeat() to only call it once in cases
            // where there are multiple physics frames or if an object is heated by multiple sources
            if (entry.Key.TryGetComponent<HeatReceiver>(out var receiver))
            {
                foreach (var heater in entry.Value)
                {
                    if (heater == null)
                        continue;
                    heater.Heat(receiver);
                }
            }
        }

        m_heatUpdates.Clear();
    }

    readonly Dictionary<GameObject, HashSet<HeatSource>> m_heatUpdates =
        new Dictionary<GameObject, HashSet<HeatSource>>();
}
