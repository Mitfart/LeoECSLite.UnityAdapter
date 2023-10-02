using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityAdapter.Editor.Elements {
  public class ControlHeader : VisualElement {
    private readonly Dictionary<string, Button> _controlButtons = new();

    private VisualElement _icon;
    private Label         _label;



    public ControlHeader() {
      CreateElements();
      InitElements();
      StructureElements();
    }



    private void CreateElements() {
      _icon  = new VisualElement();
      _label = new Label();
    }

    private void InitElements() {
      InitRoot();
      InitIcon();
      InitLabel();
    }

    private void StructureElements() {
      Add(_icon);
      Add(_label);
    }



    private void InitRoot() {
      style.paddingTop    = 5;
      style.paddingBottom = 5;
      style.paddingLeft   = 5;
      style.paddingRight  = 5;
      style.flexDirection = FlexDirection.Row;
      style.alignItems    = Align.Center;
    }

    private void InitIcon() {
      _icon.style.width       = 15;
      _icon.style.height      = 15;
      _icon.style.marginRight = 4;
      HideIcon();
    }

    private void InitLabel() {
      _label.style.flexGrow                = 1;
      _label.style.unityFontStyleAndWeight = FontStyle.Bold;
      _label.style.unityTextAlign          = TextAnchor.MiddleLeft;
      _label.style.overflow                = Overflow.Hidden;
    }



    public ControlHeader ShowIcon(string iconName) {
      _icon.style.display         = DisplayStyle.Flex;
      _icon.style.backgroundImage = EditorGUIUtility.FindTexture(iconName);
      return this;
    }

    public ControlHeader HideIcon() {
      _icon.style.display = DisplayStyle.None;
      return this;
    }



    public ControlHeader SetLabel(string label) {
      _label.text = label;
      return this;
    }



    public void AddButton(string text, Action onClick) {
      if (_controlButtons.ContainsKey(text))
        throw new Exception($"Already has [{text}]-control button!");

      var btn = new Button(onClick) {
        text = text,
        style = {
          marginBottom = 0,
          marginLeft   = 0,
          marginRight  = 0,
          marginTop    = 0
        }
      };
      
      Add(btn);
      _controlButtons.Add(text, btn);
    }

    public void DelButton(string text) {
      Remove(_controlButtons[text]);
      _controlButtons.Remove(text);
    }
  }
}