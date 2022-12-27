using UnityEngine;
using DebugHelper.Systems;

namespace DebugHelper.Interfaces
{
    public interface IDebugIcon
    {
        string Label { get; }
        Sprite Icon { get; }
        Vector3 Position { get; }
        float Scale { get; }
        Color Color { get; }
        void OnCreation(DebugIconInstance instance); // called every time the interface is assigned, can be called multiple times for the same DebugIconInstance
    }
}