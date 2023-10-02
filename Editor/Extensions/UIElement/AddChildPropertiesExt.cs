using Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.Property;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.UIElement {
  public static class AddChildPropertiesExt {
    public static VisualElement AddChildPropertiesOf(this VisualElement root, SerializedProperty property) {
      foreach (SerializedProperty childProperty in property.GetChildren())
        root.Add(new PropertyField(childProperty));
      return root;
    }
  }
}