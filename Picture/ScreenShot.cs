using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _doNotShowCanvas;
    [SerializeField] private GameObject _targetImage;

    private string _screenShotPath;
    private string _timeStamp;
    private string _trashFolder = "TrashScreenShot";

    private ShutterManager _shutterManager;

    private string GetScreenShotPath()
    {
        string path = "";

        // フォルダパスを作成（永続データパスの下に作成）
        string fullPath = Path.Combine(Application.persistentDataPath, _trashFolder);

        // フォルダが存在しない場合は作成
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        // 完全なファイルパスを作成
        path = Path.Combine(fullPath, _timeStamp + ".png");

        return path;
    }

    // UIを消したい場合はcanvasを非アクティブにする
    private void UIStateChange()
    {
        _doNotShowCanvas.SetActive(!_doNotShowCanvas.activeSelf);
    }

    private IEnumerator CreateScreenShot()
    {
        _targetImage.SetActive(false);

        UIStateChange();
        DateTime date = DateTime.Now;
        _timeStamp = date.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        // レンダリング完了まで待機
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _camera.targetTexture = renderTexture;

        Texture2D texture = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        texture.Apply();

        // 保存する画像のサイズを変えるならResizeTexture()を実行
        //texture = ResizeTexture(texture,320,90);

        byte[] pngData = texture.EncodeToPNG();
        _screenShotPath = GetScreenShotPath();

        // ファイルとして保存するならFile.WriteAllBytes()を実行
        File.WriteAllBytes(_screenShotPath, pngData);

        _camera.targetTexture = null;

        UIStateChange();
    }

    Texture2D ResizeTexture(Texture2D src, int dst_w, int dst_h)
    {
        Texture2D dst = new Texture2D(dst_w, dst_h, src.format, false);

        float inv_w = 1f / dst_w;
        float inv_h = 1f / dst_h;

        for (int y = 0; y < dst_h; ++y)
        {
            for (int x = 0; x < dst_w; ++x)
            {
                dst.SetPixel(x, y, src.GetPixelBilinear((float)x * inv_w, (float)y * inv_h));
            }
        }
        return dst;
    }

    public void SaveScreenshot()
    {
        StartCoroutine(CreateScreenShot());
    }

    public void ShowSSImage()
    {
        Debug.Log(_screenShotPath + "が_screenShotPath");

        if (!String.IsNullOrEmpty(_screenShotPath))
        {
            byte[] image = File.ReadAllBytes(_screenShotPath);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            // NGUI の UITexture に表示
            RawImage SSImage = _targetImage.GetComponent<RawImage>();

            SSImage.texture = tex;

            CamereResultGeneration(); //撮影結果を表示する処理
        }
    }

    void Start()
    {
        _shutterManager = transform.GetComponent<ShutterManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 撮影結果の画像の表示方法に関する関数
    /// 傾きを直しながら拡大されてくる動き
    /// </summary>
    private void CamereResultGeneration()
    {
        _targetImage.SetActive(true);

        //ゲーム画面の真ん中に配置
        _targetImage.transform.position = new Vector2(0, 0);
        _targetImage.transform.localScale = new Vector2(1, 1);
        _targetImage.transform.localEulerAngles = new Vector3(0f, 0f, 10f);

        Vector2 resultScale = new Vector2(3f, 3f);

        //2秒間かけて、resultScaleの大きさにする
        _targetImage.transform.DOScale(resultScale, 0.25f);

        //2秒間かけて、回転を0に戻す（Unity上で先にz=10f傾けてある）
        _targetImage.transform.DORotate(Vector3.zero, 0.25f);
    }

    /// <summary>
    /// アプリケーション終了時に呼ばれる
    /// スクリーンショットを削除する
    /// </summary>
    private void OnApplicationQuit()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _trashFolder);

        if (Directory.Exists(fullPath))
        {
            // フォルダ内の全ファイルを削除
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            // フォルダ自体を削除する場合
            // Directory.Delete(fullPath, true);
        }
    }
}
