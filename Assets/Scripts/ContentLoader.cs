using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ContentLoader
{
    public IEnumerator LoadImage(string url, Action<Texture2D> onSuccess)
    {
        var webRequest = UnityWebRequestTexture.GetTexture(url);
        
        yield return webRequest.SendWebRequest();
        
        Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
        
        onSuccess?.Invoke(texture);
    }

    public IEnumerator GetRequest(string url, Action<string> onSuccess)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            var data = webRequest.downloadHandler.text;
            onSuccess?.Invoke(data);
        }
        else
        {
            Debug.LogError("Get request failed");
        }
    }
}