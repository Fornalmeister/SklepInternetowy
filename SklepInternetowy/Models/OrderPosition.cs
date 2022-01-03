namespace SklepInternetowy.Models
{
    public class OrderPosition
    {
        public int OrderPositionId { get; set; }
        public int OrderId { get; set; }
        public int KursId { get; set; }
        public int quantity { get; set; }
        public decimal OrderCost { get; set; }

        public virtual Kurs Kurs { get; set; }
        public virtual Order Order { get; set; }
    }
}