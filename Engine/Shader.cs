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
		
		protected Shader()
		{
			this.id = GL.CreateProgram();
			this.uniforms = new Dictionary<string, int>();
			this.attributes = new Dictionary<string, int>();
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
			
			if (shader is VertexShader) this.vertexShader = (VertexShader)shader;
			if (shader is FragmentShader) this.fragmentShader = (FragmentShader)shader;
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
				if (loc == -1) throw new Exception(string.Format("Uniform {0} not found", name));
				uniforms.Add(name, loc);
			}
			return loc;	
		}
		
		public int GetAttributeLocation(string name)
		{
			int loc = 0;
			if (!attributes.TryGetValue(name, out loc)) {
				loc = GL.GetAttribLocation(this.id, name);
				if (loc == -1) throw new Exception(string.Format("Attribute {0} not found", name));
				attributes.Add(name, loc);
			}
			return loc;	
		}
		
		public void SetUniformMatrix(string name, ref Matrix4 matrix)
		{
			int loc = GetUniformLocation(name);
			GL.UniformMatrix4(loc, false, ref matrix);
		}
	}
}