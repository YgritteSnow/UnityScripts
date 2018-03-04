using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class JAnimationClip
{
	AnimationCurve[] m_curve;
	// 新建曲线
	void NewCurve(string bind_name) { return; }
	// 获取曲线
	AnimationCurve GetAnimationCurve(int index) { return m_curve[index]; }
}

public class JAnimationUtility : MonoBehaviour
{
	#region 仅加载Rotation曲线
	static public AnimationClip LoadAni_rotationCurve(string filename)
	{
		AnimationClip res = new AnimationClip();

		StreamReader sr = new StreamReader(filename, Encoding.Default);
		string line;
		while ((line = sr.ReadLine()) != null)
		{
			if (Regex.Match(line, @"m_Name:").Success)
			{
				res.name = Regex.Match(line, @"m_Name:\s*(\w+)").Groups[1].Value + "_jjj";
			}
			else if (Regex.Match(line, @"m_RotationCurves").Success)
			{
				string curve_line;
				string path = "";
				AnimationCurve curve_x = null;
				AnimationCurve curve_y = null;
				AnimationCurve curve_z = null;
				AnimationCurve curve_w = null;
				while ((curve_line = sr.ReadLine()) != null)
				{
					if (Regex.Match(curve_line, @"- curve:").Success)
					{
						curve_x = new AnimationCurve();
						curve_y = new AnimationCurve();
						curve_z = new AnimationCurve();
						curve_w = new AnimationCurve();

						string time_line;
						while ((time_line = sr.ReadLine()) != null)
						{
							if (Regex.Match(time_line, @"time:").Success)
							{
								float time = float.Parse(Regex.Match(time_line, @"time:\s*([\d.\-e+]+)").Groups[1].Value);
								GroupCollection value_group = Regex.Match(time_line = ReadIntactLine(sr), @" value:\s*{x:\s*([\d.\-e+]+),\s*y:\s*([\d.\-e+]+),\s*z:\s*([\d.\-e+]+),\s*w:\s*([\d.\-e+]+)").Groups;
								GroupCollection inSlope_group = Regex.Match(time_line = ReadIntactLine(sr), @" inSlope:\s*{x:\s*([\d.\-e+]+),\s*y:\s*([\d.\-e+]+),\s*z:\s*([\d.\-e+]+),\s*w:\s*([\d.\-e+]+)").Groups;
								GroupCollection outSlope_group = Regex.Match(time_line = ReadIntactLine(sr), @" outSlope:\s*{x:\s*([\d.\-e+]+),\s*y:\s*([\d.\-e+]+),\s*z:\s*([\d.\-e+]+),\s*w:\s*([\d.\-e+]+)").Groups;
								if(value_group.Count < 5 || inSlope_group.Count < 5 || outSlope_group.Count < 5)
								{
									Debug.Log("Error!!!");
								}
								Vector4 value = new Vector4(float.Parse(value_group[1].Value), float.Parse(value_group[2].Value), float.Parse(value_group[3].Value), float.Parse(value_group[4].Value));
								Vector4 inSlope = new Vector4(float.Parse(inSlope_group[1].Value), float.Parse(inSlope_group[2].Value), float.Parse(inSlope_group[3].Value), float.Parse(inSlope_group[4].Value));
								Vector4 outSlope = new Vector4(float.Parse(outSlope_group[1].Value), float.Parse(outSlope_group[2].Value), float.Parse(outSlope_group[3].Value), float.Parse(outSlope_group[4].Value));
								curve_x.AddKey(new Keyframe(time, value.x, inSlope.x, outSlope.x));
								curve_y.AddKey(new Keyframe(time, value.y, inSlope.y, outSlope.y));
								curve_z.AddKey(new Keyframe(time, value.z, inSlope.z, outSlope.z));
								curve_w.AddKey(new Keyframe(time, value.w, inSlope.w, outSlope.w));
							}
						}

						if (Regex.Match(time_line, @"path:").Success)
						{
							path = Regex.Match(time_line, @"path:\s*([\w.]+)").Groups[1].Value;
							break;
						}
					}

					if (Regex.Match(curve_line, @"m_RotationOrder:").Success)
					{
						break;
					}
				}
				if (curve_x != null)
				{
					res.SetCurve(path, typeof(Transform), "m_LocalRotation.x", curve_x);
					res.SetCurve(path, typeof(Transform), "m_LocalRotation.y", curve_y);
					res.SetCurve(path, typeof(Transform), "m_LocalRotation.z", curve_z);
					res.SetCurve(path, typeof(Transform), "m_LocalRotation.w", curve_w);
				}
			}
		}

		return res;
	}
	#endregion

	#region 工具函数：读取一整行，如果有右括号遗留，那么继续读取下一行，并且拼接起来返回
	public static string ReadIntactLine(StreamReader sr)
	{
		string line;
		string res_line = "";
		string reg_left = @"({)";
		string reg_right = @"(})";
		int left_count = 0;
		int right_count = 0;
		while ((line = sr.ReadLine()) != null)
		{
			left_count += Regex.Match(line, reg_left).Groups.Count;
			right_count += Regex.Match(line, reg_right).Groups.Count;
			res_line = res_line + line;
			if(left_count <= right_count)
			{
				return res_line;
			}
		}
		return res_line;
	}
	#endregion

	#region 加载Editor曲线（貌似不能用）
	static public AnimationClip LoadAni_editorCurve(string filename)
	{
		AnimationClip res = new AnimationClip();

		StreamReader sr = new StreamReader(filename, Encoding.Default);
		string line;
		while ((line = sr.ReadLine()) != null)
		{
			if (Regex.Match(line, @"m_Name:").Success)
			{
				res.name = Regex.Match(line, @"m_Name:\s*(\w+)").Groups[1].Value + "_jjj";
			}
			else if (Regex.Match(line, @"m_EditorCurve").Success)
			{
				string curve_line;
				string path = "";
				string propertyName = "";
				AnimationCurve curve = null;
				while ((curve_line = sr.ReadLine()) != null)
				{
					if (Regex.Match(curve_line, @"- curve:").Success)
					{
						curve = new AnimationCurve();

						string time_line;
						while ((time_line = sr.ReadLine()) != null)
						{
							if (Regex.Match(time_line, @"time:").Success)
							{
								float time = float.Parse(Regex.Match(time_line, @"time:\s*([\d.\-e+]+)").Groups[1].Value);
								float value = float.Parse(Regex.Match((time_line = sr.ReadLine()), @"value:\s*([\d.\-e+]+)").Groups[1].Value);
								float inSlope = float.Parse(Regex.Match((time_line = sr.ReadLine()), @"inSlope:\s*([\d.\-e+]+)").Groups[1].Value);
								float outSlope = float.Parse(Regex.Match((time_line = sr.ReadLine()), @"outSlope:\s*([\d.\-e+]+)").Groups[1].Value);
								curve.AddKey(new Keyframe(time, value, inSlope, outSlope));
							}

							if (Regex.Match(time_line, @"attribute:").Success)
							{
								propertyName = Regex.Match(time_line, @"attribute:\s*([\w.]+)").Groups[1].Value;
							}
							else if (Regex.Match(time_line, @"path:").Success)
							{
								path = Regex.Match(time_line, @"path:\s*([\w.]+)").Groups[1].Value;
							}

							if (Regex.Match(time_line, @"m_RotationOrder:").Success)
								break;
						}
					}
					else if (Regex.Match(curve_line, @"script:").Success)
						break;
				}
				if(curve != null)
				{
					res.SetCurve(path, typeof(Transform), propertyName, curve);
				}
			}
		}

		return res;
	}
	#endregion
}
