[![Roy Theunissen](Documentation~/GithubHeader.jpg)](http://roytheunissen.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](LICENSE.md)
![GitHub Follow](https://img.shields.io/github/followers/RoyTheunissen?label=RoyTheunissen&style=social)
<a href="https://roytheunissen.com" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/globe_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/globe_light.png">
    <img alt="globe" src="globe_dark.png" width="20" height="20" />
</picture></a>
<a href="https://bsky.app/profile/roytheunissen.com" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/bluesky_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/bluesky_light.png">
    <img alt="bluesky" src="bluesky_dark.png" width="20" height="20" />
</picture></a>
<a href="https://www.youtube.com/c/r_m_theunissen" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/youtube_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/youtube_light.png">
    <img alt="youtube" src="youtube_dark.png" width="20" height="20" />
</picture></a> 
<a href="https://www.tiktok.com/@roy_theunissen" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/tiktok_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/tiktok_light.png">
    <img alt="tiktok" src="tiktok_dark.png" width="20" height="20" />
</picture></a>

_Sample for baking deformation to a texture then applying it to a mesh via a shader._

## About the Project

I saw [Simon Trümpler's Tileable Liquid Mesh on Spline](https://www.artstation.com/artwork/BmN5G6) and I was wondering how the mesh was following the spline exactly. It gave me the idea of baking the deformation to a texture somehow, which actually turned out to be really simple.

[Video](https://youtu.be/bfY7kJfgMuc)    |    [Article](https://medium.com/@roy.theunissen/gpu-spline-deformation-in-unity-a710f55f210c)

![Example](Documentation~/Example.gif)

## Getting Started

- Add the package to your Unity project (tips on how to install it are in the Installation section)
- Create the mesh you will be deforming that's pointing in the forward Z axis. Samples are provided.
  - Make sure it has a material with the `Deformation Lookup Shader` shader.
  - If the mesh does not start at (0, 0, 0) and stop at (0, 0, 1), adjust the `Z Start` and `Z End` properties accordingly.
- Implement the `IDeformationProvider` interface in the script that is responsible for the deformation that you want to bake or use the provided `BezierSpline` script.
- Add a `DeformationTextureRenderer` script to the scene. I recommend adding it next to the mesh you'll be deforming.
- Assign the script responsible for the deformation to the Deformation Provider field.
  - Assign the material for the mesh that will be deformed (this is necessary for assigning it dynamically created textures)
- You should now see the mesh deform along whatever deformation is provided via the script.

## Troubleshooting
- If the mesh is not deforming, check that `Amount` is set to 1 in the material. This feature exists for quickly toggling the effect on and off to see what it does.
- If the mesh is not deforming, check that it has the right material, shader and texture assigned
- Make sure the correct texture asset and/or material are assigned in the Deformation Texture Renderer
- If you are seeing artifacts or incorrect deformation:
  - Make sure the resolution is sufficiently large. Set the mode to Dynamic and try out larger resolutions until the artifacts disappear
  - Make sure `Z Start` and `Z End` are set correctly in the material
  - Make sure the texture's wrap mode is set to clamp. For looping meshes you may want the X-axis to be set to Repeat

## Installation

### Package Manager

Go to `Edit > Project Settings > Package Manager`. Under 'Scoped Registries' make sure there is an OpenUPM entry.

If you don't have one: click the `+` button and enter the following values:

- Name: `OpenUPM` <br />
- URL: `https://package.openupm.com` <br />

Then under 'Scope(s)' press the `+` button and add `com.roytheunissen`.

It should look something like this: <br />
![image](https://user-images.githubusercontent.com/3997055/185363839-37b3bb3d-f70c-4dbd-b30d-cc8a93b592bb.png)

<br />
All of my packages will now be available to you in the Package Manager in the 'My Registries' section and can be installed from there.
<br />


### Git Submodule

You can check out this repository as a submodule into your project's Assets folder. This is recommended if you intend to contribute to the repository yourself

### OpenUPM
The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.roytheunissen.gpusplinedeformation
```

### Manifest
You can also install via git URL by adding this entry in your **manifest.json** (make sure to end with a comma if you're adding this at the top)
```
"com.roytheunissen.gpusplinedeformation": "https://github.com/RoyTheunissen/GPU-Spline-Deformation.git"
```

### Unity Package Manager
From Window->Package Manager, click on the + sign and Add from git: 
```
https://github.com/RoyTheunissen/GPU-Spline-Deformation.git
```


## Contact
[Roy Theunissen](https://roytheunissen.com)

[roy.theunissen@live.nl](mailto:roy.theunissen@live.nl)


## Acknowledgements
* [Tileable Liquid Mesh on Spline, Simon Trümpler](https://www.artstation.com/artwork/BmN5G6) for the inspiration
* [Curves and Splines, Catlike Coding](https://catlikecoding.com/unity/tutorials/curves-and-splines/) for the spline implementation
