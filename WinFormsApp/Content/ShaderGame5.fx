//--------------------------------------------------------------------------------------
// GLOBALS 
//--------------------------------------------------------------------------------------

cbuffer MatrixBuffer
{
	matrix WorldMatrix;
	matrix ViewMatrix;
	matrix ProjectionMatrix;
	float3 EyePosition;
};

cbuffer LightBuffer 
{
	float4 DiffuseColor;
	float3 SpecularColor;
	float SpecularExp;
	float3 AmbientColor;
	float3 DirLightColor;
	float3 DirToLight;
	bool UseTexture;
}

// ================================== NEW CODE START =================================
Texture2D Texture;

SamplerState samAnisotropic
{
	Filter = ANISOTROPIC;
	MaxAnisotropy = 4;
	AddressU = WRAP;
	AddressV = WRAP;
};
// ================================== NEW CODE END ====================================

//--------------------------------------------------------------------------------------
// TYPEDEFS 
//--------------------------------------------------------------------------------------
struct VertexInputType
{
	float4 position : SV_POSITION;
	float3 normal: NORMAL;
	float2 texCoord: TEXCOORD0;
};

struct PixelInputType
{
	float4 positionH : SV_POSITION;
	float4 positionW : POSITION;
	float3 normalW : NORMAL;
	float2 texCoord: TEXCOORD0;
	float4 color : COLOR;
};


//--------------------------------------------------------------------------------------
// Lighting
//--------------------------------------------------------------------------------------

struct Material
{
	float3 Normal;
	float3 DiffuseColor;
	float3 SpecColor;
	float SpecExp;
};

float3 CalcAmbient(Material material)
{
	// apply the ambient value to the Color
	return material.DiffuseColor * AmbientColor;
}

float3 CalcDirectional(float3 position, Material material)
{
	// Phong diffuse
	float NDotL = dot(DirToLight, material.Normal);
	float intensity = saturate(NDotL);
	float3 finalColor = intensity * material.DiffuseColor * DirLightColor.rgb;

	if (material.SpecExp > 0)
	{
		float3 ToEye = EyePosition.xyz - position;
			ToEye = normalize(ToEye);

		// Blinn specular
		/*float3 HalfWay = normalize(ToEye + DirToLight);
		float NDotH = dot(HalfWay, material.Normal);
		float specFactor = pow(saturate(NDotH), material.SpecExp);*/

		// Phong specular
		float3 r = reflect(-DirToLight, material.Normal);
		float specFactor = pow(max(dot(r, ToEye), 0.0), material.SpecExp);

		finalColor += specFactor * material.SpecColor * DirLightColor.rgb;
	}

	return finalColor;
}

//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
PixelInputType ColorVertexShader(VertexInputType input)
{
	PixelInputType output;

	float4 pos =  float4(input.position.xyz, 1);
	
	// Calculate the position of the vertex against the world, view, and projection matrices.
	pos = mul(pos, WorldMatrix);
	output.positionW = pos;

	pos = mul(pos, ViewMatrix);
	output.positionH = mul(pos, ProjectionMatrix);

	output.normalW = mul(input.normal, (float3x3)WorldMatrix);

	output.color = DiffuseColor;
	output.texCoord = input.texCoord;

	return output;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 ColorPixelShader(PixelInputType input) : SV_Target
{
// ================================== NEW CODE START =================================
	float4 diffuseColor = input.color;
	if (UseTexture)
	{
		// Sample texture.
		diffuseColor = Texture.Sample(samAnisotropic, input.texCoord);
	}

	return diffuseColor;
// ================================== NEW CODE END ====================================
}

float4 LightPixelShader(PixelInputType input) : SV_Target
{
// ================================== NEW CODE START =================================
	float4 diffuseColor = input.color;
	if (UseTexture)
	{
		// Sample texture.
		diffuseColor = Texture.Sample(samAnisotropic, input.texCoord);
	}

	Material material;
	material.Normal = normalize(input.normalW);
	material.DiffuseColor = diffuseColor.rgb;
	material.SpecColor = SpecularColor;
	material.SpecExp = SpecularExp;

	float3 finalColor = CalcAmbient(material);
	finalColor += CalcDirectional(input.positionW, material);

	return float4(finalColor.rgb, diffuseColor.a);
// ================================== NEW CODE END ====================================
}

//--------------------------------------------------------------------------------------
// Techniques
//--------------------------------------------------------------------------------------
technique RenderInputColor
{
	pass P0
	{
		Profile = 10.0;
		VertexShader = ColorVertexShader;
		PixelShader = ColorPixelShader;
		GeometryShader = NULL;
	}
}

technique RenderPixelLight
{
	pass P0
	{
		Profile = 10.0;
		VertexShader = ColorVertexShader;
		PixelShader = LightPixelShader;
		GeometryShader = NULL;
	}
}
