#version 330

#define MAX_POINT_LIGHTS 4

layout(std140) struct BaseLight
{
    vec3 color;
    float intensity;
};

layout(std140) struct DirectionalLight
{
    BaseLight base;
    vec3 direction;
};

layout(std140) struct Attenuation
{
    float constant;
    float linear;
    float square;
};

layout(std140) struct PointLight
{
    BaseLight base;
    Attenuation attenuation;
    vec3 position;
};

/* Lighting */
uniform BaseLight ambient;
uniform DirectionalLight sunlight;
uniform PointLight pointLights[MAX_POINT_LIGHTS];

uniform vec3 eye;

in vec4 color;
in vec3 normal;

out vec4 out_color;

varying vec3 fragmentPosition;

float remap(float value, float high1, float low1, float high2, float low2) {
    return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
}

/* Calculates base lighting based on color, intensity and direction */
vec4 calcLight(BaseLight light, vec3 direction)
{
    float intensity = light.intensity * max(dot(-direction, normal), 0.0);
    return vec4(intensity * light.color, 1.0);
}

/* Helper method for calculating directional lights */
vec4 calcDirectionalLight(DirectionalLight light) 
{
    return calcLight(light.base, light.direction);
}

/* Calculates light contribution from a point light source */
vec4 calcPointLight(PointLight light)
{
    vec3 direction = fragmentPosition - light.position;
    float distance = length(direction);
    direction = normalize(direction);

    vec4 color = calcLight(light.base, direction);
    
    /* Calculate attenuation (fallof) due to distance */
    float attenuation = light.attenuation.constant +
                        light.attenuation.linear * distance +
                        light.attenuation.square * distance * distance +
                        0.0001;
    
    return color / attenuation;
}

void main() 
{
    vec4 light = ambient.intensity * vec4(ambient.color, 1.0);
    
    /* Diffuse Lighting */
    light += calcDirectionalLight(sunlight);
    
    /* Point Lights */
    for(int i = 0; i < MAX_POINT_LIGHTS; i++)
    {
        light += calcPointLight(pointLights[i]);
    }
    
    /* Specular lighting */
    vec3 toCamera = normalize(eye - fragmentPosition);
    vec3 halfVec = normalize(toCamera - sunlight.direction);
    float specular = dot(normal, halfVec);
    
    if (specular < 0.0)
        specular = 0.0;
        
    float a = 15.0;
    float c = 0.8;
    specular = c * pow(specular, a);
    
    out_color = light * color;
}
