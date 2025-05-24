namespace bankOfLeverx.Models
{
    public class Customer
    {
        public required string Name { get; set; }

        public required string Surname { get; set; }
        public required int Key { get; set; }

        public required string Category { get; set; }

        public required string Branch { get; set; }
    }
}
