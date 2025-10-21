<h1 align="center">Flexible 2D Platformer Controller - Unity</h1>

<p align="center">
    <img src="Assets/ForGithub/logo.gif" alt="logo" />
</p>

<p align="center">
  <!-- GitHub profile -->
  <a href="https://github.com/Vladislav-EG"
     style="text-decoration: none;"><img
      src="https://img.shields.io/static/v1?style=flat&label=GitHub&message=Vladislav-EG&color=6243c4&logo=github"
      alt="GitHub badge"
      style="border: none;"
    /></a>
  &nbsp;
  <!-- Unity version -->
  <a href="https://unity3d.com/"
     style="text-decoration: none;"><img
      src="https://img.shields.io/badge/Unity-6000.0.38f1-6243c4?style=flat&logo=unity"
      alt="Unity badge"
      style="border: none;"/></a>
  &nbsp;
  <!-- Itch.io profile -->
  <a href="https://itch.io/profile/ilovevladislav"style="text-decoration: none;"><img
      src="https://img.shields.io/static/v1?style=flat&label=Itch.io&message=ilovevladislav&color=6243c4&logo=Itch.io&logoColor=white"
      alt="Itch.io badge"
      style="border: none;"/></a>
</p>

# About

This project is a convenient, customizable controller for your 2D platform game.
It's an easily modifiable system for moving your hero.
It features an extensive amount of quickly and easily configurable actions.
You can use it as a standalone system, supplementing it with other necessary game systems.

> [!TIP]
> You can try the controller in your browser - [Itch.io](https://ilovevladislav.itch.io/platformer-controller-2d)

![GamePlay](Assets/ForGithub/gameplay.gif)


# Installation
> [!IMPORTANT]<br/>
> The controller was created on Unity version `6000.0.38f1 / 11 feb 2025`.

You can download the project and install it through UnityHub, or use the controller as a UPM package for Unity

## Installation as a Unity project

1. Download the Unity project archive to your computer using `Download .zip` or `git clone`
  
```console
git clone https://github.com/Vladislav-EG/Flexible2DCharacterControllerForUnity.git
```

2. Open the project using `Unity Hub`

## Install as a package

There are two ways to install the package, via URL or from disk

> [!NOTE]<br/>
> When installing `from disk` you will be able to `edit files`, `URL` installation `does not provide this capability`.

> [!TIP]
> Test scenes can be installed in `Package manager` **-** `_PlatformerController2D` **–** `Samples` **–** `Import`

### Installation from disk

1. Download the Unity project archive to your computer using `Download .zip` or `git clone`

```console
git clone https://github.com/Vladislav-EG/Flexible2DCharacterControllerForUnity.git
```

2. In Unity `Windows` – `Package manager` - `+` - `install package from disk` – `pathForDownloadProject\Assets\PlatformerController2D\package.json`


### Installation from git URL

> [!IMPORTANT]<br/>
> You need to download not by the classic link that can be copied in Git, but by the `link with the path to package.json.`

1. In Unity – `Windows` – `Package manager` - `+` - `install package from git URL...`

```console
https://github.com/Vladislav-EG/Flexible2DCharacterControllerForUnity.git?path=/Assets/PlatformerController2D
```

# Usage

When downloading the project in the `Scene` folder or separately importing scenes from `Samples`,
you can run any scene and try the controller.

> [!TIP]
> In the `scene hierarchy` or in the `prefabs` folder you can find the character and see its organization.

## Controls

![PlayerColliderExample](Assets/ForGithub/Save.png)

## Creating and configuring your own character

1.  Create the following object hierarchy with their components in Unity

- Player - (Rigidbody2D, PlayerController, CollisionChecker, Physic Handler 2D, Collider Sprite Resizer, InputReader)
  - PlayerSprite (Sprite Renderer)
  - Colliders (CreateEmpty)
    - Body (Capsule Collider 2D \ Box Collider 2D)
    - Feet (Box Collider 2D)

Example object hierarchy:

[//]: # (![ObjectHierarchy]&#40;Assets/ForGithub/ObjectHierarchy.png&#41;)
<img src="Assets/ForGithub/ObjectHierarchy.png" alt="Grid" height="300">

2. Create a settings preset for the character

In Unity `ProjectWindow` - `right-click` - `Create` - `PlayerControllerStats`,  
Configure the created scriptable object as you wish.

> [!IMPORTANT]<br/>
>At the bottom there is a `GroundLayer` setting, `create a ground layer` and add it there, also `add this layer to all ground` blocks.

Example of part of the settings for the Player object:  

[//]: # (![BasePreset]&#40;Assets/ForGithub/BasePreset.PNG&#41;)
<img src="Assets/ForGithub/BasePreset.PNG" alt="Grid" height="300">

> [!TIP]<br/>
>The scene has the ability to configure the preset using UI, when changing values
>in the UI they will also change in the SO, when disabling `PlayMode` the settings will be saved.


<img src="Assets/ForGithub/UiPreset.PNG" alt="Grid" height="300">

3. Create a physics material

`In Unity ProjectWindow` - `right-click` - `Create` - `PhysicsMaterial`,  

Example physics material for Player object:

[//]: # (![PhysicalMaterial]&#40;Assets/ForGithub/PhysicalMaterial.PNG&#41;)
<img src="Assets/ForGithub/PhysicalMaterial.PNG" alt="Grid" height="150">

4. Configure all components using the examples below 

Example Player object components:  

[//]: # (![PlayerComponents]&#40;Assets/ForGithub/PlayerComponents.PNG&#41;)
<img src="Assets/ForGithub/PlayerComponents.PNG" alt="Grid" height="500">

Example Player colliders: 

[//]: # (![PlayerColliderExample]&#40;Assets/ForGithub/PlayerColliderExample.PNG&#41;)
<img src="Assets/ForGithub/PlayerColliderExample.PNG" alt="Grid" height="300">


## Creating a level

To create a level, drag `GridForScene` from the `Prefabs` folder to the scene, select this object and in the `Scene` window click `Open Tile Palette`,
`select the needed palette` and create the level in the `Scene` window.

[//]: # (![PlayerColliderExample]&#40;Assets/ForGithub/Grid.PNG&#41;)

<img src="Assets/ForGithub/Grid.PNG" alt="Grid" height="500">

## Changing key bindings

Go to the folder `Assets\PlatformerController2D\Runtime\Actions` - open the Action file `PlayerInputActions` -
in the opened window `change key bindings` - click `Save Assets` on the right.

![PlayerColliderExample](Assets/ForGithub/PlayerInputActions.PNG)

# Character actions and their parameters

All character capabilities and preset configuration parameters can be viewed here - [Character Actions and Preset Configuration Setup](ActionsAndPresetConfiguration.md)

# Feedback and Contributions

Enormous effort was put into developing this project, I understand that the architecture, logic, organization
in some moments falls short and requires improvement. However, the development process is not yet finished and your help is important
for the project.

> [!IMPORTANT]<br/>
> If you have feedback, suggestions for improvement, or you found bugs, please write about it.
> I would also like to address experienced developers, it would be very nice to hear your feedback about the project architecture,
> its organization and code. This will help me become better and understand many concepts, I appreciate it!


# License

MIT License. Refer to the [LICENSE.md](LICENSE) file.

Copyright (c) 2025 Vladislav-EG [Vladislav-EG](https://github.com/Vladislav-EG)














