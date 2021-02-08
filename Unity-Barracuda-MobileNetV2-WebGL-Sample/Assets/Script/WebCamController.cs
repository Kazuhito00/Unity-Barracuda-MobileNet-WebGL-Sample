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

    // MNISTモデル関連
    public NNModel modelAsset;    
    private MobileNetV2 mobileNetV2;
    
    Texture2D texture;
    Color32[] color32;

    // 推論結果描画用テキスト
    public Text text;

    void Start() 
    {
        // Webカメラ準備
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        webcamTexture.Play();
        
        // MobileNetV2推論用クラス
        mobileNetV2 = new MobileNetV2(modelAsset);
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
        // 入力用テクスチャ準備
        webcamTexture.GetPixels32(color32);
        texture.SetPixels32(color32);
        texture.Apply();
/*
        // 推論
        var scores = mobileNetV2.Inference(texture);

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
        // Debug.Log(classId + "(" + maxScore.ToString() + ")" + ":" + mobileNetV2.getClassName(classId));
        
        // 描画用テキスト構築
        string resultText = "";        
        resultText = "Class ID:" + classId.ToString() + "\n";
        resultText = resultText + "Score:" + maxScore.ToString("F3") + "\n";
        if (classId >= 0) {
            resultText = resultText + "Name:" + mobileNetV2.getClassName(classId) + "\n";
        } else {
            resultText = resultText + "Name:????\n";
        }

        // テキスト画面反映
        text.text = resultText;*/
    }
}