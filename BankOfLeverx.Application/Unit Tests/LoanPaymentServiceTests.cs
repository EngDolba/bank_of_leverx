using AutoMapper;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Application.Services;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using Moq;
using Xunit;

public class LoanPaymentServiceTests
{
    [Fact]
    public async Task SubtractInterestServiceTest()
    {
        int loanKey = 1;
        double originalAmount = 1200;
        double expectedAmount = 1188;
        double initialAmount = 1200;
        double interestRate = 12;

        var originalLoan = new Loan
        {
            Key = loanKey,
            Amount = originalAmount,
            Rate = interestRate,
            AccountKey = 0,
            BankerKey = 0,
            EndDate = DateTime.Now,
            StartDate = DateTime.Now,
            InitialAmount = initialAmount,
            Type = "tp"
        };

        var mLoanService = new Mock<ILoanService>();
        var mMapper = new Mock<IMapper>();
        var mTransactionService = new Mock<ITransactionService>();

        setupMocks(mMapper, mLoanService, mTransactionService, loanKey, originalLoan, expectedAmount);

        var service = new LoanPaymentService(mLoanService.Object, mMapper.Object, mTransactionService.Object);

        var result = await service.SubtractInterestAsync(loanKey);

        Assert.NotNull(result);
        Assert.Equal(expectedAmount, result.Amount, 2);

    }
    [Fact]
    public async Task SubtractInterestServiceTestForZero()
    {
        int loanKey = 1;
        double originalAmount = 11;
        double initialAmount = 1200;
        double expectedAmount = 0;
        double interestRate = 12;

        var originalLoan = new Loan
        {
            Key = loanKey,
            Amount = originalAmount,
            Rate = interestRate,
            AccountKey = 0,
            BankerKey = 0,
            EndDate = DateTime.Now,
            StartDate = DateTime.Now,
            InitialAmount = initialAmount,
            Type = "tp"
        };

        var mockService = new Mock<ILoanService>();
        var mockMapper = new Mock<IMapper>();
        var transactionServiceMock = new Mock<ITransactionService>();

        setupMocks(mockMapper, mockService, transactionServiceMock, loanKey, originalLoan,expectedAmount);

        var service = new LoanPaymentService(mockService.Object, mockMapper.Object, transactionServiceMock.Object);

        var result = await service.SubtractInterestAsync(loanKey);

        Assert.NotNull(result);
        Assert.Equal(expectedAmount, result.Amount, 2);

      
    }
    private void setupMocks(Mock<IMapper> mockMapper, Mock<ILoanService> mockLoan, Mock<ITransactionService> mockTransaction, int loanKey, Loan originalLoan,double expectedAmount)
    {

        mockLoan.Setup(r => r.GetByIdAsync(loanKey)).ReturnsAsync(originalLoan);
        originalLoan.Amount = expectedAmount;
        mockLoan.Setup(r => r.UpdateAsync(It.IsAny<int>(),It.IsAny<LoanDTO>()))
            .ReturnsAsync((int key,LoanDTO loan) => originalLoan);

        mockMapper.Setup(m => m.Map<LoanDTO>(It.IsAny<Loan>()))
            .Returns((Loan l) => new LoanDTO
            {
                Amount = l.Amount,
                Rate = l.Rate,
                AccountKey = l.AccountKey,
                BankerKey = l.BankerKey,
                EndDate = l.EndDate,
                StartDate = l.StartDate,
                InitialAmount = l.InitialAmount,
                Type = l.Type
            });

        mockTransaction.Setup(m => m.processTransaction(It.IsAny<int>(), It.IsAny<double>()))
             .ReturnsAsync((Transaction?)null);
         

    }
}
