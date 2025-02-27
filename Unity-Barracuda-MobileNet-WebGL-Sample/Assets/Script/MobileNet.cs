﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Barracuda;

public class MobileNet
{   
    readonly IWorker worker;

    private int inputShapeX = 224;
    private int inputShapeY = 224;

    public MobileNet(NNModel modelAsset)
    {
        var model = ModelLoader.Load(modelAsset);

#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("Worker:CPU");
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, model); // CPU
#else
        Debug.Log("Worker:GPU");
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model); // GPU
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    public float[] Inference(Texture2D texture)
    {
        // テクスチャコピー
        Texture2D inputTexture = new Texture2D(texture.width, texture.height);
        var tempColor32 = texture.GetPixels32();
        inputTexture.SetPixels32(tempColor32);
        inputTexture.Apply();
        Graphics.CopyTexture(texture, inputTexture);

        // テクスチャリサイズ、およびColor32データ取得
        TextureScale.Bilinear(inputTexture, inputShapeX, inputShapeY);
        var color32 = inputTexture.GetPixels32();
        MonoBehaviour.Destroy(inputTexture);
        
        float[] floatValues = new float[inputShapeX * inputShapeY * 3];
        for (int i = 0; i < color32.Length; ++i) {
            var color = color32[i];
            floatValues[i * 3 + 0] = (color.r - 0) / 255.0f;
            floatValues[i * 3 + 1] = (color.g - 0) / 255.0f;
            floatValues[i * 3 + 2] = (color.b - 0) / 255.0f;
        }

        var inputTensor = new Tensor(1, inputShapeY, inputShapeX, 3, floatValues);

        worker.Execute(inputTensor);
        var outputTensor = worker.PeekOutput();
        var outputArray = outputTensor.ToReadOnlyArray();
        
        inputTensor.Dispose();
        outputTensor.Dispose();

        return outputArray;
    }
#else
    public float[] Inference(Texture2D texture)
    {
        var inputTensor = new Tensor(texture, 3);

        worker.Execute(inputTensor);
        var outputTensor = worker.PeekOutput();
        var outputArray = outputTensor.ToReadOnlyArray();
        
        inputTensor.Dispose();
        outputTensor.Dispose();

        return outputArray;
    }
