using UnityEditor;

namespace NGToolsEditor.NGDraggableObject
{
	using UnityEngine;

#if NG_DRAGGABLE_OBJECT
	[CustomPropertyDrawer(typeof(Object), true)]
#endif
	internal sealed class NGDraggableObjectDrawer : PropertyDrawer
	{
		private InternalNGDraggableObjectDrawer	drawer;

		public override void	OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (this.drawer == null)
				this.drawer = new InternalNGDraggableObjectDrawer(this.fieldInfo);

			this.drawer.OnGUI(position, property, label);
		}
	}
}