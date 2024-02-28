using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MeocapActorBoneMap", menuName = "Meocap/ActorBoneMap", order = 1)]
public class MeocapActorBoneMap : ScriptableObject
{
    [System.Serializable]
    public struct BoneTransformPair
    {
        public HumanBodyBones bone;
        public Transform transform;
    }

    [System.Serializable]
    public struct BoneQuaternionPair
    {
        public HumanBodyBones bone;
        public Quaternion quaternion;
    }

    public List<BoneTransformPair> animatorHumanBonePairs;
    public List<BoneQuaternionPair> offsetsPairs;
    public List<BoneQuaternionPair> characterTPosePairs;

    // ������ʱʹ�õ��ֵ䣨����Ҫ���л���
    [System.NonSerialized] public Dictionary<HumanBodyBones, Transform> animatorHumanBones = new Dictionary<HumanBodyBones, Transform>();
    [System.NonSerialized] public Dictionary<HumanBodyBones, Quaternion> offsets = new Dictionary<HumanBodyBones, Quaternion>();
    [System.NonSerialized] public Dictionary<HumanBodyBones, Quaternion> characterTPose = new Dictionary<HumanBodyBones, Quaternion>();

    public void OnEnable()
    {
        BuildDictionaries();
    }

    public void BuildDictionaries()
    {
        animatorHumanBones.Clear();
        offsets.Clear();
        characterTPose.Clear();

        foreach (var pair in animatorHumanBonePairs)
        {
            animatorHumanBones[pair.bone] = pair.transform;
        }

        foreach (var pair in offsetsPairs)
        {
            offsets[pair.bone] = pair.quaternion;
        }

        foreach (var pair in characterTPosePairs)
        {
            characterTPose[pair.bone] = pair.quaternion;
        }
    }
    public void UpdateListsFromDictionaries()
    {
        // ��������б�
        animatorHumanBonePairs.Clear();
        offsetsPairs.Clear();
        characterTPosePairs.Clear();

        // ��animatorHumanBone�ֵ�����б�
        foreach (var pair in animatorHumanBones)
        {
            animatorHumanBonePairs.Add(new BoneTransformPair { bone = pair.Key, transform = pair.Value });
        }

        // ��offsets�ֵ�����б�
        foreach (var pair in offsets)
        {
            offsetsPairs.Add(new BoneQuaternionPair { bone = pair.Key, quaternion = pair.Value });
        }

        // ��characterTPose�ֵ�����б�
        foreach (var pair in characterTPose)
        {
            characterTPosePairs.Add(new BoneQuaternionPair { bone = pair.Key, quaternion = pair.Value });
        }
    }
}
