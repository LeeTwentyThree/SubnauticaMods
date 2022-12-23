using System;
using System.Globalization;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public class PropertyTypeToggle : PropertyType
	{
		public PropertyTypeToggle(string property, float minimum, float maximum) : base(property)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public PropertyTypeToggle(string property) : base(property)
		{
			this.minimum = 0f;
			this.maximum = 50f;
		}

		protected override void Draw(Material material)
		{
			float @float = material.GetFloat(base.Property);
			float num = GUILayout.HorizontalSlider(@float, this.minimum, this.maximum, new GUILayoutOption[]
			{
				PropertyTypeToggle.SLIDER_WIDTH
			});
			float.TryParse(GUILayout.TextField(num.ToString("F2", CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
				PropertyTypeToggle.FIELD_WIDTH
			}), NumberStyles.Any, CultureInfo.InvariantCulture, out num);
			if (@float != num)
			{
				material.SetFloat(base.Property, num);
				base.PrintLog(@float, num);
			}
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