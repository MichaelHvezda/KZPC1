texture MainTexture;
texture BackGround;
texture MeansTexture;
float width;
float height;

float3 cent1;
float3 cent2;
float3 cent3;

sampler textureSampler = sampler_state
{
	Texture = <MainTexture>;
};

sampler textureSamplerBackground = sampler_state
{
	Texture = <BackGround>;
};

sampler textureSamplerMeansTexture = sampler_state
{
	Texture = <MeansTexture>;
};

struct VertexShaderInput
{
	float4 Position : SV_Position;
	float2 TextureCoordinate : TEXCOORD0;
};


struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float2 TextureCoordinate : TEXCOORD0;
};

struct PixelShaderOutput
{
	float4 C1 : COLOR0;
	float4 C2 : COLOR1;
	float4 C3 : COLOR2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = input.Position;
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float NaDruhou(float firstNumber, float scndNumber) {
	return ((firstNumber - scndNumber) * (firstNumber - scndNumber));
}

float Vzdalenost(float3 col, float3 cent) {
	float pomZaporna;
	float pomKladna = NaDruhou(col.x, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	if (col.x < cent.x) {
		pomZaporna = NaDruhou(col.x, cent.x - 360) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	}
	else {
		pomZaporna = NaDruhou(col.x - 360, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	}

	if (pomKladna <= pomZaporna) {
		return pomKladna;
	}
	else {
		return pomZaporna;
	}
}

float3 RgbToHsb(float3 a)
{
	float r = ((float)a.r);
	float g = ((float)a.g);
	float b = ((float)a.b);

	float maxn = max(r, max(g, b));
	float minn = min(r, min(g, b));

	float h = 0.0;
	if (maxn == r && g >= b)
	{
		if (maxn - minn == 0.0)
		{
			h = 0.0;
		}else
		{
			h = 60.0 * ((g - b) / (maxn - minn));
		}
	}else if (maxn == r && g < b)
	{
		h = 60.0 * ((g - b) / (maxn - minn)) + 360.0;
	}else if (maxn == g)
	{
		h = 60.0 * ((b - r) / (maxn - minn)) + 120.0;
	}else if (maxn == b)
	{
		h = 60.0 * ((r - g) / (maxn - minn)) + 240.0;
	}

	float s = (maxn == 0.0) ? 0.0 : (1.0 - ((float)minn / (float)maxn));
	//h - 0/360
	//s - 0/1
	//maxn - 0/1
	return float3((float)h, (float)s, (float)maxn);
}

float3 hsbToRGB(float3 a) {
	float c, x, m;
	int pom;

	float h = a.x ;
	float s = a.y / 100.0;
	float b = a.z / 100.0;

	c = b * s;
	x = c * (1 - abs((h / 60) % 2 - 1));
	m = b - c;

	pom = (int)a.x / 60;

	float3 pomTuple;

	switch (pom) {
	case 0:  pomTuple = float3(c, x, 0.0);
		break;
	case 1:  pomTuple = float3(x, c, 0.0);
		break;
	case 2:  pomTuple = float3(0.0, c, x);
		break;
	case 3:  pomTuple = float3(0.0, x, c);
		break;
	case 4:  pomTuple = float3(x, 0.0, c);
		break;
	case 5:  pomTuple = float3(c, 0.0, x);
		break;
	default: pomTuple = float3(0.0, 0.0, 0.0);
		break;
	}


	return float3((float)((pomTuple.x + m)), (float)((pomTuple.y + m)), (float)(pomTuple.z + m));


}

PixelShaderOutput PixelShaderFunction1(VertexShaderOutput input)
{

	float4 a = tex2D(textureSampler, input.TextureCoordinate);
	PixelShaderOutput output;

	float3 hsb = RgbToHsb(a.xyz);

	float3 cent1hsb = RgbToHsb(cent1);
	float3 cent2hsb = RgbToHsb(cent2);
	float3 cent3hsb = RgbToHsb(cent3);

	float jedna = Vzdalenost(hsb, cent1hsb);
	float dva = Vzdalenost(hsb, cent2hsb);
	float tri = Vzdalenost(hsb, cent3hsb);


	float smal = min(min(jedna, dva), tri);

	if (jedna == smal) {
		output.C1 = float4(a.x, a.y, a.z, 1);
	}
	else {
		//output.C1 = float4(1, 0, 1, 0);
		output.C1 = float4(a.x, a.y, a.z, 0);
	}

	if (dva == smal) {
		output.C2 = float4(a.x, a.y, a.z, 1);
	}
	else {
		//output.C2 = float4(1, 0, 1, 0);
		output.C2 = float4(a.x, a.y, a.z, 0);
	}

	if (tri == smal) {
		output.C3 = float4(a.x, a.y, a.z, 1);
	}
	else {
		//output.C3 = float4(1,0,1, 0);
		output.C3 = float4(a.x, a.y, a.z, 0);
	}

	return output;
	//return float4(a.x, 0, 0, a.w);

}

float4 PixelShaderFunction2(VertexShaderOutput input) : COLOR0
{

	float4 a = tex2D(textureSampler, input.TextureCoordinate);

	return float4(a.x, a.y, a.z, a.w);;
}

float4 PixelShaderFunction3(VertexShaderOutput input) : COLOR0
{
	float x = input.TextureCoordinate.x;
	float y = input.TextureCoordinate.y;

	float x1 = (1 / width/2);
	float y1 = (1 / height/2);

	float xp = (x + x1) > 1 ? 1 - x1 : x + x1;
	float yp = (y + y1) > 1 ? 1 - y1 : y + y1;;
	float xm = (x - x1) < 0 ? x1 : x - x1;;
	float ym = (y - y1) < 0 ? y1 : y - y1;;

	int count = 0;
	float4 b = float4(0,0,0,0);
	/*if (width<=2 && height <= 2)
	{
		if (tex2D(textureSampler, float2(0.25, 0.25)).w == 1) {
			b += tex2D(textureSampler, float2(0.25, 0.25));
			count++;
		}
		if (tex2D(textureSampler, float2(0.75, 0.25)).w == 1) {
			b += tex2D(textureSampler, float2(0.5, 0.5));
			count++;
		}
		if (tex2D(textureSampler, float2(0.75, 0.75)).w == 1) {
			b += tex2D(textureSampler, float2(0.75, 0.75));
			count++;
		}
		if (tex2D(textureSampler, float2(0.25, 0.75)).w == 1) {
			b += tex2D(textureSampler, float2(0.25, 0.75));
			count++;
		}
	}
	else
	{*/
		if (tex2D(textureSampler, float2(xp, yp)).w == 1) {
			b += tex2D(textureSampler, float2(xp, yp));
			count++;
		}
		if (tex2D(textureSampler, float2(xm, yp)).w == 1) {
			b += tex2D(textureSampler, float2(xm, yp));
			count++;
		}
		if (tex2D(textureSampler, float2(xp, ym)).w == 1) {
			b += tex2D(textureSampler, float2(xp, ym));
			count++;
		}
		if (tex2D(textureSampler, float2(xm, ym)).w == 1) {
			b += tex2D(textureSampler, float2(xm, ym));
			count++;
		}
		if (tex2D(textureSampler, float2(x, y)).w == 1) {
			b += tex2D(textureSampler, float2(x, y));
			count++;
		}
		if (tex2D(textureSampler, float2(xp, y)).w == 1) {
			b += tex2D(textureSampler, float2(xp, y));
			count++;
		}
		if (tex2D(textureSampler, float2(xm, y)).w == 1) {
			b += tex2D(textureSampler, float2(xm, y));
			count++;
		}
		if (tex2D(textureSampler, float2(x, ym)).w == 1) {
			b += tex2D(textureSampler, float2(x, ym));
			count++;
		}
		if (tex2D(textureSampler, float2(x, ym)).w == 1) {
			b += tex2D(textureSampler, float2(x, ym));
			count++;
		}
	//}


	if (count != 0) {
		return b / count;
	}
	else {
		return b;
	}
}

float4 PixelShaderFunction4(VertexShaderOutput input) : COLOR0
{

	float4 a = tex2D(textureSampler, input.TextureCoordinate);
	float4 b = tex2D(textureSamplerBackground, input.TextureCoordinate);
	float4 c = tex2D(textureSamplerMeansTexture, input.TextureCoordinate);

	if (c.w == 1) {
		return b;
	}
	else {
		return a;
	}

	return float4(a.x, a.y, a.z, a.w);;
}

technique Layers
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction1();
	}

	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction2();
	}

	pass Pass2
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction3();
	}

	pass Pass3
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction4();
	}
}

