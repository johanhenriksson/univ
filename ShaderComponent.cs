using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public class ShaderComponent
	{
		public int ID { get { return this.id; } }
		public bool Compiled { get { return this.compiled; } }
		public ShaderType Type { get { return this.type; } }
		
		protected int id;
		protected bool compiled;
		protected string filename;
		protected ShaderType type;
		
		public ShaderComponent(ShaderType type, string filename)
		{
			this.type = type;
			this.filename = filename;
			this.id = GL.CreateShader(this.type);
			
			this.load();
		}
		
		~ShaderComponent()
		{
			GL.DeleteShader(this.id);
		}
		
		protected void load()
		{
			using(StreamReader sr = new StreamReader(this.filename)) {
				GL.ShaderSource(this.id, sr.ReadToEnd());
			}
		}
		
		public void Compile()
		{
			int status;
			GL.CompileShader(this.id);
			GL.GetShader(this.id, ShaderParameter.CompileStatus, out status);
			if (status == 0)
				throw new Exception(string.Format("Shader compilation error: {0}", GL.GetShaderInfoLog(this.id)));
			this.compiled = true;
		}
		
		
	}
	
	public class VertexShader : ShaderComponent
	{
		public VertexShader(string filename) : 
			base(ShaderType.VertexShader, filename)
		{
		}
	}
	
	public class FragmentShader : ShaderComponent
	{
		public FragmentShader(string filename) : 
			base(ShaderType.FragmentShader, filename)
		{
		}
	}
}