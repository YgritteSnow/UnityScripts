using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JAnimationUtility))]
[RequireComponent(typeof(JAnimationUtilityInspector))]
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
			string filename = @"J:\Unity\Nine\Assets\JAnimationMerge\Models\Animation\jj_run.anim";
			bool exi = System.IO.Directory.Exists(filename);
			AnimationClip ani = JAnimationUtility.LoadAni_rotationCurve(filename);
			m_animation.AddClip(ani, ani.name);
			m_animation.Play(ani.name);
		}
	}
}
