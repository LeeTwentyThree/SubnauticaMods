using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public abstract class PropertyType
	{
		public string Property { get; }
		
		public bool IsHeader { get; protected set; }

		protected PropertyType(string property)
		{
			Property = property;
		}

		public void DrawGUI(Material material)
		{
			Draw(material);
		}

		protected abstract void Draw(Material material);

		protected void PrintLog(object oldValue, object newValue)
		{
			SubnauticaRuntimeEditorCore.Logger.Log(Utils.Abstractions.LogLevel.Debug, string.Format("Shader property {0} updated from ({1}) to ({2})", this.Property, oldValue, newValue));
		}

		public abstract string GenerateSetPropertyCode(Material material);
	}
}