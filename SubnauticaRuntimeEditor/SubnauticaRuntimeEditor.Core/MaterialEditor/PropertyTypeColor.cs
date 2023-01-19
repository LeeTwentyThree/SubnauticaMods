using System;
using System.Globalization;
using SubnauticaRuntimeEditor.Core.Utils;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public class PropertyTypeColor : PropertyType
	{
		// Some colors are actually Vector4 in the code.
		// So we have this to determine whether this color property is Vector4 and if so, we set it properly
		// While still displaying it as a color.
		private readonly bool _isVector4;

		public PropertyTypeColor(string property, bool isVector4 = false) : base(property)
		{
			_isVector4 = isVector4;
		}

		protected override void Draw(Material material)
		{
			this.CreateTextureIfNeeded();
			Color color = material.GetColor(Property);
			Color color2 = new Color(color.r, color.g, color.b, color.a);
			Color color3 = GUI.color;
			GUI.color = new Color(color.r, color.g, color.b, 1f);
			GUILayout.Label(PREVIEW_COLOR_TEXTURE, new GUILayoutOption[]
			{
				GUILayout.Width((float)PREVIEW_COLOR_SIZE),
				GUILayout.Height((float)PREVIEW_COLOR_SIZE)
			});
			color2.r = GUILayout.HorizontalSlider(color.r, (float)COLOR_MIN_BOUND, (float)COLOR_MAX_BOUND, new GUILayoutOption[]
			{
                SLIDER_WIDTH,
                SLIDER_HEIGHT
            });
			float.TryParse(GUILayout.TextField(color2.r.ToString("F2", CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
                FIELD_WIDTH,
                FIELD_HEIGHT
            }), NumberStyles.Any, CultureInfo.InvariantCulture, out color2.r);
			color2.g = GUILayout.HorizontalSlider(color.g, (float)COLOR_MIN_BOUND, (float)COLOR_MAX_BOUND, new GUILayoutOption[]
			{
                SLIDER_WIDTH,
                SLIDER_HEIGHT
            });
			float.TryParse(GUILayout.TextField(color2.g.ToString("F2", CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
                FIELD_WIDTH,
                FIELD_HEIGHT
            }), NumberStyles.Any, CultureInfo.InvariantCulture, out color2.g);
			color2.b = GUILayout.HorizontalSlider(color.b, (float)COLOR_MIN_BOUND, (float)COLOR_MAX_BOUND, new GUILayoutOption[]
			{
                SLIDER_WIDTH,
                SLIDER_HEIGHT
            });
			float.TryParse(GUILayout.TextField(color2.b.ToString("F2", CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
                FIELD_WIDTH,
                FIELD_HEIGHT
            }), NumberStyles.Any, CultureInfo.InvariantCulture, out color2.b);
			color2.a = GUILayout.HorizontalSlider(color.a, (float)COLOR_MIN_BOUND, (float)COLOR_MAX_BOUND, new GUILayoutOption[]
			{
                SLIDER_WIDTH,
                SLIDER_HEIGHT
            });
			float.TryParse(GUILayout.TextField(color2.a.ToString("F2", CultureInfo.InvariantCulture), new GUILayoutOption[]
			{
                FIELD_WIDTH,
                FIELD_HEIGHT
            }), NumberStyles.Any, CultureInfo.InvariantCulture, out color2.a);
			GUI.color = color3;
			if (!color.IsEqualApprox(color2, 0.1f))
			{
				if (_isVector4)
					material.SetVector(Property, color2);
				else
					material.SetColor(Property, color2);
				
                PrintLog(color, color2);
			}
		}

		private void CreateTextureIfNeeded()
		{
			if (PREVIEW_COLOR_TEXTURE == null)
			{
                PREVIEW_COLOR_TEXTURE = new Texture2D(PREVIEW_COLOR_SIZE, PREVIEW_COLOR_SIZE);
			}
		}

		public override string GenerateSetPropertyCode(Material material)
		{
			Color color = material.GetColor(Property);
			return string.Format("SetColor(\"{0}\", new Color({1}f, {2}f, {3}f, {4}f))", new object[]
			{
                Property,
				color.r,
				color.g,
				color.b,
				color.a
			});
		}

		private static Texture2D PREVIEW_COLOR_TEXTURE;

		private static readonly int PREVIEW_COLOR_SIZE = 20;

		private static readonly int COLOR_MIN_BOUND = 0;

		private static readonly int COLOR_MAX_BOUND = 1;

		private static readonly GUILayoutOption FIELD_WIDTH = GUILayout.Width(40f);

		private static readonly GUILayoutOption FIELD_HEIGHT = GUILayout.Height(19f);

		private static readonly GUILayoutOption SLIDER_WIDTH = GUILayout.Height(10f);

		private static readonly GUILayoutOption SLIDER_HEIGHT = GUILayout.Width(33f);
	}
}