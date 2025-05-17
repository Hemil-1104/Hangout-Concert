using UnityEditor;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class ExtractAnimationFromFBX : EditorWindow
    {
        private Object fbxObject;
        private string newPath;

        [MenuItem("Tools/Extract Animation Clip from FBX")]
        public static void ShowWindow()
        {
            GetWindow<ExtractAnimationFromFBX>("Extract Animation Clip from FBX");
        }

        private void OnGUI()
        {
            fbxObject = EditorGUILayout.ObjectField("FBX Object", fbxObject, typeof(Object), allowSceneObjects: true);
            newPath = EditorGUILayout.TextField("Extracted Clip Path", newPath);

            if (GUILayout.Button("Extract and Save"))
            {
                if(fbxObject != null)
                {
                    string fbxObjectPath = AssetDatabase.GetAssetPath(fbxObject);

                    AnimationClip refAnimationClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(fbxObjectPath, typeof(AnimationClip));

                    AnimationClip extractedAnimationClip = new AnimationClip();

                    EditorCurveBinding[] editorCurveBindings = AnimationUtility.GetCurveBindings(refAnimationClip);
                    string clipName = GetProperNameForAnimationClip(fbxObject.name);
                    
                    foreach(var binding in editorCurveBindings)
                    {
                        AnimationCurve animationCurve = AnimationUtility.GetEditorCurve(refAnimationClip, binding);
                        AnimationUtility.SetEditorCurve(extractedAnimationClip, binding, animationCurve);
                    }

                    AssetDatabase.CreateAsset(extractedAnimationClip, newPath + $"/{clipName}.anim");
                    AssetDatabase.SaveAssets();
                }
            }
        }

        private string GetProperNameForAnimationClip(string fbxFileName)
        {
            string newFileName = "";

            if (char.IsLetter(fbxFileName[0]))
            {
                newFileName += char.ToUpper(fbxFileName[0]);
            }

            bool isNextLetterCapital = false;
            for(int i = 1; i < fbxFileName.Length; i++)
            {
                char ch = fbxFileName[i];
                if (char.IsNumber(ch))
                {
                    newFileName += $"_{ch}";
                }
                else if (ch == ' ')
                {
                    isNextLetterCapital = true;
                    newFileName += '_';
                }
                else
                {
                    if (isNextLetterCapital)
                    {
                        newFileName += char.ToUpper(ch);
                        isNextLetterCapital = false;
                    }
                    else
                    {
                        newFileName += ch;
                    }
                }
            }

            return newFileName;
        }
    }
}