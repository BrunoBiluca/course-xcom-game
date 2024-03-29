﻿using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public enum UnitFactions
    {
        Player,
        Enemy,
        Furniture
    }

    /// <summary>
    /// Base interface for placeable units in the grid
    /// </summary>
    public interface IUnit
    {
        string Name { get; }
        bool IsBlockable { get { return true; } }
        ITransform Transform { get; }
        UnitFactions Faction { get; }
        ISelectable Selectable { get { return null; } }
    }
}
