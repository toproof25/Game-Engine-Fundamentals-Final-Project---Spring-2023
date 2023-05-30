using UnityEngine;

[CreateAssetMenu(fileName = "BossState", menuName = "Scriptable Object Asset/BossState")]
public class BossStateInfo : ScriptableObject
{
    public float HP = 1000f;
    public float LeftShoulderHP = 200f;
    public float RightShoulderHP = 200f;

    public float Speed = 1f;
    public float RotationSpeed = 1f;
    public float FieldOfViewAngle = 90f;

    public float BulletSpeed = 20f;
    public float Damage = 30f;

}

