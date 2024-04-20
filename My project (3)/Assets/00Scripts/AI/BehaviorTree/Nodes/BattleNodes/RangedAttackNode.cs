using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(RangedAttackNode), "원거리 공격")]
    public class RangedAttackNode : AttackNode
    {
        public GameObject bulletPrefab;
        public float bulletDuration = 5;
        public float bulletSpeed = 10;

        protected override void OnStart()
        {
            base.OnStart();

            if (!blackboard.CheckTarget()) { return; }
            if (bulletPrefab == null) { return; }

            DamageInfo damageInfo = new DamageInfo(agent.arum, blackboard.target, 1, agent.arum.status.GetStatus(agent.arum.knockbackKey));

            var bullet = Instantiate(bulletPrefab, agent.transform.position, Quaternion.identity).GetComponent<BulletController>();
            var dir = (damageInfo.Defender.transform.position - damageInfo.Attacker.transform.position).normalized;
            bullet.SetBullet(dir, bulletDuration, bulletSpeed, 6, damageInfo);
        }

        protected override void OnStop()
        {

        }
    }
}