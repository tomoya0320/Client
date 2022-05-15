using UnityEngine;

public class CameraHelper : SingletonMono<CameraHelper> {
  // 保存上一帧渲染的结果以供其他地方使用
  public RenderTexture LastFrameRT { get; private set; }

  private void OnRenderImage(RenderTexture src, RenderTexture dest) {
    LastFrameRT = src;
    Graphics.Blit(src, dest);
  }
}