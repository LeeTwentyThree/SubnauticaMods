using System;
using System.Globalization;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public class PropertyTypeVector4 : PropertyType
	{
		public PropertyTypeVector4(string property, int minBound, int maxBound) : base(property)
		{
			this.minBound = minBound;
			this.maxBound = maxBound;
		}

		public PropertyTypeVector4(string property) : base(property)
		{
			this.minBound = 0;
			this.maxBound = 1;
		}

		protected override void Draw(Material material)
		{
			Vector4 vector = material.GetVector(base.Property);
			Vector4 vector2 = new Vector4(vector.x, vector.y, vector.z, vector.w);
			vector2.x = GUILayout.HorizontalSlider(vector.x, (float)this.minBound, (float)this.maxBound, new GUILayoutOption[]
			{
				PropertyTypeVector4.SLIDER_WIDTH,
				PropertyTypeVector4.SLIDER_HEIGHT
			});
			float.TryParse(GUILayout.TextField(vector2.x.ToString(MaterialEditorViewer.FloatPropertyFormattingString, CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
				PropertyTypeVector4.FIELD_WIDTH,
				PropertyTypeVector4.FIELD_HEIGHT
			}), NumberStyles.Any, CultureInfo.InvariantCulture, out vector2.x);
			vector2.y = GUILayout.HorizontalSlider(vector.y, (float)this.minBound, (float)this.maxBound, new GUILayoutOption[]
			{
				PropertyTypeVector4.SLIDER_WIDTH,
				PropertyTypeVector4.SLIDER_HEIGHT
			});
			float.TryParse(GUILayout.TextField(vector2.y.ToString(MaterialEditorViewer.FloatPropertyFormattingString, CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
				PropertyTypeVector4.FIELD_WIDTH,
				PropertyTypeVector4.FIELD_HEIGHT
			}), NumberStyles.Any, CultureInfo.InvariantCulture, out vector2.y);
			vector2.z = GUILayout.HorizontalSlider(vector.z, (float)this.minBound, (float)this.maxBound, new GUILayoutOption[]
			{
				PropertyTypeVector4.SLIDER_WIDTH,
				PropertyTypeVector4.SLIDER_HEIGHT
			});
			float.TryParse(GUILayout.TextField(vector2.z.ToString(MaterialEditorViewer.FloatPropertyFormattingString, CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
				PropertyTypeVector4.FIELD_WIDTH,
				PropertyTypeVector4.FIELD_HEIGHT
			}), NumberStyles.Any, CultureInfo.InvariantCulture, out vector2.z);
			vector2.w = GUILayout.HorizontalSlider(vector.w, (float)this.minBound, (float)this.maxBound, new GUILayoutOption[]
			{
				PropertyTypeVector4.SLIDER_WIDTH,
				PropertyTypeVector4.SLIDER_HEIGHT
			});
			float.TryParse(GUILayout.TextField(vector2.w.ToString(MaterialEditorViewer.FloatPropertyFormattingString, CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
				PropertyTypeVector4.FIELD_WIDTH,
				PropertyTypeVector4.FIELD_HEIGHT
			}), NumberStyles.Any, CultureInfo.InvariantCulture, out vector2.w);
			if (!vector.Equals(vector2))
			{
				material.SetVector(base.Property, vector2);
				base.PrintLog(vector, vector2);
			}
		}

		public override string GenerateSetPropertyCode(Material material)
		{
			Vector4 vector = material.GetVector(base.Property);
			return string.Format("SetVector(\"{0}\", new Vector4({1}f, {2}f, {3}f, {4}f))", new object[]
			{
				base.Property,
				vector.x,
				vector.y,
				vector.z,
				vector.w
			});
		}

		private static readonly GUILayoutOption FIELD_WIDTH = GUILayout.Width(38f);

		private static readonly GUILayoutOption FIELD_HEIGHT = GUILayout.Height(19f);

		private static readonly GUILayoutOption SLIDER_WIDTH = GUILayout.Height(10f);

		private static readonly GUILayoutOption SLIDER_HEIGHT = GUILayout.Width(33f);

		private readonly int minBound;

		private readonly int maxBound;
	}
}