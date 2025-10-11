using FluentAssertions;
using FoodRescue.Web.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoodRescue.Tests.Services;

public class TestDataServiceTests
{
    private readonly Mock<ILogger<TestDataService>> _loggerMock;
    private readonly TestDataService _sut;

    public TestDataServiceTests()
    {
        _loggerMock = new Mock<ILogger<TestDataService>>();
        _sut = new TestDataService(_loggerMock.Object);
    }

    [Fact]
    public void GenerateFoodDonations_ShouldReturnCorrectCount()
    {
        // Arrange
        const int count = 10;

        // Act
        var result = _sut.GenerateFoodDonations(count);

        // Assert
        result.Should().HaveCount(count);
    }

    [Fact]
    public void GenerateFoodDonations_ShouldReturnDefaultCountWhenNotSpecified()
    {
        // Act
        var result = _sut.GenerateFoodDonations();

        // Assert
        result.Should().HaveCount(10);
    }

    [Fact]
    public void GenerateFoodDonations_ShouldGenerateUniqueIds()
    {
        // Arrange
        const int count = 20;

        // Act
        var result = _sut.GenerateFoodDonations(count);

        // Assert
        result.Select(d => d.Id).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateFoodDonations_ShouldGenerateValidData()
    {
        // Act
        var result = _sut.GenerateFoodDonations(5);

        // Assert
        result.Should().AllSatisfy(donation =>
        {
            donation.Id.Should().BeGreaterThan(0);
            donation.DonorName.Should().NotBeNullOrWhiteSpace();
            donation.FoodType.Should().NotBeNullOrWhiteSpace();
            donation.Quantity.Should().BeGreaterThan(0);
            donation.Unit.Should().NotBeNullOrWhiteSpace();
            donation.PickupLocation.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public void GenerateFoodDonations_ShouldGenerateDonationsWithinLast30Days()
    {
        // Arrange
        var now = DateTime.Now;
        var thirtyDaysAgo = now.AddDays(-30);

        // Act
        var result = _sut.GenerateFoodDonations(10);

        // Assert
        result.Should().AllSatisfy(donation =>
        {
            donation.DonationDate.Should().BeAfter(thirtyDaysAgo.AddDays(-1));
            donation.DonationDate.Should().BeBefore(now.AddDays(1));
        });
    }
}
