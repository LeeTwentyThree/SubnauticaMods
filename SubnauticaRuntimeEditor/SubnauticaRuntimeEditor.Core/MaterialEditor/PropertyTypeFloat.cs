using System.Globalization;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public class PropertyTypeFloat : PropertyType
	{
		public PropertyTypeFloat(string property, bool isHeader) : base(property)
		{
			IsHeader = isHeader;
		}
		
		public PropertyTypeFloat(string property, float minimum, float maximum) : base(property)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public PropertyTypeFloat(string property) : base(property)
		{
			minimum = 0f;
			maximum = 50f;
		}
		
		protected override void Draw(Material material)
		{
			var @float = material.GetFloat(Property);
			var num = GUILayout.HorizontalSlider(@float, minimum, maximum, SLIDER_WIDTH);
			float.TryParse(GUILayout.TextField(num.ToString(MaterialEditorViewer.FloatPropertyFormattingString,
					CultureInfo.InvariantCulture), FIELD_WIDTH),
				NumberStyles.Any, CultureInfo.InvariantCulture, out num);
			if (@float == num) return;
			material.SetFloat(Property, num);
			PrintLog(@float, num);
		}

		public override string GenerateSetPropertyCode(Material material)
		{
			float @float = material.GetFloat(base.Property);
			return string.Format("SetFloat(\"{0}\", {1}f)", base.Property, @float);
		}

		private static readonly GUILayoutOption FIELD_WIDTH = GUILayout.Width(100f);

		private static readonly GUILayoutOption SLIDER_WIDTH = GUILayout.Width(100f);

		private readonly float minimum;

		private readonly float maximum;
	}
}