using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Elements;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.Property;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.UIElement;
using Mitfart.LeoECSLite.UnityAdapter.Editor.Search;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityAdapter.Editor {
  [CustomEditor(typeof(Entity), true)]
  public class EntityEditor : UnityEditor.Editor {
    private const string COMPONENTS_PROPERTY_NAME = nameof(Entity.components);

    private const string ERROR_ICON = "console.erroricon";

    private const string ADD_BTN_TEXT = "Add";
    private const string DEL_BTN_TEXT = "Del";

    private readonly Dictionary<IComponentsAdapter, VisualElement> _componentsViews = new();

    private VisualElement _root;
    private Box           _main;
    private Box           _control;
    private Button        _addComponentBtn;
    private Box           _components;

    private Entity                   Target     => (Entity) target;
    private List<IComponentsAdapter> Components => Target.components;



    public override VisualElement CreateInspectorGUI() {
      CreateElements();
      StructureElements();
      InitElements();
      return _root;
    }



    private void CreateElements() {
      _root            = new VisualElement();
      _main            = new Box();
      _control         = new Box();
      _addComponentBtn = new Button(ChooseAndAdd) { text = ADD_BTN_TEXT };
      _components      = new Box();
    }

    private void StructureElements() {
      _root
       .AddChild(_main.AddScriptField(serializedObject))
       .AddChild(_control.AddChild(_addComponentBtn))
       .AddChild(_components);
    }

    private void InitElements() {
      InitMain();
      InitControl();
      InitComponents();
    }



    private void InitMain() {
      _main.style.marginBottom = 10;
      _main.style.marginLeft   = -10;
      AddExtraData();
    }

    private void InitControl() {
      _control.style.marginBottom  = 10;
      _control.style.marginLeft    = -10;
      _control.style.flexDirection = FlexDirection.Row;

      _addComponentBtn.style.flexGrow = 1f;
    }

    private void InitComponents() {
      _components.style.marginLeft = -10;

      RefreshComponentsViews();
      _root.TrackPropertyValue(SizeProperty(), _ => RefreshComponentsViews());
    }



    private void AddExtraData() {
      SerializedProperty currentProp = ComponentsProperty().Copy();

      while (currentProp.NextVisible(false))
        _main.AddChild(new PropertyField(currentProp));
    }



    private void RefreshComponentsViews() {
      RefreshData();
      DelComponentViews();
      AddComponentViews();
    }

    private void DelComponentViews() {
      foreach (IComponentsAdapter adapter in RemovedComponents()) {
        _componentsViews[adapter].RemoveFromHierarchy();
        _componentsViews.Remove(adapter);
      }
    }

    private void AddComponentViews() {
      foreach (IComponentsAdapter adapter in Components) {
        if (_componentsViews.ContainsKey(adapter))
          continue;

        VisualElement compView = CreateComponentView(adapter);

        _components.Add(compView);
        _componentsViews.Add(adapter, compView);
      }
    }



    private IEnumerable<IComponentsAdapter> RemovedComponents() {
      return _componentsViews
            .Keys
            .Where(adapter => !Components.Contains(adapter))
            .ToArray();
    }



    private VisualElement CreateComponentView(IComponentsAdapter adapter) {
      var view   = new VisualElement();
      var header = new ControlHeader();
      var fields = new PropertyField(GetPropertyOf(adapter));

      view
       .AddChild(header)
       .AddChild(fields);

      InitHeader();
      InitFields();

      return view;

      
      void InitHeader() {
        if (adapter.Broken)
          header
           .ShowIcon(ERROR_ICON)
           .SetLabel(adapter.BrokenMsg());
        else
          header.SetLabel(adapter.Name);

        header.AddButton(DEL_BTN_TEXT, () => Components.Remove(adapter));
      }

      void InitFields() {
        fields.style.marginLeft   = 10;
        fields.style.marginRight  = 5;
        fields.style.marginBottom = 5;
        fields.style.marginTop    = -5;
      }
    }


    private SerializedProperty GetPropertyOf(IComponentsAdapter adapter) {
      int index          = Components.IndexOf(adapter);
      int propIndex      = index     + 1; // 0 - "Size" property => +1
      int requiredLength = propIndex + 1; // maxAmount = Length => +1

      return ComponentsProperty().GetChildren(requiredLength)[propIndex];
    }



    private void ChooseAndAdd() => ComponentsSearchWindow.Open(AddComponent);

    private bool AddComponent(Type component) {
      if (HasAdapter(component)) {
        Debug.LogWarning($"Already has component typeof: {component}");
        return false;
      }

      Target.Add(component);
      return true;
    }



    private bool HasAdapter(Type component) => Components.Any(adapter => adapter.Types.Contains(component));

    private SerializedProperty SizeProperty()       => ComponentsProperty().GetChildren(1)[0]; // hack: "Size" property is the first one
    private SerializedProperty ComponentsProperty() => serializedObject.FindProperty(COMPONENTS_PROPERTY_NAME);

    private void RefreshData() => serializedObject.Update();
  }
}