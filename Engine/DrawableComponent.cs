using System;
using OpenTK;

namespace univ
{
    public class DrawableComponent : Component
    {
        public Matrix4 ModelMatrix { get { return this.modelMatrix; } }
        public Vector3 Rotation { get { return this.rotation; } }
        public Vector3 Scale { get { return this.scale; } }
        public Shader Shader { get { return this.shader; } }
     
        /* Transformation Matricies */
        protected Matrix4 modelMatrix;
        protected Matrix4 translationMatrix;
        protected Matrix4 rotationMatrix;
        protected Matrix4 scaleMatrix;
        
        /* Transformations */
        protected Quaternion orientation;
        protected Vector3 rotation;
        protected Vector3 scale;
        protected Shader shader;
         
        public DrawableComponent(Shader shader)
         : base()
        {
            this.modelMatrix = Matrix4.Identity;
            this.orientation = Quaternion.Identity;
            this.rotation = Vector3.Zero;
            this.scale = Vector3.One;
            this.shader = shader;
            RecalculateMatrix();
        }
     
        public override void Move(float x, float y, float z)
        {
            base.Move(x, y, z);
            RecalculateMatrix();
        }
     
        /// <summary>
        /// Rotates the component around the x, y and z axis. Angles specified in degrees.
        /// </summary>
        public void Rotate(float x, float y, float z)
        {
            this.rotation.X += (float)(Math.PI / 180 * x);
            this.rotation.Y += (float)(Math.PI / 180 * y);
            this.rotation.Z += (float)(Math.PI / 180 * z);

            Quaternion qx = Quaternion.FromAxisAngle(Vector3.UnitX, this.rotation.X);
            Quaternion qy = Quaternion.FromAxisAngle(Vector3.UnitY, this.rotation.Y);
            Quaternion qz = Quaternion.FromAxisAngle(Vector3.UnitZ, this.rotation.Z);
         
            this.orientation = qz * qy * qx;
            this.orientation.Normalize();
            RecalculateMatrix();
        }
     
        public void Rescale(float scale)
        {
            Rescale(scale, scale, scale);        
        }
     
        public void Rescale(float x, float y, float z)
        {
            this.scale.X *= x;
            this.scale.Y *= y;
            this.scale.Z *= z;
            RecalculateMatrix();
        }
     
        /// <summary>
        /// Recalculates the transformation matricies for this component.
        /// </summary>
        public void RecalculateMatrix()
        {
            Matrix4.CreateFromQuaternion(ref this.orientation, out this.rotationMatrix);
            Matrix4.CreateScale(ref this.scale, out this.scaleMatrix);
            Matrix4.CreateTranslation(ref this.position, out this.translationMatrix);
         
            Matrix4.Mult(ref scaleMatrix, ref rotationMatrix, out modelMatrix); // S*R
            Matrix4.Mult(ref modelMatrix, ref translationMatrix, out modelMatrix); // (S*R)*T
        }
     
        public virtual void Draw(DrawEventArgs e)
        {
            Matrix4 model = this.modelMatrix * e.ModelMatrix;
            DrawEventArgs child_event = new DrawEventArgs(e.Scene, e.Camera, model);
            foreach (Component child in this.children)
                if (child is DrawableComponent)
                    ((DrawableComponent)child).Draw(child_event);
        }
    }        
}