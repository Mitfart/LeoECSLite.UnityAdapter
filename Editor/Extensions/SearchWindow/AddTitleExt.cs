﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.SearchWindow {
  public static class AddTitleExt {
    public static void AddTitle(
      this ICollection<SearchTreeEntry> items,
      string                            text
    ) {
      items.Add(
        new SearchTreeGroupEntry(
          new GUIContent(
            text,
            AddItemExt.IndentationIcon
          )
        ) { level = 0 }
      );
    }
  }
}