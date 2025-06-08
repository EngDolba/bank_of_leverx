using AutoMapper;
using BankOfLeverx.Application.Services;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using Moq;
using Xunit;

public class LoanServiceTests
{ 
    [Fact]
    public async Task SubtractInterestServiceTest()
    {
        // Arrange
        int loanKey = 1;
        double originalAmount = 1200;
        double expectedAmount = 1180;

        var originalLoan = new Loan
        {
            Key = loanKey,
            Amount = originalAmount,
            Rate = 12,
            AccountKey = 0,
            BankerKey = 0,
            EndDate = DateTime.Now,
            StartDate = DateTime.Now,
            InitialAmount = 100,
            Type = "tp"
        };

        var mockRepo = new Mock<ILoanRepository>();
        var mockMapper = new Mock<IMapper>();

        // Mock GetByIdAsync
        mockRepo.Setup(r => r.GetByIdAsync(loanKey)).ReturnsAsync(originalLoan);

        // Mock IMapper.Map<LoanDTO>
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

        // Mock UpdateAsync to return the updated Loan
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Loan>()))
            .ReturnsAsync((int k, LoanDTO dto) => new Loan
            {
                Key = k,
                Amount = dto.Amount,
                Rate = dto.Rate,
                AccountKey = dto.AccountKey,
                BankerKey = dto.BankerKey,
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
                InitialAmount = dto.InitialAmount,
                Type = dto.Type
            });

        // Create service
        var service = new LoanService(mockRepo.Object, mockMapper.Object);

        // Act
        var result = await service.SubtractInterestAsync(loanKey);

        // Assert
        Assert.Equal(expectedAmount, result.Amount, 2); // Allow precision margin
        mockRepo.Verify(r => r.UpdateAsync(
            It.Is<Loan>(dto => Math.Abs(dto.Amount - expectedAmount) < 0.01)
        ), Times.Once);
    }
}
