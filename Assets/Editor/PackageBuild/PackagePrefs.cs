using UnityEditor;
using UnityEngine;

public enum ChannelPrefsKey
{
	None = 0,
	AndroidBuildABForPerChannelPrefsKey = 1,
	IOSBuildABForPerChannelPrefsKey = 2,
	StandaloneWindowsBuildABForPerChannelPrefsKey = 3,
	StandaloneWindows64BuildABForPerChannelPrefsKey = 4,
	WebGlBuildABForPerChannelPrefsKey = 5,
}

public class PackagePrefs
{
	public static bool GetBuildABForPerChannelSetting(ChannelPrefsKey channel_prefs_key) 
	{
		string channel_prefs_key_string = channel_prefs_key.ToString();
		//Debug.LogError(channel_prefs_key_string);
		if (!EditorPrefs.HasKey(channel_prefs_key_string))
		{
			SaveBuildABForPerChannelSetting(channel_prefs_key, false);
			return false;
		}
		bool enable = EditorPrefs.GetBool(channel_prefs_key_string, false);
		return enable;
	}
	public static void SaveBuildABForPerChannelSetting(ChannelPrefsKey channel_prefs_key, bool enable)
	{
		string channel_prefs_key_string = channel_prefs_key.ToString();
		EditorPrefs.SetBool(channel_prefs_key_string, enable);
	}
}
