using System.Collections.Generic;
using ModStructureHelperPlugin.Interfaces;
using UnityEngine;

namespace ModStructureHelperPlugin.UI;

public class OverlayIconManager : MonoBehaviour
{
    public static OverlayIconManager main;

    [SerializeField] private GameObject overlayIconPrefab;

    private readonly List<Entry> _entries = new();

    public void AddIcon(IOverlayIconData data)
    {
        var iconObject = Instantiate(overlayIconPrefab, transform);
        var icon = iconObject.GetComponent<OverlayIconInstance>();
        icon.gameObject.SetActive(true);
        icon.OnCreate(data);
        _entries.Add(new Entry(icon, data));
    }
    
    public void RemoveIcon(IOverlayIconData data)
    {
        foreach (var entry in _entries)
        {
            if (entry.Data != data) continue;
            _entries.Remove(entry);
            Destroy(entry.IconInstance.gameObject);
            return;
        }
    }

    private void Awake()
    {
        main = this;
    }

    private struct Entry
    {
        public OverlayIconInstance IconInstance { get; }
        public IOverlayIconData Data { get; }

        public Entry(OverlayIconInstance iconInstance, IOverlayIconData data)
        {
            IconInstance = iconInstance;
            Data = data;
        }
    }
}