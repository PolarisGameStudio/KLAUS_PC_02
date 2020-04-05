/*
 * @author Valentin Simonov / http://va.lent.in/
 * Modified by Luis Vieira
 */

using TouchScript.Gestures;
using UnityEditor;
using UnityEngine;

namespace TouchScript.Editor.Gestures
{
	[CustomEditor(typeof(FlickCrazyGesture), true)]
	internal sealed class FlickCrazyGestureEditor : GestureCrazyEditor
	{
		
		private static readonly GUIContent DIRECTION = new GUIContent("Direction", "Flick direction.");
		private static readonly GUIContent MOVEMENT_THRESHOLD = new GUIContent("Movement Threshold (cm)", "Minimum distance in cm touch points must move for the gesture to begin.");
		private static readonly GUIContent MIN_DISTANCE = new GUIContent("Minimum Distance (cm)", "Minimum distance in cm touch points must move in <Flick Time> seconds for the gesture to be recognized.");
		
		private SerializedProperty direction;
		private SerializedProperty minDistance;
		private SerializedProperty movementThreshold;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			minDistance = serializedObject.FindProperty("minDistance");
			movementThreshold = serializedObject.FindProperty("movementThreshold");
			direction = serializedObject.FindProperty("direction");
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfDirtyOrScript();
			
			EditorGUIUtility.labelWidth = 180;
			EditorGUILayout.PropertyField(direction, DIRECTION);
			EditorGUILayout.PropertyField(movementThreshold, MOVEMENT_THRESHOLD);
			EditorGUILayout.PropertyField(minDistance, MIN_DISTANCE);
			
			serializedObject.ApplyModifiedProperties();
			base.OnInspectorGUI();
		}
	}
}