namespace Zork
{
    public class Item
    {
        public string Name { get; set; }

        public int Damage { get; set; }

        public bool Eatable { get; set; }

        public int ItemNum { get; set; }

        public Item(string name, int damage, bool eatable, int itemNum)
        {
            Name = name;
            Damage = damage;
            Eatable = eatable;
            ItemNum = itemNum;
        }
    }
}
