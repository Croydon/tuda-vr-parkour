# Unity VR project for Meta Quest 2: Study Participant 42

## Requirements

  * Unity 2022.3.19f1
  * Meta Quest 2 (Quest 3 might work with some small build settings adjustments)


## Additional Credits

  * `Assets/HQ Explisions Pack Free/`: [HQ Explosions Pack FREE](https://assetstore.unity.com/packages/vfx/particles/fire-explosions/hq-explosions-pack-free-263326)
  * `Assets/croydon/Sounds/tts/`: Generated with [Mozilla's Text-To-Speech](https://github.com/mozilla/TTS)
  * `Assets/croydon/Images/`: Some are created with [Excalidraw](https://github.com/excalidraw/excalidraw)
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
