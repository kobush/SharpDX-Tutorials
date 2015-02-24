//--------------------------------------------------------------------------------------
// GLOBALS 
//--------------------------------------------------------------------------------------

// ================================== NEW CODE START =================================
cbuffer MatrixBuffer
{
	matrix WorldMatrix;
	matrix ViewMatrix;
	matrix ProjectionMatrix;
};
// ================================== NEW CODE END ====================================

//--------------------------------------------------------------------------------------
// TYPEDEFS 
//--------------------------------------------------------------------------------------
struct VertexInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
PixelInputType ColorVertexShader(VertexInputType input)
{
	PixelInputType output;

// ================================== NEW CODE START =================================
	float4 pos =  float4(input.position.xyz, 1);
	
	// Calculate the position of the vertex against the world, view, and projection matrices.
	pos = mul(pos, WorldMatrix);
	pos = mul(pos, ViewMatrix);
	output.position = mul(pos, ProjectionMatrix);
// ================================== NEW CODE END ====================================

	output.color = input.color;

	return output;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 ColorPixelShader(PixelInputType input) : SV_Target
{
	return input.color;
}

//--------------------------------------------------------------------------------------
// Techniques
//--------------------------------------------------------------------------------------
technique Render
{
	pass P0
	{
		Profile = 10.0;
		VertexShader = ColorVertexShader;
		PixelShader = ColorPixelShader;
		GeometryShader = NULL;
	}
}
