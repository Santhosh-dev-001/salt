// attach this to the Animator GO if OnDropAnimation is on a parent
using SaltAnalysis.Interaction;
using UnityEngine;

public class AnimationEventBridge : MonoBehaviour
{
    [SerializeField] OnDropAnimation _target;

    public void OnAnimationComplete() => _target.OnAnimationComplete();
}