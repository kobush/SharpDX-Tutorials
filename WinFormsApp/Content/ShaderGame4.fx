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

// ================================== NEW CODE START =================================
cbuffer LightBuffer 
{
	float4 DiffuseColor;
	float3 SpecularColor;
	float SpecularExp;
	float3 AmbientColor;
	float3 DirLightColor;
	float3 DirToLight;
}
// ================================== NEW CODE END ====================================

//--------------------------------------------------------------------------------------
// TYPEDEFS 
//--------------------------------------------------------------------------------------

// ================================== NEW CODE START =================================
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
// ================================== NEW CODE END ====================================

//--------------------------------------------------------------------------------------
// Lighting
//--------------------------------------------------------------------------------------

// ================================== NEW CODE START =================================

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
// ================================== NEW CODE END ====================================

//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
PixelInputType ColorVertexShader(VertexInputType input)
{
	PixelInputType output;

	float4 pos =  float4(input.position.xyz, 1);
	
// ================================== NEW CODE START =================================
	// Calculate the position of the vertex against the world, view, and projection matrices.
	pos = mul(pos, WorldMatrix); // world space
	output.positionW = pos; 

	pos = mul(pos, ViewMatrix); // camera space
	pos = mul(pos, ProjectionMatrix); // projected space
	output.positionH = pos; 

	output.normalW = mul(input.normal, (float3x3)WorldMatrix);

	output.color = DiffuseColor;
	output.texCoord = input.texCoord;
// ================================== NEW CODE END ====================================

	return output;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 ColorPixelShader(PixelInputType input) : SV_Target
{
	return input.color;
}

// ================================== NEW CODE START =================================
float4 LightPixelShader(PixelInputType input) : SV_Target
{
	Material material;
	material.Normal = normalize(input.normalW);
	material.DiffuseColor = input.color.rgb;
	material.SpecColor = SpecularColor;
	material.SpecExp = SpecularExp;

	float3 finalColor = CalcAmbient(material);
	finalColor += CalcDirectional(input.positionW, material);

	return float4(finalColor.rgb, input.color.a);
}
// ================================== NEW CODE END ====================================

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

// ================================== NEW CODE START =================================
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
// ================================== NEW CODE END ====================================
