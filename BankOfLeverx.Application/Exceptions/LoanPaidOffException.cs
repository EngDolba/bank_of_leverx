namespace BankOfLeverx.Application.Exceptions
{
    [Serializable]
    public class LoanPaidOffException : Exception
    {
        public LoanPaidOffException()
        {
        }

        public LoanPaidOffException(string? message) : base(message)
        {
        }

        public LoanPaidOffException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}