# ToDo
- [ ] FPS計測処理を追加する
- [ ] コメントがMNISTのまま
- [ ] ファイル名がV2のままのため、無印に統一する
- [ ] Androidビルドを試す
- [ ] 各FPSを計測する

# Unity-Barracuda-MobileNet-WebGL-Sample
Unity Barracudaを用いてMobileNet(画像クラス分類)をWebGL上で推論するサンプルです。<br>
![v9g8l-5fqfl](https://user-images.githubusercontent.com/37477845/107118246-1549c680-68c3-11eb-8bb3-961881a20158.gif)

# Demo
動作確認用ページは以下。<br>
[https://kazuhito00.github.io/Unity-Barracuda-MobileNetV1-WebGL-Sample/WebGL-Build](https://kazuhito00.github.io/Unity-Barracuda-MobileNetV1-WebGL-Sample/WebGL-Build)

# FPS(参考値)
* WebGL(CPU：Intel Core i7-8750H CPU @2.20GHz)
* Android(Google Pixel4a(Snapdragon 730G))
* Unity Editor(GPU：NVIDIA GeForce GTX 1050 Ti Max-Q(4GB GDDR5))

# Requirement (Unity)
* Unity 2020.1.6f1 or later
* Barracuda 1.3.0 or later

# Requrement (Python) ※ONNX変換をする場合のみ
* Tensorflow 2.4.0 or later
* tf2onnx 1.8.2 or later
* onnxruntime 1.6.0 or later(※ONNX変換後の推論をテストする場合のみ)

# Reference
* [Barracuda 1.3.0 preview](https://docs.unity3d.com/Packages/com.unity.barracuda@1.3/manual/index.html)
* [【Unity】WebGLで日本語テキストが表示されない問題について](https://chiritsumo-blog.com/unity-webgl-japanese/)
* [Texutre2Dのサイズ変更【Unity】](https://kan-kikuchi.hatenablog.com/entry/TextureScale)
* [【Unity】FPS を計測するスクリプト](https://baba-s.hatenablog.com/entry/2019/05/04/220500)

# Author
高橋かずひと(https://twitter.com/KzhtTkhs)
 
# License 
Unity-Barracuda-MobileNet-WebGL-Sample is under [Apache-2.0 License](LICENSE).
