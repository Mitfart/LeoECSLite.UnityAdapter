# UnityAdapter - Unity Editor integration for LeoECS Lite
Unity Editor integration for converting Unity components into ECS

> Tested on Unity ***2021.3 LTS*** (depends on Unity)



# Contents
* [Installation](#Установка)
    * [As a Unity module](#В-виде-unity-модуля)
    * [As source code](#В-виде-исходников)
* [Usage](#Интеграция)
    * [Connecting the system](#Подключение-системы)
    * [Declaring a component](#Объявление-компонента)
* [Feedback](#Обратная-связь)
* [Known issues](#Известные-проблемы)

  

# Installation

> **IMPORTANT!** Depends on [LeoECS Lite](https://github.com/Leopotam/ecslite)
- `the framework must be installed before this extension.`


### As a Unity module
##### Via Package manager:
- Open Package manager
- Click the plus in the top-left corner
- Select "Add from git url" and paste:

```
https://github.com/Mitfart/LeoECSLite.UnityIntegration.git
```

##### or by adding to *@/Packages/manifest.json*:
```
"com.mitfart.leoecslite.unity-integration": "https://github.com/Mitfart/LeoECSLite.UnityIntegration.git",
```

### As source code
The code can also be cloned or downloaded as an archive



# Usage

### Connecting the system
```c#
// ecs-startup code:
IEcsSystems _systems;

void Start() {        
    _systems = new EcsSystems (new EcsWorld());
    ...
     // После добавления всех миров
    _systems.RegisterWorlds();
    ...
}
```

### Declaring a component
```c#
[Serializable]
[EcsComponent] // <-- обязательный атрибут
public struct Comp {
    ...
}
```

### Creating an entity
Add the **Entity** component to the game object, enter the world name, and add components \
- leave the world name empty for the "main" world
- Components are arranged by namespace



# Feedback
#### Discord [LeoEcsLite group](https://discord.gg/5GZVde6)
#### Telegram [Ecs group](https://t.me/ecschat)
```
@Mitfart
```


# Known issues

### When renaming / changing a type namespace, the component "breaks"
Solution:
- Add the [MovedFrom](https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Scripting/APIUpdating/UpdatedFromAttribute.cs) attribute
```cs
[MovedFrom(
   autoUpdateAPI: false, 
   sourceNamespace: "OldNamespace", // null, if you don't change it
   sourceAssembly: "OldAssembly",   // null, if you don't change it
   sourceClassName: "OldName"       // null, if you don't change it
)]
public struct Comp { ... }
```
