using UnityEngine.UI;

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

    public void OnSelect()
    {
        PlayerMorpher.main.InitiateMorph(entry.GetMorphType());
    }

    public void SetSpriteHovered(bool selected)
    {
        GetComponent<Image>().sprite = selected ? _selectedSprite : _normalSprite;
    }
}
