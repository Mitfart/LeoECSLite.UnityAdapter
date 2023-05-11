using System;
using System.Collections.Generic;
using System.Linq;
using LeoECSLite.UnityAdapter.Editor.Elements;
using LeoECSLite.UnityAdapter.Editor.Extensions.Property;
using LeoECSLite.UnityAdapter.Editor.Extensions.UIElement;
using LeoECSLite.UnityAdapter.Editor.Search;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityAdapter.Editor {
  [CustomEditor(typeof(Entity), true)]
  public class EntityEditor : UnityEditor.Editor {
    private const string COMPONENTS_PROPERTY_NAME = nameof(Entity.components);

    private const string ADD_BTN_TEXT = "Add";
    private const string DEL_BTN_TEXT = "Del";

    private readonly Dictionary<ComponentAdapter, VisualElement> _componentsViews = new();
    private          Button                                      _addComponentBtn;
    private          Box                                         _components;
    private          VisualElement                               _componentsContainer;
    private          Box                                         _control;
    private          Box                                         _main;


    private VisualElement _root;

    private Entity                 Target     => (Entity) target;
    private List<ComponentAdapter> Components => Target.components;



    public override VisualElement CreateInspectorGUI() {
      CreateElements();
      StructureElements();
      InitElements();
      return _root;
    }



    private void CreateElements() {
      _root                = new VisualElement();
      _main                = new Box();
      _control             = new Box();
      _addComponentBtn     = new Button(ChooseAndAdd) { text = ADD_BTN_TEXT };
      _components          = new Box();
      _componentsContainer = new VisualElement();
    }

    private void StructureElements() {
      _root
       .AddChild(_main.AddScriptField(serializedObject))
       .AddChild(_control.AddChild(_addComponentBtn))
       .AddChild(_components.AddChild(_componentsContainer));
    }

    private void InitElements() {
      InitMain();
      InitControl();
      InitComponents();
    }



    private void InitMain() {
      _main.style.marginBottom = 10;
      AddExtraData();
    }

    private void InitControl() {
      _control.style.marginBottom  = 10;
      _control.style.flexDirection = FlexDirection.Row;

      _addComponentBtn.style.flexGrow = 1f;
    }

    private void InitComponents() {
      SerializedProperty sizeProperty = ComponentsProperty().GetChildren(1)[0];

      RefreshComponentViews();

      _root.TrackPropertyValue(sizeProperty, _ => RefreshComponentViews());
    }



    private void AddExtraData() {
      SerializedProperty currentProp = ComponentsProperty().Copy();

      while (currentProp.NextVisible(false))
        _main.AddChild(new PropertyField(currentProp));
    }



    private void RefreshComponentViews() {
      DelComponentViews();
      AddComponentViews();
    }

    private void DelComponentViews() {
      IEnumerable<ComponentAdapter> removed = RemovedComponents();

      foreach (ComponentAdapter adapter in removed) {
        _componentsContainer.Remove(_componentsViews[adapter]);
        _componentsViews.Remove(adapter);
      }
    }

    private void AddComponentViews() {
      foreach (ComponentAdapter adapter in Components) {
        if (_componentsViews.ContainsKey(adapter))
          continue;

        VisualElement compView = CreateComponentView(adapter);

        _componentsContainer.Add(compView);
        _componentsViews.Add(adapter, compView);
      }
    }



    private IEnumerable<ComponentAdapter> RemovedComponents() {
      return _componentsViews
            .Keys
            .Where(adapter => !Components.Contains(adapter))
            .ToArray();
    }



    private VisualElement CreateComponentView(ComponentAdapter adapter) {
      var view = new PropertyField();

      view.BindProperty(GetPropertyOf(adapter));

      EditorApplication.delayCall += () => {
        view.Q<ControlHeader>()
            .AddButton(
               DEL_BTN_TEXT,
               () => Components.Remove(adapter)
             );
      };

      return view;
    }



    private void ChooseAndAdd() => ComponentsSearchWindow.Open(AddComponent);

    private bool AddComponent(Type component) {
      if (HasAdapter(component)) {
        Debug.LogWarning($"Already has component typeof: {component}");
        return false;
      }

      Target.AddAdapter(component);
      return true;
    }



    private SerializedProperty GetPropertyOf(ComponentAdapter adapter) {
      int index          = Components.IndexOf(adapter);
      int propIndex      = index     + 1; // 0 - "Size" property => +1
      int requiredLength = propIndex + 1; // maxAmount = Length => +1

      return ComponentsProperty().GetChildren(requiredLength)[propIndex];
    }



    private bool HasAdapter(Type component) => Components.Any(adapter => adapter.ComponentType == component);

    private SerializedProperty ComponentsProperty() => serializedObject.FindProperty(COMPONENTS_PROPERTY_NAME);
  }
}