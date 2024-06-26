# Unity VR project for Meta Quest: Study Participant 42

## Requirements

### Build Requirements

  * Unity 2022.3.19f1

### Runtime Requirements

  * A Meta Quest device
    * Meta Quest 2 is tested for all versions <= 0.10.0
    * Meta Quest 3 is tested for all versions >= 0.10.0


## Video Walkthrough

<a href="https://www.youtube.com/watch?v=WJ9H1r-tjjA" target="_blank"><img src="https://img.youtube.com/vi/WJ9H1r-tjjA/0.jpg" alt="VR Walkthrough Video with the view of inside VR and outside" width="240" height="180" border="10"></a>


## How To Play

  * You need to configure flying first (see `Control`)
  * You are supposed to stand in one place
  * You can rotate freely on your position
  * Small steps in some directions to reach something a bit better should be fine, but if you are leaving your original position too much you need to re-set your position with the Oculus button. Otherwise the physical and virtual player position do not match anymore perfectly and collecting coins can become a problem.
  * Goals; you have 10 minutes to:
    * Do as many rounds as possible
    * Collect as many coins as possible
    * Perform the object positioning tasks as preceise as possible


## Control

  * Before flying is enabled it needs to be configured
    * Put your right hand with the controller up as far up as you can, then press `X`
    * Then, put your right hand with the controler as far down as you can (while still standing straight), then press `A`
    * If done both, you should be able to fly up, down or hold your position, depending on the height of your right controller (or hand for that matter if hand-tracking is enabled, but I don't recommend that)
  * Press the `Left Trigger Button` to move in the horizontol direction of where you are looking at
  * `B` or `Y` to reset to last save point, for cases when the player flew/ran complete off the road and does not find a quick way back
  * If you moved too much physically away from your original position, use the `Oculus` button to return to the logical position of your player object. You might notice that this is necessary, if you having troubles collection coins that you should be able to get or if you are standing far too away from the portals.


## Additional Credits

  * `Assets/HQ Explisions Pack Free/`: [HQ Explosions Pack FREE](https://assetstore.unity.com/packages/vfx/particles/fire-explosions/hq-explosions-pack-free-263326)
  * `Assets/croydon/Sounds/tts/`: Generated with [Mozilla's Text-To-Speech](https://github.com/mozilla/TTS)
  * `Assets/croydon/Images/`:
    * Some are created with [Excalidraw](https://github.com/excalidraw/excalidraw)
      * `study_facility_wall.png`
    * `lock.png`: [cc0-icons](https://cc0-icons.jonh.eu/)
  * Package [Tilia.Visuals.Vignette.Unity](https://github.com/ExtendRealityLtd/Tilia.Visuals.Vignette.Unity) - MIT licensed


## License

All original work in this repository is licensed under the terms of the MIT license.
However, this does not include huge parts of this repository, including, but not limiting to Unity, Unity packages and third-party assets.


## Original Readme: VR-locomotion-parkour

### Demo Video

2022 with Object Interaction Task

<a href="http://www.youtube.com/watch?feature=player_embedded&v=ZVDoHTefdR0" target="_blank"><img src="http://img.youtube.com/vi/ZVDoHTefdR0/0.jpg" alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10"></a>


2021 Version

<a href="http://www.youtube.com/watch?feature=player_embedded&v=5s-vTwTFc7U" target="_blank"><img src="http://img.youtube.com/vi/5s-vTwTFc7U/0.jpg" alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10"></a>


### How to Start

```{bash}
git clone https://github.com/wenjietseng/VR-locomotion-parkour.git
```

- download the GitHub repo and open __VRParkour__ folder as a Unity project
- implement your locomotion technique in `LocomotionTechnique.cs`
- Selection...
- play and see how fast and how many coins you can get!


### Misc

#### Core

- Unity 2021.3.10f1 LTS
- Oculus Integration 46.0 (Oculus Utilities v1.78.0, OVRPlugin v1.78.0, SDK v1.78.0)
- Oculus XR Plugin 3.0.2

#### Rendering settings

- [ref](https://developer.oculus.com/documentation/unity/unity-conf-settings/#rendering-settings)
- Color space: Linear
- OpenGL ES 3.0
- Multithreaded Rendering

#### Configuration settings

- [ref](https://developer.oculus.com/documentation/unity/unity-conf-settings/#configuration-settings)
- Scripting backend: IL2CPP
- Target architectures: ARM64

#### Cybersickness reduction

- ~~[Ginger VR](https://github.com/angsamuel/GingerVR) Note: This repo seems not being maintained anymore. Still, it can be a reference for implementing your cybersickness reduction technique.~~

#### Scene

- [Low Poly Ultimate Pack](https://assetstore.unity.com/packages/3d/props/low-poly-ultimate-pack-54733)
- [Forest - Low Poly Toon Battle Arena / Tower Defense Pack](https://assetstore.unity.com/packages/3d/environments/forest-low-poly-toon-battle-arena-tower-defense-pack-100080)

#### Sound effect and music

- Winner Winner Funky Chicken Dinner (YouTube Audio Library)
- [tone beep](https://freesound.org/people/pan14/sounds/263133/)
- [crowd yay](https://freesound.org/people/mlteenie/sounds/169233/)


### License

MIT
