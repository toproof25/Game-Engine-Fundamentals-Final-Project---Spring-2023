using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Scriptable Object Asset/PlayerState")]
public class PlayerStateInfo : ScriptableObject
{


    public float MaxHP;
    public float HP;
    public float Speed;
    public float BulletSpeed;
    public float Damage;
    public string Gun ;
    public float secondGunDamage;
    public string secondGunName;
    public int OnGun;
    public int accuracy;
    public int[] TotalNumberOfBullets = { 0, 0, 0 };
    public int[] NumberOfBullets = { 20, 30, 10 };

    

    
}

