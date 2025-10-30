using UnityEngine;
using UnderGroundPoker.Prefab.Manager;
using UnderGroundPoker.Manager;
using UnderGroundPoker.Objects;

namespace UnderGroundPoker.Prefab.Card
{
    public class SCard_ChangeBullet : SpecialCard
    {
        public override void ApplyEffect(PlayerManager player)
        {
            Shotgun shotgun = GameManager.Instance.Shotgun;
            var magazine = shotgun.Bulletinfo;
            if (magazine != null)
            {
                Bullet target = magazine[0];

                //총알의 상태를 변경
                target.isFalse = !target.isFalse;

                //정보 갱신 및 TODO : UI나 연출 등을 통해서 플레이어에게 알림
                shotgun.RefreshMag();
            }

        }
    }
}