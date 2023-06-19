using UnityEngine;

namespace CreatureMorphs.Mono.UI;

internal class MorphMenuButton : MonoBehaviour
{
    public MorphDatabase.Entry entry;

    private static Sprite _normalSprite;
    private static Sprite _selectedSprite;

    private void Awake()
    {
        if (_normalSprite == null)
        {
            _normalSprite = Plugin.bundle.LoadAsset<Sprite>("MorphButton");
            _selectedSprite = Plugin.bundle.LoadAsset<Sprite>("MorphButtonSelected");
        }
    }

    public void OnClick()
    {
        PlayerMorpher.main.InitiateMorph(entry.GetMorphType());
    }

    public void SetSpriteSelected(bool selected)
    {
        GetComponent<Image>().sprite = selected ? _selectedSprite : _normalSprite;
    }
}
