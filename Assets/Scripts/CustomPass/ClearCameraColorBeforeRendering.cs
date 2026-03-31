using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ClearCameraColorBeforeRendering : CustomPass
{
    public Color clearColor = Color.black;

    protected override void Execute(CustomPassContext ctx)
    {
        CoreUtils.SetRenderTarget(
            ctx.cmd,
            ctx.cameraColorBuffer,
            ctx.cameraDepthBuffer,
            ClearFlag.None
        );

        CoreUtils.ClearRenderTarget(
            ctx.cmd,
            ClearFlag.Color,
            clearColor
        );
    }
}