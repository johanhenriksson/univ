# univ engine

Yet another attempt at creating some sort of 3D engine, this time in C#/OpenTK. 

## Todo

Port to OpenGL 3.2 for OSX compability?
 * Not sure if 4.0+ is really needed for anything in particular. Look into this asap

Camera:
 * Implement rotation
 * Direction relative movement (turn with Q/E)
 * Mouse movement

Scene Graph:
 * Use Scene class
 * Attach all objects to scenes

Shader Library:
 * Automated shader loading based on a name. Could probably be a static class since the shader files are global by nature, and since only one instance of a shader should ever be created. Also, there is no way to test them.

Model Loader:
 * Implement a simple  OBJ loader. This will also require implementing texturing etc

Basic User Interface:
 * At this point we probably need some basic user interface to aid debugging etc.

Networking?
 * Early networking could be kinda cool. Some kind of basic server to synchronize dynamic objects/players between clients.

Skeletal Animations:
 * Implement support for bone based animations. Will require implementing a model loader for a more complex model format.
