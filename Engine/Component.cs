using System;
using System.Collections.Generic;
using OpenTK;

namespace univ
{
    public class Component
    {
        public Vector3 Position { get { return this.position; } }
     
        protected Vector3 position;
        protected List<Component> children;
     
        public Component()
        {
            this.position = Vector3.Zero;
            this.children = new List<Component>();
        }
     
        public void Attach(Component child)
        {
            this.children.Add(child);
        }
     
        public virtual void Move(Vector3 offset)
        {
            Move(offset.X, offset.Y, offset.Z);
        }
     
        public virtual void Move(float x, float y, float z)
        {
            this.position.X += x;
            this.position.Y += y;
            this.position.Z += z;
        }
     
        public virtual void Update(float dt)
        {
            foreach (Component child in this.children)
                child.Update(dt);
        }
    } 
}