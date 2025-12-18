using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyID = "Slime";
    [SerializeField] private int hp = 3;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) Die();
    }

    private void Die()
    {
        QuestManager.Instance.OnEnemyKilled(enemyID);
        Destroy(gameObject);
    }
}

