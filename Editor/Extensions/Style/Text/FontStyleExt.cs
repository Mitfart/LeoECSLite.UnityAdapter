﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions.Style.Text {
  public static class FontStyleExt {
    public static IStyle FontStyle(this IStyle style, FontStyle fontStyle) {
      style.unityFontStyleAndWeight = fontStyle;
      return style;
    }
  }
}