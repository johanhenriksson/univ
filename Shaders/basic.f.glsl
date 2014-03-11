#version 330

uniform vec3 light;

in vec4 color;
in vec3 normal;

out vec4 out_color;

float remap(float value, float high1, float low1, float high2, float low2) {
    return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
}

void main() {
    float intensity = max(dot(normal, light), 0.0);
    intensity = remap(intensity, 1.0, 0.0, 0.9, 0.2);
    out_color = intensity * color;
}
