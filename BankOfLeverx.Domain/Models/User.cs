namespace BankOfLeverx.Domain.Models
{
    public class User
    {
        public required int Key { get; set; }
        public required string Username { get; set; }

        public required string HashedPassword { get; set; }
        public required int Role { get; set; }
    }

}
