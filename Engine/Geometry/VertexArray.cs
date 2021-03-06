using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ.Engine.Geometry
{
	public class VertexArray
	{
		public int Elements { get { return this.elements; } }
		public PrimitiveType Primitive { get { return this.primitive; } }
		
		protected int id;
		protected int elements;
		protected PrimitiveType primitive;
		protected Dictionary<string, GLBuffer> arrays;
		protected Dictionary<int, VertexAttribute> attributes;
		
		public VertexArray() : this(PrimitiveType.Triangles) { }
		
		public VertexArray(PrimitiveType primitive)
		{
			this.id = GL.GenVertexArray();
			this.primitive = primitive;
			this.arrays = new Dictionary<string, GLBuffer>();
			this.attributes = new Dictionary<int, VertexAttribute>();
		}
		
		~VertexArray()
		{   
			GL.DeleteVertexArray(this.id);
		}
		
		public void Bind()
		{
			GL.BindVertexArray(this.id);
		}
		
		public static void Unbind()
		{
			GL.BindVertexArray(0);
		}
		
        public GLBuffer CreateBuffer(string bufferName) {
           return this.CreateBuffer(bufferName, BufferTarget.ArrayBuffer);
        }
        
		public GLBuffer CreateBuffer(string bufferName, BufferTarget target)
		{
			this.Bind();
			GLBuffer buffer = new GLBuffer(target);
			this.arrays.Add(bufferName, buffer);
			return buffer;
		}
		
		public void AddPointer(string bufferName, VertexAttribute pointer)
		{
			GLBuffer buffer = this.arrays[bufferName];
			this.attributes.Add(pointer.Location, pointer);
			buffer.Bind();
			pointer.Point();
			buffer.Unbind();
		}
		
		public void Enable()
		{
			foreach(VertexAttribute attribute in this.attributes.Values)
				attribute.Enable();
		}
		
		public void Disable()
		{
			foreach(VertexAttribute attribute in this.attributes.Values)
				attribute.Disable();
		}
		
		public void Draw(int elements)
		{
			Bind();
			Enable();
			GL.DrawArrays(primitive, 0, elements);	
			Disable();
			Unbind();
		}
		
		public void DrawElements(int elements, DrawElementsType type)
		{
			Bind();
			Enable();
			GL.DrawElements(BeginMode.Triangles, elements, type, 0);
			Disable();
			Unbind();
		}
	}
}

