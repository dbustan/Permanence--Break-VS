using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisibilityPass : ScriptableRenderPass
{
    private VisibilityPassSettings settings;
    private RTHandle targetHandle;
    private Shader visibilityShader;

    public VisibilityPass(VisibilityPassSettings settings) {
        this.settings = settings;
        renderPassEvent = settings.renderPassEvent;
        visibilityShader = settings.visibilityShader;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        cmd.GetTemporaryRT(Shader.PropertyToID(settings.targetTexture.name), settings.targetTexture.descriptor);
        targetHandle = RTHandles.Alloc(settings.targetTexture);
        ConfigureTarget(targetHandle);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("VisibilityPass");

        SortingSettings ss = new SortingSettings(renderingData.cameraData.camera) {
            criteria = SortingCriteria.BackToFront
        };
        DrawingSettings drawSettings = new DrawingSettings(new ShaderTagId("SRPDefaultUnlit"), ss)
        {
            overrideShader = visibilityShader
        };
        FilteringSettings filteringSettings = FilteringSettings.defaultValue;

        cmd.ClearRenderTarget(true, true, new Color(0, 0, 1, 1));
        context.ExecuteCommandBuffer(cmd);
        
        context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
        context.Submit();

        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(Shader.PropertyToID(targetHandle.name));
    }
}

[Serializable]
public struct VisibilityPassSettings {
    public RenderPassEvent renderPassEvent;
    public RenderTexture targetTexture;
    public Shader visibilityShader;
}
