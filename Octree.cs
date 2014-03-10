using System;
using OpenTK;

namespace univ
{
    public class Octree : Component
    {
        Shader shader;
        Component[] nodes = new Component[8];
        Matrix4[] transforms = new Matrix4[8];
        
        public Octree(Shader shader)
        {
            this.shader = shader;
            for(int i = 0; i < 8; i++) {
                nodes[i] = new Voxel(shader);
                transforms[i] = Matrix4.CreateScale(0.5f);
                
                /* Calculate translation */
                Vector3 offset = new Vector3(
                    ((i & 4) >> 2) * 0.5f, 
                    ((i & 2) >> 1) * 0.5f, 
                    (i & 1) * 0.5f
                );
                Matrix4 position = Matrix4.CreateTranslation(offset);
    
                Matrix4.Mult(ref transforms[i], ref position, out transforms[i]);
            }
        }
        
        public Component Child(int x, int y, int z)
        {
            int loc = (x << 2) + (y << 1) + z;
            if (loc < 0 || loc > 7) throw new Exception("Invalid octree location");
            return nodes[loc];
        }
        
        public void Split(int x, int y, int z)
        {
            int loc = (x << 2) + (y << 1) + z;
            if (nodes[loc] is Octree)
                ((Octree)nodes[loc]).Split(x,y,z);
            else
                nodes[loc] = new Octree(this.shader);
        }
        
        public void Delete(int x, int y, int z)
        {
           int loc = (x << 2) + (y << 1) + z;
            if (nodes[loc] is Octree)
                ((Octree)nodes[loc]).Delete(x,y,z);
            else
                nodes[loc] = null; 
        }
        
        public override void Draw(DrawEventArgs e)
        {
            Matrix4 base_model;
            Matrix4.Mult(ref this.modelMatrix, ref e.ModelMatrix, out base_model);
            for(int i = 0; i < 8; i++)
            {
                if (nodes[i] == null) continue;
                Matrix4 model = transforms[i] * base_model;
                DrawEventArgs ec = new DrawEventArgs(e.Scene, e.Camera, model);
                nodes[i].Draw(ec);
            }
        }
    }
}

