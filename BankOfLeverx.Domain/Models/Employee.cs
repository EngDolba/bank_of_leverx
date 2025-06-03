namespace BankOfLeverx.Domain.Models
{
    public class Employee
    {
        public required string Name { get; set; }

        public required string Surname { get; set; }
        public  int Key { get; set; }

        public required string Position { get; set; }

        public required string Branch { get; set; }
    }
}
