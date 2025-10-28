using UnityEngine;

namespace UnderGroundPoker.Animation
{
    /// <summary>
    /// 애니메이션 파라미터 해시모음
    /// </summary>
    public static class AnimHash
    {
        public static readonly int singleFire = Animator.StringToHash("Single");
        public static readonly int doubleFire = Animator.StringToHash("Double");
    }
}

