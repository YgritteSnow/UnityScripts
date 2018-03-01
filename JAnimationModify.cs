using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class JAnimationModify : MonoBehaviour {
	Animation m_animation = null;
	AnimationClip m_ani_clip;
	public string xxx;

	void Start () {
		m_animation = GetComponent<Animation>();
	}
}
