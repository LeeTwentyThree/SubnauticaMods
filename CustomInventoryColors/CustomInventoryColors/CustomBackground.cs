using UnityEngine;

namespace InventoryColorCustomization
{
    internal class CustomBackground
    {
        public string name;
        public Texture2D texture;

        public CustomBackground(string name, Texture2D texture)
        {
            this.name = name;
            this.texture = texture;
        }
    }
}