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

        // �t�H���_�p�X���쐬�i�i���f�[�^�p�X�̉��ɍ쐬�j
        string fullPath = Path.Combine(Application.persistentDataPath, _trashFolder);

        // �t�H���_�����݂��Ȃ��ꍇ�͍쐬
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        // ���S�ȃt�@�C���p�X���쐬
        path = Path.Combine(fullPath, _timeStamp + ".png");

        return path;
    }

    // UI�����������ꍇ��canvas���A�N�e�B�u�ɂ���
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
        // �����_�����O�����܂őҋ@
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _camera.targetTexture = renderTexture;

        Texture2D texture = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        texture.Apply();

        // �ۑ�����摜�̃T�C�Y��ς���Ȃ�ResizeTexture()�����s
        //texture = ResizeTexture(texture,320,90);

        byte[] pngData = texture.EncodeToPNG();
        _screenShotPath = GetScreenShotPath();

        // �t�@�C���Ƃ��ĕۑ�����Ȃ�File.WriteAllBytes()�����s
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
        Debug.Log(_screenShotPath + "��_screenShotPath");

        if (!String.IsNullOrEmpty(_screenShotPath))
        {
            byte[] image = File.ReadAllBytes(_screenShotPath);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            // NGUI �� UITexture �ɕ\��
            RawImage SSImage = _targetImage.GetComponent<RawImage>();

            SSImage.texture = tex;

            CamereResultGeneration(); //�B�e���ʂ�\�����鏈��
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
    /// �B�e���ʂ̉摜�̕\�����@�Ɋւ���֐�
    /// �X���𒼂��Ȃ���g�傳��Ă��铮��
    /// </summary>
    private void CamereResultGeneration()
    {
        _targetImage.SetActive(true);

        //�Q�[����ʂ̐^�񒆂ɔz�u
        _targetImage.transform.position = new Vector2(0, 0);
        _targetImage.transform.localScale = new Vector2(1, 1);
        _targetImage.transform.localEulerAngles = new Vector3(0f, 0f, 10f);

        Vector2 resultScale = new Vector2(3f, 3f);

        //2�b�Ԃ����āAresultScale�̑傫���ɂ���
        _targetImage.transform.DOScale(resultScale, 0.25f);

        //2�b�Ԃ����āA��]��0�ɖ߂��iUnity��Ő��z=10f�X���Ă���j
        _targetImage.transform.DORotate(Vector3.zero, 0.25f);
    }

    /// <summary>
    /// �A�v���P�[�V�����I�����ɌĂ΂��
    /// �X�N���[���V���b�g���폜����
    /// </summary>
    private void OnApplicationQuit()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _trashFolder);

        if (Directory.Exists(fullPath))
        {
            // �t�H���_���̑S�t�@�C�����폜
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            // �t�H���_���̂��폜����ꍇ
            // Directory.Delete(fullPath, true);
        }
    }
}
