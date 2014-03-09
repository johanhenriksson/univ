using System;
using OpenTK;

namespace univ
{
    public class DrawEventArgs
    {
        public Scene Scene { get { return this.scene; } }
        public Camera Camera { get { return this.camera; } }

        public Matrix4 ModelMatrix;
        protected Scene scene;
        protected Camera camera;
     
        public DrawEventArgs(Scene scene, Camera camera, Matrix4 model)
        {
            this.scene = scene;
            this.camera = camera;
            this.ModelMatrix = model;
        }
    }
}

