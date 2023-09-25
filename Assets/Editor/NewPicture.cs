using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NewPicture : EditorWindow
{
    [MenuItem("Tool/NewPicture")]
    public static void NewWindow()
    {
        var window= GetWindow(typeof(NewPicture),true,"图片生成窗口");
        window.Show();
        window.minSize = new Vector2(200, 300);
    }
    Object prefab;

    string path, path2;

    void OnGUI()
    {


        if (GUILayout.Button("生成图片"))
        {
            path = Application.dataPath + "/Picture";
            path2 ="Assets/prefab";
            if (!Directory.Exists(path))
            {
                //创建
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(path2))
            {
                //创建
                Directory.CreateDirectory(path2);
            }
            //刷新
            AssetDatabase.Refresh();



            List<Object> _list = GetAllPrefabByDirectory(path2);
            if (_list.Count < 0) return;

            for (int i = 0; i < _list.Count; i++)
            {
                EditorUtility.SetDirty(_list[i]);
                Texture2D image = AssetPreview.GetAssetPreview(_list[i]);
                System.IO.File.WriteAllBytes(path + "/" + _list[i].name + ".png", image.EncodeToPNG());
            }

        }
    }


    /// <summary>
    /// 格式化路径成Asset的标准格式
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string FormatAssetPath(string filePath)
    {
        var newFilePath1 = filePath.Replace("\\", "/");
        var newFilePath2 = newFilePath1.Replace("//", "/").Trim();
        newFilePath2 = newFilePath2.Replace("///", "/").Trim();
        newFilePath2 = newFilePath2.Replace("\\\\", "/").Trim();
        return newFilePath2;
    }

    public static List<Object> GetAllPrefabByDirectory(string path)
    {

        string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = FormatAssetPath(files[i]);
        }

        List<Object> _prefabList = new List<Object>();
        Object _prefab;
        foreach (var _path in files)
        {
            _prefab = AssetDatabase.LoadAssetAtPath(_path, typeof(Object)) as Object;//????
            _prefabList.Add(_prefab);
        }
        return _prefabList;
    }

}
