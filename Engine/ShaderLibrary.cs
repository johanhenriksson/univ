using System;
using System.IO;
using System.Collections.Generic;

namespace univ
{
    public static class ShaderLibrary
    {
        private static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
        
        public static Shader Get(string name)
        {
            Shader shader;
            if (!shaders.TryGetValue(name, out shader)) 
            {
                string vertexSource = string.Format("Shaders/{0}.v.glsl", name);
                string fragmentSource = string.Format("Shaders/{0}.f.glsl", name);
                if (!File.Exists(vertexSource))
                    throw new FileNotFoundException("Vertex shader not found");
                if (!File.Exists(fragmentSource))
                    throw new FileNotFoundException("Fragment shader not found");
                
                /* Files seem to exist, load the shader */
                shader = new Shader(vertexSource, fragmentSource);
                shaders.Add(name, shader);
            }
            return shader;
        }
    }
}

