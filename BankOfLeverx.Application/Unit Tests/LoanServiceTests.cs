using AutoMapper;
using BankOfLeverx.Application.Services;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class LoanServiceTests
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

        var mockRepo = new Mock<ILoanRepository>();
        var mockMapper = new Mock<IMapper>();

        setupMocks(mockMapper, mockRepo, loanKey, originalLoan);

        var service = new LoanService(mockRepo.Object, mockMapper.Object);

        var result = await service.SubtractInterestAsync(loanKey);

        Assert.NotNull(result);
        Assert.Equal(expectedAmount, result.Amount, 2); 

        mockRepo.Verify(r => r.UpdateAsync(
            It.Is<Loan>(loan => Math.Abs(loan.Amount - expectedAmount) < 0.01)
        ), Times.Once);
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

        var mockRepo = new Mock<ILoanRepository>();
        var mockMapper = new Mock<IMapper>();

        setupMocks(mockMapper, mockRepo, loanKey, originalLoan);

        var service = new LoanService(mockRepo.Object, mockMapper.Object);

        var result = await service.SubtractInterestAsync(loanKey);

        Assert.NotNull(result);
        Assert.Equal(expectedAmount, result.Amount, 2);

        mockRepo.Verify(r => r.UpdateAsync(
            It.Is<Loan>(loan => Math.Abs(loan.Amount - expectedAmount) < 0.01)
        ), Times.Once);
    }
    private void setupMocks(Mock<IMapper> mockMapper, Mock<ILoanRepository> mockRepo,int loanKey,Loan originalLoan)
    {

        mockRepo.Setup(r => r.GetByIdAsync(loanKey)).ReturnsAsync(originalLoan);

        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Loan>()))
            .ReturnsAsync((Loan loan) => loan);

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

        mockMapper.Setup(m => m.Map<Loan>(It.IsAny<LoanDTO>()))
          .Returns((LoanDTO dto) => new Loan
          {
              Key = 1,
              Amount = dto.Amount,
              Rate = dto.Rate,
              AccountKey = dto.AccountKey,
              BankerKey = dto.BankerKey,
              EndDate = dto.EndDate,
              StartDate = dto.StartDate,
              InitialAmount = dto.InitialAmount,
              Type = dto.Type
          });

    }
}
