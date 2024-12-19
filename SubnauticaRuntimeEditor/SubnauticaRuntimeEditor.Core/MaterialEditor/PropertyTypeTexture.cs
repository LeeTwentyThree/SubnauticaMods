using System;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public class PropertyTypeTexture : PropertyType
	{
		public PropertyTypeTexture(string property) : base(property)
		{
		}

		protected override void Draw(Material material)
		{
			var texture = material.GetTexture(Property);
			var text = texture == null ? "Unassigned" : texture.name;
			GUILayout.Label(text);
		}

		public override string GenerateSetPropertyCode(Material material)
		{
			return "Unimplemented";
		}
	}
}