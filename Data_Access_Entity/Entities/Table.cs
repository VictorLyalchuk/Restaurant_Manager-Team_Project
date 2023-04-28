namespace Data_Access_Entity.Entities
{
    public class Table
    {
        public int ID { get; set; }
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }

    }
}