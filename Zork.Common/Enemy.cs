namespace Zork
{
    public class Enemy
    {
        public string Name { get; set; }

        public float HitPoints { get; set; }
        public float MaxHitPoints { get; set; }

        public string DefeatMessage { get; set; }

        public int DefeatScore { get; set; }

        public int EnemyNum { get; set; }
    }
}
