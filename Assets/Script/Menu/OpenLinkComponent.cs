using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OpenLinkComponent : MonoBehaviour
{
    public string url = "https://vk.com/id219783144";
    public string urlGit = "https://github.com/Kim-David-2814";

    public void OpenProfile()
    {
        Application.OpenURL(url);
    }

    public void OpenGit()
    {
        Application.OpenURL(urlGit);
    }
}
