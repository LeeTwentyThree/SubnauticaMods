﻿using System.Collections;
using ModStructureHelperPlugin.UndoSystem;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class TransformableObject : MonoBehaviour, IOriginator
{
    public IMemento GetSnapshot()
    {
        return new Memento(this, transform.position, transform.rotation, transform.localScale, Time.frameCount);
    }

    private void Restore(Memento memento)
    {
        transform.position = memento.Position;
        transform.rotation = memento.Rotation;
        transform.localScale = memento.Scale;
    }
    
    private readonly struct Memento : IMemento
    {
        private TransformableObject Object { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Vector3 Scale { get; }
        public int SaveFrame { get; }
        public bool Invalid => Object == null;

        public Memento(TransformableObject o, Vector3 position, Quaternion rotation, Vector3 scale, int saveFrame)
        {
            Object = o;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            SaveFrame = saveFrame;
        }

        public IEnumerator Restore()
        {
            if (Object == null) yield break;
            Object.Restore(this);
        }
    }
}