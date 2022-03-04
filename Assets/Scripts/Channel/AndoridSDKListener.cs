using UnityEngine;

namespace GameChannel
{
    public class AndroidSDKListener : MonoSingleton<AndroidSDKListener>
    {
        private void InitCallback(string msg)
        {
            Debug.Log("InitSDKComplete with msg : " + msg);
            ChannelManager.instance.OnInitSDKCompleted(msg);
        }

        private void DownloadGameCallback(string msg)
        {
            Debug.Log("Download game with msg: " + msg);
            int result = -1;
            int.TryParse(msg, out result);
            ChannelManager.instance.OnDownloadGameFinished(result == 0);
        }

        private void DownloadGameProgressValueChangeCallback(string msg)
        {
            Debug.Log("Download game progress : " + msg);
            int progress = 0;
            int.TryParse(msg, out progress);
            ChannelManager.instance.OnDownloadGameProgressValueChange(progress);
        }

        private void InstallApkCallback(string msg)
        {
            Debug.Log("Install apk with msg: " + msg);
            int result = -1;
            int.TryParse(msg, out result);
            ChannelManager.instance.OnInstallGameFinished(result == 0);
        }

        private void LoginCallback(string msg)
        {
            Debug.Log("Login with msg : " + msg);
            ChannelManager.instance.OnLogin(msg);
        }

        private void LogoutCallback(string msg)
        {
            Debug.Log("Logout with msg : " + msg);
            ChannelManager.instance.OnLoginOut(msg);
        }

        private void PayCallback(string msg)
        {
            Debug.Log("SDKPay complete with msg : " + msg);
            ChannelManager.instance.OnSDKPay(msg);
        }
    }
}