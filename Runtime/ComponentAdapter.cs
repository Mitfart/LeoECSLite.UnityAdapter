using System;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Runtime.Extensions.Ecs.World;
using Unity.VisualScripting;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Runtime {
  [Serializable]
  public class ComponentAdapter {
    [SerializeReference] private object component;

    [field: SerializeField] public string SerializedComponentType { get; private set; }


    public bool Broken => RawComponent == null;

    public Type ComponentType {
      get => component?.GetType() ?? Type.GetType(SerializedComponentType); //
      private set => SerializedComponentType = value.AssemblyQualifiedName; //
    }

    public object RawComponent {
      get => component;
      set {
        ThrowIfNotCompatible(value);
        component = value;
      }
    }



    public ComponentAdapter(object component) {
#if UNITY_EDITOR
      if (component == null)
        throw new Exception($"{nameof(ComponentAdapter)}: Can't create {nameof(ComponentAdapter)} of NULL!");

      Type type = component.GetType();

      if (!type.IsStruct())
        throw new Exception($"Can't create {nameof(ComponentAdapter)} of non struct type!");
#endif

      ComponentType = type;
      RawComponent  = component;
    }



    public virtual void AddTo(int e, EcsWorld world) {
      IEcsPool pool = world.GetPool(ComponentType);

      if (pool.Has(e))
        pool.Del(e);

      pool.AddRaw(e, RawComponent);
    }



    private void ThrowIfNotCompatible(object value) {
#if UNITY_EDITOR
      Type valueType = value?.GetType();

      if (valueType == null)
        throw new Exception($"{nameof(ComponentAdapter)}: Component can not be NULL!");

      if (!valueType.IsStruct())
        throw new Exception($"{nameof(ComponentAdapter)}: Can't set non struct type!");

      if (ComponentType == null)
        throw new Exception($"{nameof(ComponentAdapter)}: ComponentType is null! (internal)");

      if (!ComponentType.IsAssignableFrom(valueType))
        throw new Exception($"{nameof(ComponentAdapter)}: Incorrect type! \n (type: {valueType.Name} | required: {ComponentType.Name})");
#endif
    }
  }
}