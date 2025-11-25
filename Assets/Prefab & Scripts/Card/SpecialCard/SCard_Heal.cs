using UnderGroundPoker.Prefab.Card;
using UnderGroundPoker.Manager;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    public class SCard_Heal : SpecialCard
    {
        public override void ApplyEffect(PlayerManager player)
        {
            player.PlayerLife++;
            //Debug.Log($"Heal 카드 사용, {player.gameObject.name} 체력 1 회복");
        }
    }
}