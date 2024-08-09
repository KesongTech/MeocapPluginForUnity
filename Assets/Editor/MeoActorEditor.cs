using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Meocap;
namespace Meocap.UI
{
    [CustomEditor(typeof(Perform.MeoActor))]
    public class MeoActorEditor : Editor
    {
        private SerializedProperty animatorProperty;
        private SerializedProperty targetTransformProperty;
        private SerializedProperty boneMapProperty;
        protected void OnEnable()
        {
            animatorProperty = serializedObject.FindProperty("animator");
            targetTransformProperty = serializedObject.FindProperty("target");
            boneMapProperty = serializedObject.FindProperty("bone_map"); // ������Ǹ� Asset
        }
        public override void OnInspectorGUI()
        {
            Perform.MeoActor actor = (Perform.MeoActor)target;
            EditorGUILayout.BeginHorizontal();
            if (actor.animator != null)
            {
                if (actor.animator.enabled && actor.animator.isHuman)
                {
                    // Todo: Append pre-processing
                }
                else
                {
                    EditorGUILayout.HelpBox("Animator��Avatar���ͱ���Ϊ����!", MessageType.Error);
                }

            }
            else
            {
                EditorGUILayout.HelpBox("������һ�� Animator!", MessageType.Error);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(animatorProperty);
            EditorGUILayout.PropertyField(targetTransformProperty);
            if (GUILayout.Button("�Զ�����Transform��Animator"))
            {
                actor.animator = actor.GetComponentInChildren<Animator>();
                actor.target = actor.transform;
            }

            if (actor.animator == null)
            {
                EditorGUILayout.HelpBox("MeoActor������������һ��ӵ�� animator ��Object�ϣ���Ҫ��ӵ������avatar!", MessageType.Error);
            }
            else if (!actor.animator.isHuman)
            {
                EditorGUILayout.HelpBox("AnimatorҪ��ӵ������avatar!", MessageType.Error);

            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}
