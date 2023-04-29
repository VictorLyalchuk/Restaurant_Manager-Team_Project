namespace Data_Access_Entity.Entities
{
    [Serializable]
    public class Table
    {
        public int ID { get; set; }
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        public bool Active { get; set; }
    }
}