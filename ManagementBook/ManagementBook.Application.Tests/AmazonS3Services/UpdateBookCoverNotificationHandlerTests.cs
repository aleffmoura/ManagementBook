namespace ManagementBook.Application.Tests.AmazonS3Services;

using Amazon.S3.Transfer;
using FluentAssertions;
using ManagementBook.Application.Features.Utilities;
using ManagementBook.Application.Notifications.Books;
using ManagementBook.Domain.Books;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;

[TestFixture]
public class UpdateBookCoverNotificationHandlerTests
{
    private Mock<ITransferUtility> _transferUtility;
    private Mock<IConfigurationRoot> _mockConfiguration;
    private Mock<IBookRepository> _bookRepository;

    private AmazonS3Service _amazonS3Service;
    private UpdateBookCoverNotificationHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _transferUtility = new();
        _mockConfiguration = new();
        _bookRepository = new();
        _amazonS3Service = new(_transferUtility.Object, _mockConfiguration.Object);
        _handler = new(_amazonS3Service, _bookRepository.Object);
    }

    [Test]
    public async Task UpdateBookCoverNotificationHandlerTests_HandleNotification_ShouldBeSucess()
    {
        //arrange
        CancellationTokenSource cts = new();
        BookCoverNotification bookCoverNotification = new()
        {
            BookId = Guid.NewGuid(),
            Data = [1, 1, 1, 1, 1, 10, 1, 1, 2, 1, 2, 1,]
        };
        var bucketName = "bucketName";
        var key = "key";

        _mockConfiguration.Setup(c => c["amazon:s3:bucketName"])
                          .Returns(bucketName)
                          .Verifiable();

        _mockConfiguration.Setup(c => c["amazon:s3:key"])
                          .Returns(key)
                          .Verifiable();

        _transferUtility.Setup(s => s.UploadAsync(It.IsAny<TransferUtilityUploadRequest>(), It.IsAny<CancellationToken>()));

        //action
        var action = async () => await _handler.Handle(bookCoverNotification, cts.Token);

        //arrange
        await action.Should().NotThrowAsync();

        _mockConfiguration.Verify();
        _mockConfiguration.VerifyNoOtherCalls();
    }
}