#endif

    ~MobileNet()
    {
        worker?.Dispose();
    }

    public string getClassName(int classId)
    {
        if (classId < 0 || className.Length <= classId){
            return "";
        }
        return className[classId];
    }

    string[] className = {
        "テンチ",
        "金魚",
        "ホホジロザメ",
        "イタチザメ",
        "ハンマーヘッド",
        "シビレエイ",
        "アカエイ",
        "コック",
        "めんどり",
        "ダチョウ",
        "アトリ",
        "ゴシキヒワ",
        "ハウスフィンチ",
        "ユキヒメドリ",
        "インディゴホオジロ",
        "ロビン",
        "ブルブル",
        "カケス",
        "カササギ",
        "四十雀",
        "水クロウタドリ",
        "凧",
        "白頭ワシ",
        "ハゲワシ",
        "カラフトフクロウ",
        "欧州ファイアサラマンダー",
        "共通イモリ",
        "イモリ",
        "サンショウウオを発見",
        "アホロートル",
        "ウシガエル",
        "アマガエル",
        "つかれたカエル",
        "とんちき",
        "オサガメ",
        "鼈",
        "テラピン",
        "ハコガメ",
        "縞模様のヤモリ",
        "共通イグアナ",
        "アメリカンカメレオン",
        "ウィッペイル",
        "アガマトカゲ",
        "フリルトカゲ",
        "アリゲータートカゲ",
        "アメリカドクトカゲ",
        "緑のトカゲ",
        "アフリカのカメレオン",
        "コモドドラゴン",
        "アフリカのワニ",
        "アメリカワニ",
        "トリケラトプス",
        "雷のヘビ",
        "リングネックスネーク",
        "ホーノースヘビ",
        "緑のヘビ",
        "キングスネーク",
        "ガータースネーク",
        "水蛇",
        "つるヘビ",
        "夜のヘビ",
        "ボア・コンストリクター",
        "ロックパイソン",
        "インドコブラ",
        "グリーンマンバ",
        "ウミヘビ",
        "ツノクサリヘビ",
        "ダイヤ",
        "サイドワインダー",
        "三葉虫",
        "刈り入れ作業者",
        "サソリ",
        "黒と金の庭クモ",
        "納屋クモ",
        "庭クモ",
        "クロゴケグモ",
        "タランチュラ",
        "オオカミのクモ",
        "ダニ",
        "百足",
        "クロライチョウ",
        "雷鳥",
        "ひだえりの付いたライチョウ",
        "草原チキン",
        "孔雀",
        "ウズラ",
        "ヤマウズラ",
        "アフリカの灰色",
        "コンゴウインコ",
        "硫黄トキオウム",
        "インコ",
        "バンケン",
        "蜂食べる人",
        "サイチョウ",
        "ハチドリ",
        "錐嘴",
        "オオハシ",
        "ドレイク",
        "赤ブレストアイサ属のガモ",
        "ガチョウ",
        "黒い白鳥",
        "タスカービール",
        "ハリモグラ",
        "カモノハシ",
        "ワラビー",
        "コアラ",
        "ウォンバット",
        "クラゲ",
        "イソギンチャク",
        "脳サンゴ",
        "扁形動物",
        "線虫",
        "巻き貝",
        "カタツムリ",
        "ナメクジ",
        "ウミウシ",
        "キトン",
        "オウムガイ",
        "アメリカイチョウガニ",
        "岩カニ",
        "シオマネキ",
        "タラバガニ",
        "アメリカンロブスター",
        "伊勢エビ",
        "ザリガニ",
        "ヤドカリ",
        "等脚類",
        "コウノトリ",
        "ナベコウ",
        "ヘラサギ",
        "フラミンゴ",
        "小さな青いサギ",
        "アメリカン白鷺",
        "にがり",
        "クレーン",
        "ツルモドキ科の鳥",
        "ヨーロピアン水鳥",
        "アメリカオオバン",
        "ノガン",
        "キョウジョシギ",
        "赤担保シギ",
        "アカアシシギ",
        "オオハシシギ",
        "ミヤコドリ",
        "ペリカン",
        "キングペンギン",
        "アルバトロス",
        "コククジラ",
        "シャチ",
        "ジュゴン",
        "アシカ",
        "チワワ",
        "狆",
        "マルチーズ犬",
        "狆",
        "シーズー、シーズー",
        "ブレナムスパニエル",
        "パピヨン",
        "トイテリア",
        "ローデシアン・リッジバック",
        "アフガンハウンド",
        "バセット犬",
        "ビーグル",
        "ブラッドハウンド",
        "ブルーティック",
        "黒と黄褐色の猟犬",
        "ウォーカーハウンド",
        "イングリッシュフォックスハウンド",
        "レッドボーン",
        "ボルゾイ",
        "アイリッシュ・ウルフハウンド",
        "イタリアングレーハウンド",
        "ウィペット",
        "イビサハウンド",
        "ノルウェーエルクハウンド",
        "オッターハウンド",
        "サルーキ",
        "スコティッシュ・ディアハウンド",
        "ワイマラナー",
        "スタフォードシャーブルテリア",
        "アメリカン・スタッフォードシャー・テリア",
        "ベドリントンテリア",
        "ボーダーテリア",
        "ケリーブルーテリア",
        "アイリッシュテリア",
        "ノーフォークテリア",
        "ノーリッチ・テリア",
        "ヨークシャーテリア",
        "ワイヤーヘアー・フォックステリア",
        "レークランドテリア",
        "シーリーハムテリア",
        "エアデール",
        "ケルン",
        "オーストラリアテリア",
        "ダンディディンモントテリア",
        "ボストンブル",
        "ミニチュアシュナウザー",
        "ジャイアントシュナウザー",
        "スタンダードシュナウザー",
        "スコッチテリア",
        "チベタンテリア",
        "シルキーテリア",
        "ソフトコーテッド・ウィートン・テリア",
        "ウェストハイランドホワイトテリア",
        "ラサ",
        "フラットコーテッド・レトリーバー",
        "カーリーコーティングされたレトリーバー",
        "ゴールデンレトリバー",
        "ラブラドル・レトリーバー犬",
        "チェサピーク湾レトリーバー",
        "ジャーマン・ショートヘア・ポインタ",
        "ビズラ",
        "イングリッシュセッター",
        "アイリッシュセッター",
        "ゴードンセッター",
        "ブリタニースパニエル",
        "クランバー",
        "イングリッシュスプリンガー",
        "ウェルシュスプリンガースパニエル",
        "コッカースパニエル",
        "サセックススパニエル",
        "アイルランドのウォータースパニエル",
        "クバース犬",
        "スキッパーキー",
        "ベルジアン・シェパード・ドッグ・グローネンダール",
        "マリノア",
        "ブリアール",
        "ケルピー",
        "コモンドール",
        "オールドイングリッシュシープドッグ",
        "シェトランドシープドッグ",
        "コリー",
        "ボーダーコリー",
        "ブーヴィエ・デ・フランドル",
        "ロットワイラー",
        "ジャーマンシェパード",
        "ドーベルマン犬",
        "ミニチュアピンシャー",
        "グレータースイスマウンテンドッグ",
        "バーネーズマウンテンドッグ",
        "アッペンツェル",
        "エントレブッシャー",
        "ボクサー",
        "ブルマスチフ",
        "チベットマスチフ",
        "フレンチブルドッグ",
        "グレートデーン",
        "セントバーナード",
        "エスキモー犬",
        "マラミュート",
        "シベリアンハスキー",
        "ダルメシアン",
        "アーフェンピンシャー",
        "バセンジー",
        "パグ",
        "レオンバーグ",
        "ニューファンドランド島",
        "グレートピレニーズ",
        "サモエド",
        "ポメラニアン",
        "チャウ",
        "キースホンド",
        "ブラバンソングリフォン",
        "ペンブローク",
        "カーディガン",
        "トイプードル",
        "ミニチュアプードル",
        "スタンダードプードル",
        "メキシカン・ヘアーレス",
        "シンリンオオカミ",
        "白いオオカミ",
        "レッドウルフ",
        "コヨーテ",
        "ディンゴ",
        "ドール",
        "リカオン",
        "ハイエナ",
        "アカギツネ",
        "キットキツネ",
        "ホッキョクギツネ",
        "灰色のキツネ",
        "タビー",
        "虎猫",
        "ペルシャ猫",
        "シャム猫",
        "エジプトの猫",
        "クーガー",
        "オオヤマネコ",
        "ヒョウ",
        "ユキヒョウ",
        "ジャガー",
        "ライオン",
        "虎",
        "チーター",
        "ヒグマ",
        "アメリカクロクマ",
        "氷のクマ",
        "ナマケグマ",
        "マングース",
        "ミーアキャット",
        "ハンミョウ",
        "てんとう虫",
        "グランドビートル",
        "カミキリムシ",
        "ハムシ",
        "フンコロガシ",
        "サイハムシ",
        "ゾウムシ",
        "ハエ",
        "蜂",
        "蟻",
        "バッタ",
        "クリケット",
        "杖",
        "ゴキブリ",
        "カマキリ",
        "蝉",
        "ヨコバイ",
        "クサカゲロウ",
        "トンボ",
        "イトトンボ",
        "提督",
        "リングレット",
        "君主",
        "モンシロチョウ",
        "硫黄蝶",
        "シジミチョウ",
        "ヒトデ",
        "うに",
        "ナマコ",
        "木のウサギ",
        "野ウサギ",
        "アンゴラ",
        "ハムスター",
        "ヤマアラシ",
        "キツネリス",
        "マーモット",
        "ビーバー",
        "モルモット",
        "栗色",
        "シマウマ",
        "豚",
        "イノシシ",
        "イボイノシシ",
        "カバ",
        "雄牛",
        "水牛",
        "バイソン",
        "ラム",
        "ビッグホーン",
        "アイベックス",
        "ハーテビースト",
        "インパラ",
        "ガゼル",
        "アラビアラクダ",
        "ラマ",
        "イタチ",
        "ミンク",
        "ケナガイタチ",
        "クロアシイタチ",
        "カワウソ",
        "スカンク",
        "狸",
        "アルマジロ",
        "ミユビナマケモノ",
        "オランウータン",
        "ゴリラ",
        "チンパンジー",
        "テナガザル",
        "フクロテナガザル",
        "オナガザル",
        "パタス",
        "ヒヒ",
        "マカク",
        "ヤセザル",
        "コロブス属",
        "テングザル",
        "マーモセット",
        "オマキザル",
        "ホエザル",
        "ティティ",
        "クモザル",
        "リスザル",
        "マダガスカル猫",
        "インドリ",
        "インドゾウ",
        "アフリカゾウ",
        "レッサーパンダ",
        "ジャイアントパンダ",
        "バラクータ",
        "ウナギ",
        "ギンザケ",
        "岩の美しさ",
        "クマノミ",
        "チョウザメ",
        "ガー",
        "ミノカサゴ",
        "フグ",
        "そろばん",
        "アバヤ",
        "アカデミックガウン",
        "アコーディオン",
        "アコースティックギター",
        "空母",
        "旅客機",
        "飛行船",
        "祭壇",
        "救急車",
        "両生類",
        "アナログ時計",
        "養蜂場",
        "エプロン",
        "ごみ入れ",
        "アサルトライフル",
        "バックパック",
        "ベーカリー",
        "平均台",
        "バルーン",
        "ボールペン",
        "バンドエイド",
        "バンジョー",
        "バニスター",
        "バーベル",
        "理髪店の椅子",
        "理髪店",
        "納屋",
        "バロメーター",
        "バレル",
        "バロー",
        "野球",
        "バスケットボール",
        "バシネット",
        "ファゴット",
        "水泳帽",
        "バスタオル",
        "バスタブ",
        "ビーチワゴン",
        "ビーコン",
        "ビーカー",
        "ベアスキン",
        "ビール瓶",
        "ビールグラス",
        "ベルコート",
        "ビブ",
        "自転車",
        "ビキニ",
        "バインダー",
        "双眼鏡",
        "巣箱",
        "ボートハウス",
        "ボブスレー",
        "ループタイ",
        "ボンネット",
        "本棚",
        "書店",
        "瓶のキャップ",
        "弓",
        "ちょうネクタイ",
        "真鍮",
        "ブラジャー",
        "防波堤",
        "胸当て",
        "ほうき",
        "バケツ",
        "バックル",
        "防弾チョッキ",
        "新幹線",
        "精肉店",
        "タクシー",
        "大釜",
        "キャンドル",
        "大砲",
        "カヌー",
        "缶切り",
        "カーディガン",
        "車のミラー",
        "回転木馬",
        "大工のキット",
        "カートン",
        "車のホイール",
        "現金自動預け払い機",
        "カセット",
        "カセット・プレーヤー",
        "城",
        "カタマラン",
        "CDプレーヤー",
        "チェロ",
        "スマートフォン",
        "鎖",
        "チェーンリンクフェンス",
        "チェーンメール",
        "チェーンソー",
        "胸",
        "シフォニア",
        "チャイム",
        "中国キャビネット",
        "クリスマスの靴下",
        "教会",
        "映画",
        "クリーバー",
        "崖の住居",
        "マント",
        "クロッグ",
        "カクテルシェーカー",
        "コーヒーマグ",
        "コーヒーポット",
        "コイル",
        "ダイヤル錠",
        "コンピュータのキーボード",
        "製菓",
        "コンテナ船",
        "コンバーチブル",
        "コークスクリュー",
        "コルネット",
        "カウボーイブーツ",
        "カウボーイハット",
        "クレードル",
        "クレーン",
        "クラッシュヘルメット",
        "木箱",
        "ベビーベッド",
        "クロークポット",
        "クロケットボール",
        "松葉杖",
        "胸当て",
        "ダム",
        "机",
        "デスクトップコンピューター",
        "ダイヤル電話",
        "おむつ",
        "デジタル時計",
        "デジタル腕時計",
        "ダイニングテーブル",
        "意気地なし",
        "食器洗い機",
        "ディスクブレーキ",
        "ドック",
        "犬ぞり",
        "ドーム",
        "玄関マット",
        "掘削基地",
        "ドラム",
        "ドラムスティック",
        "ダンベル",
        "ダッチオーブン",
        "扇風機",
        "エレキギター",
        "電気機関車",
        "娯楽施設",
        "封筒",
        "エスプレッソマシーン",
        "フェースパウダー",
        "フェザーボア",
        "ファイル",
        "消防艇",
        "消防車",
        "ファイアースクリーン",
        "旗竿",
        "フルート",
        "折り畳み式椅子",
        "フットボールヘルメット",
        "フォークリフト",
        "噴水",
        "万年筆",
        "四柱",
        "貨車",
        "フレンチホルン",
        "フライパン",
        "毛皮のコート",
        "ごみ収集車",
        "ガスマスク",
        "ガソリンポンプ",
        "ゴブレット",
        "ゴーカート",
        "ゴルフボール",
        "ゴルフカート",
        "ゴンドラ",
        "ゴング",
        "ガウン",
        "グランドピアノ",
        "温室",
        "グリル",
        "食料品店",
        "ギロチン",
        "ヘアスライド",
        "ヘアスプレー",
        "半トラック",
        "ハンマー",
        "妨げます",
        "ハンドブロワー",
        "タブレット",
        "ハンカチ",
        "ハードディスク",
        "ハーモニカ",
        "ハープ",
        "ハーベスタ",
        "斧",
        "ホルスター",
        "ホームシアター",
        "ハニカム",
        "フック",
        "フープスカート",
        "水平バー",
        "馬車",
        "砂時計",
        "アイフォーン",
        "鉄",
        "ジャックオーランタン",
        "ジーンズ",
        "ジープ",
        "ジャージー",
        "ジグソーパズル",
        "人力車",
        "ジョイスティック",
        "着物",
        "膝パッド",
        "結び目",
        "白衣",
        "ひしゃく",
        "ランプのかさ",
        "ノートパソコン",
        "芝刈り機",
        "レンズキャップ",
        "レターオープナー",
        "ライブラリ",
        "救命ボート",
        "ライター",
        "リムジン",
        "ライナー",
        "口紅",
        "ローファー",
        "ローション",
        "スピーカー",
        "ルーペ",
        "製材所",
        "磁気コンパス",
        "郵袋",
        "メールボックス",
        "マイヨ",
        "マイヨ",
        "マンホールの蓋",
        "マラカス",
        "マリンバ",
        "マスク",
        "マッチ棒",
        "メイポール",
        "迷路",
        "計量カップ",
        "薬箱",
        "巨石",
        "マイク",
        "マイクロ波",
        "軍服",
        "ミルク缶",
        "ミニバス",
        "ミニスカート",
        "ミニバン",
        "ミサイル",
        "ミトン",
        "ミキシングボウル",
        "移動住宅",
        "モデルT",
        "モデム",
        "修道院",
        "モニター",
        "モペット",
        "モルタル",
        "モルタルボード",
        "モスク",
        "蚊帳",
        "スクーター",
        "マウンテンバイク",
        "山のテント",
        "マウス",
        "ネズミ捕り",
        "引っ越しトラック",
        "銃口",
        "ネイル",
        "ネックブレース",
        "ネックレス",
        "乳首",
        "ノート",
        "オベリスク",
        "オーボエ",
        "オカリナ",
        "オドメーター",
        "オイルフィルター",
        "器官",
        "オシロスコープ",
        "オーバースカート",
        "牛車",
        "酸素マスク",
        "パケット",
        "パドル",
        "パドルホイール",
        "南京錠",
        "絵筆",
        "パジャマ",
        "宮殿",
        "パンパイプ",
        "ペーパータオル",
        "パラシュート",
        "平行棒",
        "公園のベンチ",
        "パーキングメーター",
        "乗用車",
        "パティオ",
        "有料電話",
        "台座",
        "筆箱",
        "鉛筆削り",
        "香水",
        "ペトリ皿",
        "コピー機",
        "選ぶ",
        "スパイク付き鉄かぶと",
        "杭柵",
        "拾う",
        "桟橋",
        "貯金箱",
        "錠剤瓶",
        "枕",
        "ピンポン球",
        "風車",
        "海賊",
        "ピッチャー",
        "飛行機",
        "プラネタリウム",
        "ビニール袋",
        "皿立て",
        "プラウ",
        "プランジャー",
        "ポラロイドカメラ",
        "ポール",
        "警察車",
        "ポンチョ",
        "ビリヤード台",
        "ポップ・ボトル",
        "ポット",
        "ろくろ",
        "パワードリル",
        "礼拝用敷物",
        "プリンタ",
        "刑務所",
        "発射体",
        "プロジェクター",
        "パック",
        "サンドバッグ",
        "財布",
        "クイル",
        "キルト",
        "レーサー",
        "ラケット",
        "ラジエーター",
        "無線",
        "電波望遠鏡",
        "天水桶",
        "RV車",
        "リール",
        "レフレックスカメラ",
        "冷蔵庫",
        "リモコン",
        "レストラン",
        "リボルバー",
        "ライフル",
        "ロッキングチェア",
        "焼肉料理店",
        "消しゴム",
        "ラグビーボール",
        "ルール",
        "ランニングシューズ",
        "安全",
        "安全ピン",
        "塩の入れ物",
        "サンダル",
        "サロン",
        "サックス",
        "鞘",
        "規模",
        "スクールバス",
        "スクーナー",
        "スコアボード",
        "画面",
        "スクリュー",
        "ドライバー",
        "シートベルト",
        "ミシン",
        "シールド",
        "靴屋",
        "障子",
        "買い物かご",
        "ショッピングカート",
        "シャベル",
        "シャワーキャップ",
        "シャワーカーテン",
        "スキー",
        "スキーマスク",
        "寝袋",
        "計算尺",
        "引き戸",
        "スロット",
        "スノーケル",
        "スノーモービル",
        "除雪機",
        "ソープディスペンサー",
        "サッカーボール",
        "靴下",
        "太陽の皿",
        "ソンブレロ",
        "スープ皿",
        "スペースキー",
        "スペースヒーター",
        "スペースシャトル",
        "へら",
        "スピードボート",
        "クモの巣",
        "スピンドル",
        "スポーツカー",
        "スポットライト",
        "ステージ",
        "蒸気機関車",
        "鋼アーチ橋",
        "スチールドラム",
        "聴診器",
        "ストール",
        "石垣",
        "ストップウォッチ",
        "レンジ",
        "ストレーナー",
        "路面電車",
        "ストレッチャー",
        "スタジオソファ",
        "仏舎利塔",
        "潜水艦",
        "スーツ",
        "日時計",
        "サングラス",
        "サングラス",
        "日焼け止め剤",
        "つり橋",
        "綿棒",
        "トレーナー",
        "海パン",
        "スイング",
        "スイッチ",
        "注射器",
        "電気スタンド",
        "タンク",
        "テーププレーヤー",
        "ティーポット",
        "テディ",
        "テレビ",
        "テニスボール",
        "サッチ",
        "劇場のカーテン",
        "指ぬき",
        "脱穀機",
        "王位",
        "瓦屋根",
        "トースター",
        "タバコ屋",
        "便座",
        "トーチ",
        "トーテムポール",
        "レッカー車",
        "玩具屋",
        "トラクター",
        "トレーラートラック",
        "トレイ",
        "トレンチコート",
        "三輪車",
        "三胴船",
        "三脚",
        "凱旋門",
        "トロリーバス",
        "トロンボーン",
        "バスタブ",
        "回転ドア",
        "タイプライターのキーボード",
        "傘",
        "一輪車",
        "直立",
        "真空",
        "花瓶",
        "ボールト",
        "ベルベット",
        "自動販売機",
        "祭服",
        "高架橋",
        "バイオリン",
        "バレーボール",
        "ワッフル焼き型",
        "壁時計",
        "財布",
        "ワードローブ",
        "戦闘機",
        "洗面器",
        "ワッシャー",
        "水筒",
        "水差し",
        "給水塔",
        "ウイスキージャグ",
        "ホイッスル",
        "かつら",
        "窓網戸",
        "ブラインド",
        "ウィンザーネクタイ",
        "ワインボトル",
        "翼",
        "中華鍋",
        "木製スプーン",
        "ウール",
        "ワームフェンス",
        "難破船",
        "ヨール",
        "パオ",
        "サイト",
        "コミックブック",
        "クロスワードパズル",
        "道路標識",
        "交通信号灯",
        "ブックカバー",
        "メニュー",
        "プレート",
        "グアカモーレ",
        "コンソメ",
        "ホットポット",
        "パフェ",
        "アイスクリーム",
        "アイスキャンディー",
        "フランスパン",
        "ベーグル",
        "プレッツェル",
        "チーズバーガー",
        "ホットドッグ",
        "マッシュポテト",
        "キャベツ",
        "ブロッコリー",
        "カリフラワー",
        "ズッキーニ",
        "そうめんかぼちゃ",
        "ドングリかぼちゃ",
        "カボチャ",
        "キュウリ",
        "アーティチョーク",
        "ピーマン",
        "カルドン",
        "キノコ",
        "リンゴ",
        "イチゴ",
        "オレンジ",
        "レモン",
        "イチジク",
        "パイナップル",
        "バナナ",
        "パラミツ",
        "カスタードアップル",
        "ザクロ",
        "干し草",
        "カルボナーラ",
        "チョコレートソース",
        "パン生地",
        "ミートローフ",
        "ピザ",
        "ポットパイ",
        "ブリトー",
        "赤ワイン",
        "エスプレッソ",
        "カップ",
        "エッグノッグ",
        "アルプス",
        "バブル",
        "崖",
        "サンゴ礁",
        "間欠泉",
        "湖畔",
        "岬",
        "砂州",
        "海岸",
        "谷",
        "火山",
        "野球選手",
        "新郎",
        "スキューバダイバー",
        "菜種",
        "デイジー",
        "蘭",
        "トウモロコシ",
        "ドングリ",
        "ヒップ",
        "トチノキ",
        "サンゴ菌",
        "ハラタケ",
        "シャグマアミガサタケ",
        "スッポンタケ",
        "ハラタケ",
        "舞茸",
        "きのこ",
        "耳",
        "トイレットペーパー"
    };

}