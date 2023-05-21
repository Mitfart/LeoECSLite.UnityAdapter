using System.Linq;
using Extensions.Editor.Property;
using Git.Extensions.Editor;
using LeoECSLite.UnityAdapter.Editor.Elements;
using UnityEditor;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityAdapter.Editor {
  [CustomPropertyDrawer(typeof(ComponentAdapter), true)]
  public class ComponentDrawer : PropertyDrawer {
    private const string COMPONENT_FIELD                   = "component";
    private const string ERROR_ICON                        = "console.erroricon";
    private const string ASSEMBLY_QUALIFIED_NAME_SEPARATOR = ",";
    private const string TYPE_SEPARATOR                    = ".";

    private SerializedProperty _property;

    private VisualElement _root;
    private ControlHeader _controlHeader;
    private VisualElement _main;
    private VisualElement _fields;



    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
      _property = property;
      CreateElements();
      StructureElements();
      InitElements();
      return _root;
    }



    private void CreateElements() {
      _root          = new VisualElement();
      _controlHeader = new ControlHeader();
      _main          = new VisualElement();
      _fields        = new VisualElement();
    }

    private void StructureElements() {
      _root
       .AddChild(_controlHeader)
       .AddChild(_main);

      if (!Target().Broken)
        _main.AddChild(_fields.AddChildPropertiesOf(ComponentProperty()));
    }

    private void InitElements() {
      if (!Target().Broken) {
        _controlHeader.SetLabel(ComponentName());

        if (!Empty()) {
          _main.style.paddingTop    = 0;
          _main.style.paddingBottom = 5;
          _main.style.paddingLeft   = 5;
          _main.style.paddingRight  = 5;
        }
      }
      else {
        var errorText = $"Broken | {LastComponentName()}";

        _controlHeader
         .ShowIcon(ERROR_ICON)
         .SetLabel(errorText);

        // Debug.LogError($"ERROR | \"{_property.serializedObject.targetObject.GameObject().name}\"-Entity | {errorText}");
      }

      _fields.style.paddingTop    = 0;
      _fields.style.paddingBottom = 0;
      _fields.style.paddingLeft   = 10;
      _fields.style.paddingRight  = 5;
    }



    private bool               Empty()             => ComponentProperty().GetChildren(1).Count == 0;
    private string             ComponentName()     => Target().RawComponent.GetType().Name;
    private string             LastComponentName() => Target().SerializedComponentType.Split(ASSEMBLY_QUALIFIED_NAME_SEPARATOR).First().Split(TYPE_SEPARATOR).Last();
    private ComponentAdapter   Target()            => (ComponentAdapter) _property.managedReferenceValue;
    private SerializedProperty ComponentProperty() => _property.FindPropertyRelative(COMPONENT_FIELD);
  }
}