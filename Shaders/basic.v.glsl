#version 330

in vec3 vPosition;
in vec3 vColor;
in vec3 vNormal;

out vec4 color;
out vec3 normal;

uniform mat3 model;
uniform mat4 mvp;
uniform mat3 G;

varying vec3 fragmentPosition;

vec3 gamma(vec3 color){
    return pow(color, vec3(1.0/1.8));
}

void main() {
    fragmentPosition = model * vPosition;
    gl_Position = mvp * vec4(vPosition, 1.0);
    
    // Gamma-corrected color
    color = vec4(gamma(vColor), 1.0);
    
    // Normalized normal vector
    normal = normalize(G * vNormal);
}


