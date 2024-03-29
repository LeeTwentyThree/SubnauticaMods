﻿using System;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public class PropertyTypeToggle : PropertyType
	{
		public PropertyTypeToggle(string property) : base(property)
		{
		}

		protected override void Draw(Material material)
		{
			bool flag = material.GetFloat(Property) > 0f;
			bool flag2 = GUILayout.Toggle(flag, "Enable", Array.Empty<GUILayoutOption>());
			if (flag != flag2)
			{
				material.SetFloat(Property, flag2 ? 1f : 0f);
				PrintLog(flag, flag2);
			}
		}

		public override string GenerateSetPropertyCode(Material material)
		{
			float @float = material.GetFloat(base.Property);
			return string.Format("SetFloat(\"{0}\", {1}f)", base.Property, @float);
		}
	}
}