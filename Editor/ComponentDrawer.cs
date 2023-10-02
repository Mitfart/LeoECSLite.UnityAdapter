using System;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Elements;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.Property;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.UIElement;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityAdapter.Editor {
  [CustomPropertyDrawer(typeof(OneComponentsAdapter), true)]
  public class ComponentDrawer : PropertyDrawer {
    private const string COMPONENT_FIELD = "component";

    private VisualElement _root;
    private VisualElement _fields;

    private SerializedProperty   _property;
    private OneComponentsAdapter _target;



    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
      _property = property;
      _target   = (OneComponentsAdapter) property.GetUnderlyingValue();

      CreateElements();
      StructureElements();
      return _root;
    }



    private void CreateElements() {
      _root   = new VisualElement();
      _fields = new VisualElement();
    }

    private void StructureElements() {
      _root.AddChild(_fields.AddChildPropertiesOf(ComponentProperty()));
    }



    private bool Empty() => ComponentProperty().GetChildren(1).Count == 0;

    private SerializedProperty ComponentProperty() => _property.FindPropertyRelative(COMPONENT_FIELD);
  }
}