#ifndef DETECTEDGES_INCLUDED
#define DETECTEDGES_INCLUDED

static float sobelHMat[9] = {
    1,  2,  1,
    0,  0,  0,
    -1, -2, -1
};
static float sobelVMat[9] = {
    -1, 0, 1,
    -2, 0, 2,
    -1, 0, 1
};

void DepthEdges_float(float2 UV, float Thickness, float Threshold, float Power, float Strength, out float Out) {

    float2 sobel = 0;
    float aspectRatio = (float)_ScreenParams.x/_ScreenParams.y;

    for(int y = 0; y < 3; y++) {
        for(int x = 0; x < 3; x++) {
            float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2((x-1)/aspectRatio, y-1) * Thickness);
            int sobelIndex = y * 3 + x;
            sobel += depth * float2(sobelHMat[sobelIndex], sobelVMat[sobelIndex]);
        }
    }

    float outlineVal = pow(length(sobel)/5.0, Power);
    if(outlineVal >= Threshold) {
        Out = (outlineVal-Threshold)/(1-Threshold) * Strength;
    } else {
        Out = 0.0;
    }
}
void ColorEdges_float(float2 UV, float Thickness, float Threshold, float Power, float Strength, out float Out) {
    float2 sobelR = 0;
    float2 sobelG = 0;
    float2 sobelB = 0;
    float aspectRatio = (float)_ScreenParams.x/_ScreenParams.y;

    for(int y = 0; y < 3; y++) {
        for(int x = 0; x < 3; x++) {
            float3 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, UV + float2((x-1)/aspectRatio, y-1) * Thickness);
            int sobelIndex = y * 3 + x;
            float2 kernel = float2(sobelHMat[sobelIndex], sobelVMat[sobelIndex]);
            sobelR += color.x * kernel;
            sobelG += color.y * kernel;
            sobelB += color.z * kernel;
        }
    }

    float outlineVal = pow(max(length(sobelR), max(length(sobelG), length(sobelB)))/5.0, Power);

    if(outlineVal >= Threshold) {
        Out = (outlineVal-Threshold)/(1-Threshold) * Strength;
    } else {
        Out = 0.0;
    }
}
void NormalEdges_float(float2 UV, float Thickness, float Threshold, float Power, float Strength, out float Out) {
    float2 sobelR = 0;
    float2 sobelG = 0;
    float2 sobelB = 0;
    float aspectRatio = (float)_ScreenParams.x/_ScreenParams.y;

    for(int y = 0; y < 3; y++) {
        for(int x = 0; x < 3; x++) {
            float3 color = SHADERGRAPH_SAMPLE_SCENE_NORMAL(UV + float2((x-1)/aspectRatio, y-1) * Thickness);
            int sobelIndex = y * 3 + x;
            float2 kernel = float2(sobelHMat[sobelIndex], sobelVMat[sobelIndex]);
            sobelR += color.x * kernel;
            sobelG += color.y * kernel;
            sobelB += color.z * kernel;
        }
    }

    float outlineVal = pow(max(length(sobelR), max(length(sobelG), length(sobelB)))/5.0, Power);

    if(outlineVal >= Threshold) {
        Out = (outlineVal-Threshold)/(1-Threshold) * Strength;
    } else {
        Out = 0.0;
    }
}

#endif