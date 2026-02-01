using NSubstitute;
using Library.ApplicationCore;
using Library.ApplicationCore.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace Library.UnitTests.Infrastructure.JsonLoanRepositoryTests;

public class GetLoan
{
    private readonly ILoanRepository _mockLoanRepository;
    private readonly JsonLoanRepository _jsonLoanRepository;
    private readonly IConfiguration _configuration;
    private readonly JsonData _jsonData;

    public GetLoan()
    {
        _mockLoanRepository = Substitute.For<ILoanRepository>();
         _configuration = new ConfigurationBuilder().Build();
        _jsonData = new JsonData(_configuration);
        _jsonLoanRepository = new JsonLoanRepository(_jsonData);
    }

    // TODO: Add test methods here
    
    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Returns loan when loan ID is found")]
    public async Task GetLoan_ReturnsLoanWhenFound()
    {
        // Arrange
        var expectedLoanId = 1; // This ID exists in Loans.json
        var expectedLoan = new Loan
        {
            Id = expectedLoanId,
            BookItemId = 17,
            PatronId = 22,
            LoanDate = DateTime.Parse("2023-12-08T00:40:43.1808862"),
            DueDate = DateTime.Parse("2023-12-22T00:40:43.1808862"),
            ReturnDate = null
        };
        
        _mockLoanRepository.GetLoan(expectedLoanId).Returns(expectedLoan);

        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(expectedLoanId);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(actualLoan),
            () => Assert.Equal(expectedLoanId, actualLoan.Id),
            () => Assert.Equal(17, actualLoan.BookItemId),
            () => Assert.Equal(22, actualLoan.PatronId),
            () => Assert.Null(actualLoan.ReturnDate)
        );
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Returns null when loan ID is not found")]
    public async Task GetLoan_ReturnsNullWhenNotFound()
    {
        // Arrange
        var nonExistentLoanId = 9999; // This ID does not exist in Loans.json
        _mockLoanRepository.GetLoan(nonExistentLoanId).Returns((Loan?)null);
        
        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(nonExistentLoanId);
        
        // Assert
        Assert.Null(actualLoan);
    }
}