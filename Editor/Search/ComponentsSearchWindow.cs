using System;
using System.Collections.Generic;
using Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.SearchWindow;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Editor.Search {
  public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider {
    private const string TITLE = "Components";

    private static ComponentsSearchWindow _Window;

    private Func<Type, bool> _select;



    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
      int length = ComponentsDatabase.SerializableComponents.Count;
      var items  = new List<SearchTreeEntry>(length);
      var groups = new List<string>(length);

      items.AddTitle(TITLE);

      foreach (Type componentType in ComponentsDatabase.SerializableComponents)
        AddComponent(componentType);

      return items;


      void AddComponent(Type component) {
        items
         .AddNamespaceGroups(groups, component, out int indentLevel)
         .AddItem(component.Name, indentLevel, component);
      }
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
      var component = (Type) searchTreeEntry.userData;
      return _select?.Invoke(component) ?? false;
    }



    public static void Open(Func<Type, bool> select = null) {
      SearchWindow.Open(MousePosition(), Window());

      _Window._select = select;
    }



    private static ComponentsSearchWindow Window() {
      return _Window ??= CreateInstance<ComponentsSearchWindow>();
    }

    private static SearchWindowContext MousePosition() {
      Vector2 mousePos  = Event.current.mousePosition;
      Vector2 openPoint = GUIUtility.GUIToScreenPoint(mousePos);
      var     context   = new SearchWindowContext(openPoint);

      return context;
    }
  }
}