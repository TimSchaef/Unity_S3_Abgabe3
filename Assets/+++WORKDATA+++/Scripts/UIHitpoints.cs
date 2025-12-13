using TMPro;
using UnityEngine;

/// <summary>
/// Visualizes the current hitpoints of an entity in the GUI.
/// </summary>
public class UIHitpoints : MonoBehaviour
{
    [SerializeField] private TMP_Text hitpointsTextmesh;

    /// <summary>
    /// Updates the currently shown hitpoints with the provided parameter.
    /// </summary>
    public void UpdateHitpoints(int newHitpoints)
    {
        hitpointsTextmesh.text = newHitpoints.ToString();
    }
}
