using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JAnimationSplit))]
public class JAnimationSplitInspector : Editor {
	JAnimationSplit thisTarget;

	string m_input_path;
	string m_output_path;

	Animation m_animation;
	string path;
	Rect rect;

	// Use this for initialization
	void OnEnable ()
	{
		thisTarget = serializedObject.targetObject as JAnimationSplit;
		m_animation = thisTarget.GetComponent<Animation>();
	}

	// Update is called once per frame
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		EditorGUILayout.LabelField("路径");
		rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
		path = EditorGUI.TextField(rect, path);
		path.SetEnabled(false);
		
		if ((Event.current.type == EventType.DragUpdated
		  || Event.current.type == EventType.DragExited)
		  && rect.Contains(Event.current.mousePosition))
		{
			//改变鼠标的外表  
			DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
			{
				path = DragAndDrop.paths[0];
			}
		}

		if (GUILayout.Button("Split"))
		{
			string filename = @"J:\Unity\Nine\Assets\JAnimationSplit\Models\Animation\jj_run.anim";
			AnimationClip cilp = JAnimationUtility.LoadAni_rotationCurve(filename);
			m_animation.AddClip(cilp, cilp.name);
			m_animation.Play(cilp.name);

			string new_filename = "Assets/JAnimationSplit/Models/Animation/" + cilp.name + ".anim";
			AssetDatabase.DeleteAsset(new_filename);
			AssetDatabase.CreateAsset(cilp, new_filename);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}
