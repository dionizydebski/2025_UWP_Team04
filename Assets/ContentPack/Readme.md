# Basic 3D Content Pack for Tower Defense game.

Prefabs folder contains ready to use assets:
## Prefabs/Props
+P_Base - base to defend

## Prefabs/Props/Towers

All models for towers and upgrades

+ P_Tower1_Base
+ P_Tower1_Upgrade1
+ P_Tower1_Upgrade2
+ P_Tower1_Weapon
+ P_Tower2_Base
+ P_Tower2_Upgrade1
+ P_Tower2_Upgrade2
+ P_Tower2_Weapon

Weapons are separate prefabs for easier configuration.
Every tower upgrade is a separate tower model.

## Prefabs/Props/Environment

Environmental models

+ P_Bush
+ P_Bush1
+ P_Road1
+ P_Road2
+ P_Road3
+ P_Rock1
+ P_Rock2
+ P_Tree1
+ P_Tree2

## Prefabs/Characters

Prefabs for 2 enemy types: Golem and Ent

+ P_Ent
+ P_Golem

Characters have already a default animation controller.
Animations for the characters can be found at: Content/Characters. Inside of every .fbx file, displayed as prefab in Unity Project View, there is an anim file which can be used in Animator component. By default animations are rigged for a specific character and it is mentioned in the name of the file (i.e. Ent_Attack is an attack only for Ent).

Victory animation works for both characters.

In case of need for more animations, visit www.mixamo.com service and download an animation from the library. Before assigning it to character make sure to change the Rig type to Humanoid in Unity fbx import settings in Rig tab.