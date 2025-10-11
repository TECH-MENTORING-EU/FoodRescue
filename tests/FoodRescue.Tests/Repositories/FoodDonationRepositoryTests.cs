using System.Data;
using Dapper;
using FluentAssertions;
using FoodRescue.Web.Models;
using FoodRescue.Web.Repositories;
using FoodRescue.Web.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;

namespace FoodRescue.Tests.Repositories;

public class FoodDonationRepositoryTests
{
    private readonly Mock<IDatabaseService> _databaseServiceMock;
    private readonly Mock<IDbConnection> _connectionMock;
    private readonly Mock<ILogger<FoodDonationRepository>> _loggerMock;
    private readonly FoodDonationRepository _sut;

    public FoodDonationRepositoryTests()
    {
        _databaseServiceMock = new Mock<IDatabaseService>();
        _connectionMock = new Mock<IDbConnection>();
        _loggerMock = new Mock<ILogger<FoodDonationRepository>>();

        _databaseServiceMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _sut = new FoodDonationRepository(_databaseServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDonations()
    {
        // Arrange
        var expectedDonations = new List<FoodDonation>
        {
            new() { Id = 1, DonorName = "Test Donor 1", FoodType = "Bread" },
            new() { Id = 2, DonorName = "Test Donor 2", FoodType = "Vegetables" }
        };

        _connectionMock.SetupDapperAsync(c => c.QueryAsync<FoodDonation>(
            It.IsAny<string>(), 
            null, 
            null, 
            null, 
            null))
            .ReturnsAsync(expectedDonations);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedDonations);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDonation_WhenExists()
    {
        // Arrange
        var expectedDonation = new FoodDonation 
        { 
            Id = 1, 
            DonorName = "Test Donor", 
            FoodType = "Bread" 
        };

        _connectionMock.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<FoodDonation>(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            null, 
            null, 
            null))
            .ReturnsAsync(expectedDonation);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDonation);
    }

    [Fact]
    public void CreateAsync_MethodExists()
    {
        // Arrange
        var donation = new FoodDonation
        {
            DonorName = "Test Donor",
            FoodType = "Bread",
            Quantity = 10,
            Unit = "kg"
        };

        // Act & Assert
        // Verify the method exists and has the correct signature
        var method = typeof(FoodDonationRepository).GetMethod("CreateAsync");
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task<int>));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenDonationUpdated()
    {
        // Arrange
        var donation = new FoodDonation
        {
            Id = 1,
            DonorName = "Updated Donor",
            FoodType = "Bread"
        };

        _connectionMock.SetupDapperAsync(c => c.ExecuteAsync(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            null, 
            null, 
            null))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.UpdateAsync(donation);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenDonationNotFound()
    {
        // Arrange
        var donation = new FoodDonation { Id = 999 };

        _connectionMock.SetupDapperAsync(c => c.ExecuteAsync(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            null, 
            null, 
            null))
            .ReturnsAsync(0);

        // Act
        var result = await _sut.UpdateAsync(donation);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDonationDeleted()
    {
        // Arrange
        _connectionMock.SetupDapperAsync(c => c.ExecuteAsync(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            null, 
            null, 
            null))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
    }
}
