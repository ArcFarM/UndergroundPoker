using UnderGroundPoker.Prefab.Card;
using UnderGroundPoker.Prefab.Manager;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    public class SCard_HealorDmg : SpecialCard
    {
        public override void ApplyEffect(PlayerManager player)
        {
            int coinToss = Random.Range(0, 2);

            if (coinToss == 1)
            {
                player.PlayerLife += 2;
                //Debug.Log($"{player.gameObject.name} 체력 2 회복");
            }
            else
            {
                player.PlayerLife--;
                //Debug.Log($"{player.gameObject.name} 체력 1 감소");
            }
        }
    }
}