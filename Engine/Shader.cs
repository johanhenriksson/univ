using System;
using System.Collections.Generic; 
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
    public class Shader
    {
        public int ID { get { return this.id; } }
        public VertexShader Vertex { get { return this.vertexShader; } }
        public FragmentShader Fragment { get { return this.fragmentShader; } }
 
        int id;
        VertexShader vertexShader;
        FragmentShader fragmentShader;
        protected Dictionary<string, int> uniforms;
        protected Dictionary<string, int> attributes;
        protected Dictionary<string, UniformBuffer> blocks;
     
        protected Shader()
        {
            this.id = GL.CreateProgram();
            this.uniforms = new Dictionary<string, int>();
            this.attributes = new Dictionary<string, int>();
            this.blocks = new Dictionary<string, UniformBuffer>();
        }
     
        public Shader(string vertexSource, string fragmentSource)
         : this()
        {
            this.vertexShader = new VertexShader(vertexSource);
            this.fragmentShader = new FragmentShader(fragmentSource);
            Attach(this.vertexShader);
            Attach(this.fragmentShader);
            Link();
        }
     
        ~Shader()
        {
            GL.DeleteProgram(this.id);
        }
     
        public void Use()
        {
            GL.UseProgram(this.id);
        }
     
        public void Attach(ShaderComponent shader)
        {
            if (!shader.Compiled)
                shader.Compile();
            GL.AttachShader(this.id, shader.ID);
         
            if (shader is VertexShader)
                this.vertexShader = (VertexShader)shader;
            if (shader is FragmentShader)
                this.fragmentShader = (FragmentShader)shader;
        }
     
        public void Link()
        {
            int status;
            GL.LinkProgram(this.id);
            GL.GetProgram(this.id, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
                throw new Exception(string.Format("Linker error: {0}", GL.GetProgramInfoLog(this.id)));
        }
     
        public int GetUniformLocation(string name)
        {
            int loc = 0;
            if (!uniforms.TryGetValue(name, out loc)) {
                loc = GL.GetUniformLocation(this.id, name);
                if (loc == -1)
                    throw new Exception(string.Format("Uniform {0} not found", name));
                uniforms.Add(name, loc);
            }
            return loc;  
        }
     
        public int GetAttributeLocation(string name)
        {
            int loc = 0;
            if (!attributes.TryGetValue(name, out loc)) {
                loc = GL.GetAttribLocation(this.id, name);
                if (loc == -1)
                    throw new Exception(string.Format("Attribute {0} not found", name));
                attributes.Add(name, loc);
            }
            return loc;  
        }
        
        public void SetFloat(string name, float floatval)
        {
            int loc = GetUniformLocation(name);
            GL.Uniform1(loc, floatval);
        }
     
        public void SetMatrix(string name, ref Matrix4 matrix)
        {
            int loc = GetUniformLocation(name);
            GL.UniformMatrix4(loc, false, ref matrix);
        }
        
        public void SetMatrix3(string name, ref Matrix3 matrix)
        {
            int loc = GetUniformLocation(name);
            GL.UniformMatrix3(loc, false, ref matrix);
        }
        
        public void SetVector3(string name, ref Vector3 vector)
        {
            int loc = GetUniformLocation(name);
            GL.Uniform3(loc, ref vector);
        }
        
        public void SetVector3(string name, Vector3 vector)
        {
            int loc = GetUniformLocation(name);
            GL.Uniform3(loc, vector);
        }
        
        public void SetBlock<T>(string name, ref T data) where T : struct
        {
            T[] array = new T[] { data }; 
            SetBlock<T>(name, ref array);
        }
        
        public void SetBlock<T>(string name, ref T[] data) where T : struct
        {
            UniformBuffer block;
            if (!blocks.TryGetValue(name, out block)) {
                block = new UniformBuffer(this);
                blocks.Add(name, block);
            }
            block.BufferData<T>(ref data); 
        }
        
        
        /* Piss fucking ugly hacks messing up my beautiful class */
       
        public void SetBaseLight(string name, BaseLight light)
        {
            SetVector3(name + ".color", light.Color);
            SetFloat(name + ".intensity", light.Intensity);
        }
        
        public void SetDirectionalLight(string name, DirectionalLight light)
        {
            SetBaseLight(name + ".base", light.Light);
            SetVector3(name + ".direction", light.Direction);
        }
    }
}