using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JAnimationUtility))]
public class JAnimationUtilityInspector : Editor {
	JAnimationUtility thisTarget;
	Animation m_animation;

	// Use this for initialization
	void OnEnable ()
	{
		thisTarget = serializedObject.targetObject as JAnimationUtility;
		m_animation = thisTarget.GetComponent<Animation>();
	}

	// Update is called once per frame
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		if (GUILayout.Button("Set"))
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
