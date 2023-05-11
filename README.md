# UnityAdapter - Интеграция в редактор Unity для LeoECS Lite
Интеграция в редактор Unity для конвертации компонентов Unity в ECS

> Проверено на Unity ***2021.3 LTS*** (зависит от Unity)



# Содержание
* [Установка](#Установка)
    * [В виде unity модуля](#В-виде-unity-модуля)
    * [В виде исходников](#В-виде-исходников)
* [Использование](#Интеграция)
    * [Подключение системы](#Подключение-системы)
    * [Объявление компонента](#Объявление-компонента)
* [Обратная связь](#Обратная-связь)
* [Известные проблемы](#Известные-проблемы)

  

# Установка

> **ВАЖНО!** Зависит от [LeoECS Lite](https://github.com/Leopotam/ecslite)
- `фреймворк должены быть установлены до этого расширения.`


### В виде Unity модуля
##### Через Package manager:
- Откройте Package manager
- Нажмите плюс в левом верхнем углу
- "Add from git url" и вставьте:

```
https://github.com/Mitfart/LeoECSLite.UnityIntegration.git
```

##### или через добавление в *@/Packages/manifest.json*:
```
"com.mitfart.leoecslite.unity-integration": "https://github.com/Mitfart/LeoECSLite.UnityIntegration.git",
```

### В виде исходников
Код также может быть склонирован или получен в виде архива



# Использование

### Подключение системы
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

### Объявление компонента
```c#
[Serializable]
[EcsComponent]
public struct Comp {
    ...
}
```

### Создание сущности
Добавьте компонент **Entity** на геймобжект, введити имя мира и добавьте компоненты \
- оставьте пустым имя мира, для "основного" мира
- Компоненты распологаются по неймспейсам



# Обратная связь
#### Discord [Группа по LeoEcsLite](https://discord.gg/5GZVde6)
#### Telegram [Группа по Ecs](https://t.me/ecschat)
```
@Mitfart
```


# Известные проблемы

### При переименовании / изменении неймспейса типа компонент "ломается"
Решение:
- Добавьте аттрибут [MovedFrom](https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Scripting/APIUpdating/UpdatedFromAttribute.cs)
```cs
[MovedFrom(
   autoUpdateAPI: false, 
   sourceNamespace: "OldNamespace", // null, if you don't change it
   sourceAssembly: "OldAssembly",   // null, if you don't change it
   sourceClassName: "OldName"       // null, if you don't change it
)]
public struct Comp { ... }
```