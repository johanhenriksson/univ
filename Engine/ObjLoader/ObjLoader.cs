using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using univ.Engine.Geometry;

namespace univ
{
    public class ObjLoader
    {
        Scanner scanner;
        StreamReader reader;
        List<Vector3> verticies;
        List<Vector3> vertexNormals;
        List<UInt3> faces;

        public ObjLoader(string path)
        {
            scanner = new Scanner();
            reader = new StreamReader(path);
            verticies = new List<Vector3>();
            vertexNormals = new List<Vector3>();
            faces = new List<UInt3>();

            int ln = 0;
            string line;

            while ((line = reader.ReadLine()) != null) {
                ln++;
                line = line.Trim();
                if (line.Length == 0)
                    continue;

                scanner.Feed(line);

                string type = scanner.NextWord().ToLower();
                switch (type) 
                {
                    case "#":
                        continue;
                    case "v":
                        verticies.Add(scanner.NextVector3());
                        break;
                    case "vn":
                        vertexNormals.Add(scanner.NextVector3().Normalized());
                        break;
                    case "f":
                        faces.Add(scanner.NextUInt3());
                        break;
                }

            }
            
            Console.WriteLine("Verticies: {0}", verticies.Count);
            Console.WriteLine("Normals: {0}", vertexNormals.Count);
            Console.WriteLine("Faces: {0}", faces.Count);
        }

        protected void readFace()
        {
            string v1 = scanner.NextWord();
            if (v1.Contains("/")) {

            }
        }

        public Model Assemble()
        {
            Vector3[] vertexData = verticies.ToArray();
            Vector3[] normals = vertexNormals.ToArray();
            Byte3[] colors = new Byte3[vertexData.Length];
            ushort[] indices = new ushort[faces.Count * 3];
            
            Byte3 color = new Byte3(210, 35, 35);
            for(int i = 0; i < colors.Length; i++)
                colors[i] = color;
            
            int j = 0;
            foreach(UInt3 f in faces) {
                indices[j++] = (ushort)(f.X - 1);
                indices[j++] = (ushort)(f.Y - 1);
                indices[j++] = (ushort)(f.Z - 1);
            }
            
            VertexArray mesh = new VertexArray();
            mesh.CreateBuffer("vertex").BufferData<Vector3>(ref vertexData);
            mesh.CreateBuffer("normal").BufferData<Vector3>(ref normals);
            mesh.CreateBuffer("colors").BufferData<Byte3>(ref colors);
            mesh.CreateBuffer("index", BufferTarget.ElementArrayBuffer).BufferData<ushort>(ref indices);
            
            Model model = new Model(faces.Count, mesh);
            
            mesh.AddPointer("vertex",
                new VertexAttribute(model.Shader, "vPosition", VertexAttribPointerType.Float, 3, 0, false));
            mesh.AddPointer("colors",
                new VertexAttribute(model.Shader, "vColor", VertexAttribPointerType.UnsignedByte, 3, 0, true));
            mesh.AddPointer("normal",
                new VertexAttribute(model.Shader, "vNormal", VertexAttribPointerType.Float, 3, 0, false));
                
            return model;
        }
    }
}

