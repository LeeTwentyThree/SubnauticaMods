using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;
using Nautilus.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModStructureHelperPlugin.Mono;

public class ObjectPivotCircle : MonoBehaviour, IOverlayIconData, ISelectionListener, IPointerEnterHandler, IPointerExitHandler
{
    private static Sprite _normalSprite;
    private static Sprite _hoveredSprite;
    private static Sprite _selectedSprite;
    
    public bool showName;

    private bool _selecting;
    private bool _pointerOver;
    
    public string Label => showName ? _objectName : null;
    public Sprite Icon => _selecting ? _selectedSprite : _pointerOver ? _hoveredSprite : _normalSprite;
    public Vector3 Position => transform.position;
    public float Scale => 5f / Vector3.Distance(MainCameraControl.main.transform.position, transform.position);

    private string _objectName;
    
    public void OnCreation(OverlayIconInstance instance)
    {
        if (_normalSprite == null || _hoveredSprite == null || _selectedSprite == null)
        {
            _normalSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleDefault");
            _hoveredSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleHovered");
            _selectedSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleSelected");
        }

        _objectName = gameObject.name.TrimClone();
        
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
    }

    public void OnObjectSelected()
    {
        _selecting = true;
    }

    public void OnObjectDeselected()
    {
        _selecting = false;
    }
    
    private void OnDisable()
    {
        _pointerOver = false;
        OverlayIconManager.main.RemoveIcon(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerOver = false;
    }

    private void OnEnable()
    {
        OverlayIconManager.main.AddIcon(this);
    }

    private void OnDestroy()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
    }

    private void OnStructureInstanceChanged(StructureInstance newInstance)
    {
        if (newInstance == null)
            Destroy(this);
    }
}