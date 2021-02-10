using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Unity.Barracuda;

public class WebCamController : MonoBehaviour
 {
    int width = 640;
    int height = 480;
    int fps = 30;
    WebCamTexture webcamTexture;

    // MobileNetモデル関連
    public NNModel modelAsset;    
    private MobileNet mobileNet;
    
    Texture2D texture;
    Color32[] color32;

    // 推論結果描画用テキスト
    public Text text;
    private readonly FPSCounter fpsCounter = new FPSCounter();

    void Start() 
    {
        // Webカメラ準備
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        webcamTexture.Play();
        
        // MobileNetV2推論用クラス
        mobileNet = new MobileNet(modelAsset);
        StartCoroutine(WebCamTextureInitialize());
    }

    IEnumerator WebCamTextureInitialize()
    {
        while (true) {
            if (webcamTexture.width > 16 && webcamTexture.height > 16) {
                GetComponent<Renderer>().material.mainTexture = webcamTexture;
                color32 = new Color32[webcamTexture.width * webcamTexture.height];
                texture = new Texture2D(webcamTexture.width, webcamTexture.height);
                break;
            }
            yield return null;
        }
    }
    
    void Update()
    {
        fpsCounter.Update();

        // 入力用テクスチャ準備
        webcamTexture.GetPixels32(color32);
        texture.SetPixels32(color32);
        texture.Apply();
        
        // 推論
        var scores = mobileNet.Inference(texture);

        // 推論結果
        var maxScore = float.MinValue;
        int classId = -1;
        for (int i = 0; i < scores.Length; i++) {
            float score = scores[i];
            if (maxScore < score) {
                maxScore = score;
                classId = i;
            }
        }
        
        // 描画用テキスト構築
        string resultText = "";
        resultText = "FPS:" + fpsCounter.FPS.ToString("F2") + "\n" + "\n";        
        resultText = resultText + "Class ID:" + classId.ToString() + "\n";
        resultText = resultText + "Score:" + maxScore.ToString("F3") + "\n";
        if (classId >= 0) {
            resultText = resultText + "Name:" + mobileNet.getClassName(classId) + "\n";
        } else {
            resultText = resultText + "Name:????\n";
        }
#if UNITY_IOS || UNITY_ANDROID
        resultText = resultText + SystemInfo.graphicsDeviceType;
#endif

        // テキスト画面反映
        text.text = resultText;
    }
}