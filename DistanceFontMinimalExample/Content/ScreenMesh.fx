
//==================================== Basic properties ====================================
float4x4 WorldViewProjection;

float4 FontColor;
float2 FontWeight; //boldness, outlineFade

float4 OutlineColor;
float2 OutlineWeight;

texture Texture;
sampler2D textureSampler = sampler_state {
	Texture = (Texture);
	MagFilter = Linear; //None
	MinFilter = Linear; //None
	AddressU = Clamp;
	AddressV = Clamp;
};

//==================================== Data structures =====================================
struct ApplicationToVertex
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexToFragment
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

//======================================== Shaders =========================================
VertexToFragment MeshVertex(ApplicationToVertex input)
{
	VertexToFragment output;
	
	output.Position = mul(input.Position, WorldViewProjection);
	output.TexCoord = input.TexCoord;
	
	return output;
}

float4 DistanceFieldTexture(VertexToFragment input) : COLOR0
{
	float distance = 1 - tex2D(textureSampler, input.TexCoord).a;
	float alpha = 1 - smoothstep(FontWeight.x, FontWeight.x + FontWeight.y, distance);
	float outlineAlpha = 1 - smoothstep(OutlineWeight.x, OutlineWeight.x + OutlineWeight.y, distance);
	
	float overallAlpha = alpha + (1 - alpha) * outlineAlpha;
	float4 overallColor = lerp(OutlineColor, FontColor, alpha / overallAlpha);
	
	return float4(overallColor.rgb, overallColor.a * overallAlpha);
}

float4 AlphaTexture(VertexToFragment input) : COLOR0
{
	return float4(FontColor.rgb, FontColor.a * tex2D(textureSampler, input.TexCoord).a);
}

float4 SimpleTexture(VertexToFragment input) : COLOR0
{
	return tex2D(textureSampler, input.TexCoord);
}

//======================================= Techniques =======================================
technique Mesh
{
	pass Pass1
	{
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
		VertexShader = compile vs_4_0 MeshVertex();
		PixelShader = compile ps_4_0 DistanceFieldTexture();
	}
}