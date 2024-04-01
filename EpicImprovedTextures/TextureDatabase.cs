using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace EpicImprovedTextures;

public class TextureDatabase : MonoBehaviour
{
    public readonly List<RenderTexture> Textures = new List<RenderTexture>();

    private static TextureDatabase _instance;
    
    public static TextureDatabase GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("TextureDatabase").AddComponent<TextureDatabase>();
        }

        return _instance;
    }
    
    private void Awake()
    {
        LoadAllTextures();
    }

    private void LoadAllTextures()
    {
        var clips = Plugin.AssetBundle.LoadAllAssets<VideoClip>();
        foreach (var clip in clips)
        {
            CreateMovie(clip);
        }
    }

    private void CreateMovie(VideoClip clip)
    {
        var videoPlayer = new GameObject($"VideoPlayer-{clip}").AddComponent<VideoPlayer>();
        videoPlayer.clip = clip;
        videoPlayer.isLooping = true;
        var renderTexture = new RenderTexture((int) clip.width, (int) clip.height, 0);
        videoPlayer.targetTexture = renderTexture;
        Textures.Add(renderTexture);
    }
}