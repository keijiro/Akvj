#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

float3 GetEffectDirection()
{
    float sel = smoothstep(-0.02, 0.02, snoise(float2(_Time.y / 20, 0.43)));
    return lerp(float3(0, 1, 0), float3(1, 0, 0), sel);
}

float GetEffectParameter()
{
    float n = snoise(float2(_Time.y / 18, 7.334));
    return saturate(0.5 + n * 0.7);
}

float GetIntensityBoost()
{
    float n = snoise(float2(_Time.y / 16, 3.174));
    return 1 + (n < 0 ? n : n * 30);
}

void ContourLines_float
  (float3 position, float2 uv, float3 keyColor, float3 altColor, float fader,
   out float4 output)
{
    float t = _Time.y;
    float x = dot(position, GetEffectDirection());
    float rep = lerp(50, 150, GetEffectParameter());

    // Contour using derivatives
    float x2 = x * rep + t * 3;
    float fw = fwidth(x2);
    float g = saturate(1 - abs(0.5 - frac(x2)) / fw);

    // Frequency filter
    g = lerp(g, 0.4, smoothstep(0.5, 1, fw));

    float3 c = g * 6;

    // Scanner
    c += smoothstep(0.99, 1, frac(x - t * 0.31)) * keyColor * 200;
    c += smoothstep(0.99, 1, frac(x + t * 0.21)) * altColor * 200;

    // Alpha cut
    float a = snoise(uv * 100) / 2 + 0.5 < fader;

    // Boost
    c *= GetIntensityBoost();

    // Output
    output = float4(c, a);
}

void MovingSlits_float
  (float3 position, float2 uv, float3 keyColor, float3 altColor, float fader,
   out float4 output)
{
    float t = _Time.y;
    float x = dot(position, GetEffectDirection());

    // Slits
    float g = 0;
    g += snoise(float2(x * 29 - t * 1.2, t * 1.1));
    g += snoise(float2(x * 11 - t * 0.4, t * 0.7));
    g = saturate(abs(g));

    // Color selection
    float sel = snoise(float2(x * 4.31 - t * 0.21, t * 0.619));
    float3 c = lerp(keyColor, altColor, smoothstep(0.5, 0.6, sel)) * 20;

    // Boost
    c *= GetIntensityBoost();

    // Alpha threshold
    float thresh = lerp(0.1, 0.5, GetEffectParameter()) * fader;

    // Output
    output = float4(c, g < thresh);
}

void SlidingRects_float
  (float3 position, float2 uv, float3 keyColor, float3 altColor, float fader,
   out float4 output)
{
    float t = _Time.y;
    float2 dir = GetEffectDirection();
    float x = dot(position, dir);
    float y = dot(position, dir.yx);

    // Parameters
    uint seed = (y + 10) * 50;
    float wid = lerp(0.1, 0.5, Hash(seed * 2));
    float spd = lerp(0.5, 1.5, Hash(seed * 2 + 1));

    // Rows
    float p = wid * x + spd * t;
    float g = frac(p);

    // Random color
    float sel = Hash(seed * 37 + (uint)p);
    float3 c = lerp(keyColor, altColor, sel > 0.5) * 10;

    // Boost
    c *= GetIntensityBoost();

    // Alpha threshold
    float thresh = lerp(0.1, 0.5, GetEffectParameter()) * fader;

    // Output
    output = float4(c, g < thresh);
}
