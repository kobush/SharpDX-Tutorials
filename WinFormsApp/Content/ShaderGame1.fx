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

	output.position = input.position;
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
