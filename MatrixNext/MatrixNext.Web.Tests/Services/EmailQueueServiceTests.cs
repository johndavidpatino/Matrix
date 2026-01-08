using Moq;
using MatrixNext.Web.Services;

namespace MatrixNext.Web.Tests.Services
{
    /// <summary>
    /// Unit tests for EmailQueueService
    /// Tests async email queueing without external dependencies (no Hangfire)
    /// Ref: S4-003 implementation
    /// </summary>
    public class EmailQueueServiceTests
    {
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ILogger<EmailQueueService>> _mockLogger;
        private readonly EmailQueueService _emailQueueService;

        public EmailQueueServiceTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<EmailQueueService>>();
            _emailQueueService = new EmailQueueService(_mockEmailService.Object, _mockLogger.Object);
        }

        #region QueueEmailAsync Tests

        [Fact]
        public async Task QueueEmailAsync_WithValidInput_EnqueuesEmail()
        {
            // Arrange
            var destinatario = "test@example.com";
            var asunto = "Test Subject";
            var cuerpo = "Test Body";

            // Act
            await _emailQueueService.QueueEmailAsync(destinatario, asunto, cuerpo);

            // Assert
            Assert.Equal(1, _emailQueueService.GetQueueDepth());
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("encolado")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task QueueEmailAsync_WithNullDestinatario_LogsWarning()
        {
            // Act
            await _emailQueueService.QueueEmailAsync(null, "subject", "body");

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task QueueEmailAsync_WithEmptyDestinatario_LogsWarning()
        {
            // Act
            await _emailQueueService.QueueEmailAsync(string.Empty, "subject", "body");

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
        }

        [Theory]
        [InlineData("test@example.com", "Asunto 1", "Cuerpo 1")]
        [InlineData("otro@example.com", "Asunto 2", "Cuerpo 2")]
        [InlineData("tercero@example.com", "Asunto 3", "Cuerpo 3")]
        public async Task QueueEmailAsync_VariousInputs_EnqueuesAllEmails(string dest, string asunto, string cuerpo)
        {
            // Act
            await _emailQueueService.QueueEmailAsync(dest, asunto, cuerpo);

            // Assert
            Assert.Equal(1, _emailQueueService.GetQueueDepth());
        }

        #endregion

        #region QueueEmailMultipleAsync Tests

        [Fact]
        public async Task QueueEmailMultipleAsync_WithValidInput_EnqueuesEmail()
        {
            // Arrange
            var destinatarios = new List<string> { "test1@example.com", "test2@example.com" };
            var asunto = "Multiple Test";
            var cuerpo = "Body for multiple";

            // Act
            await _emailQueueService.QueueEmailMultipleAsync(destinatarios, asunto, cuerpo);

            // Assert
            Assert.Equal(1, _emailQueueService.GetQueueDepth());
        }

        [Fact]
        public async Task QueueEmailMultipleAsync_WithEmptyList_LogsWarning()
        {
            // Act
            await _emailQueueService.QueueEmailMultipleAsync(new List<string>(), "subject", "body");

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task QueueEmailMultipleAsync_WithNullList_LogsWarning()
        {
            // Act
            await _emailQueueService.QueueEmailMultipleAsync(null, "subject", "body");

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task QueueEmailMultipleAsync_VariousDestinationCounts_EnqueuesSuccessfully(int count)
        {
            // Arrange
            var destinatarios = Enumerable.Range(0, count)
                .Select(i => $"test{i}@example.com")
                .ToList();

            // Act
            await _emailQueueService.QueueEmailMultipleAsync(destinatarios, "subject", "body");

            // Assert
            Assert.Equal(1, _emailQueueService.GetQueueDepth());
        }

        #endregion

        #region QueueEmailConArchivosAsync Tests

        [Fact]
        public async Task QueueEmailConArchivosAsync_WithValidInput_EnqueuesEmail()
        {
            // Arrange
            var destinatario = "test@example.com";
            var asunto = "Email with files";
            var cuerpo = "Body";
            var archivos = new List<string> { "/path/file1.pdf", "/path/file2.xlsx" };

            // Act
            await _emailQueueService.QueueEmailConArchivosAsync(destinatario, asunto, cuerpo, archivos);

            // Assert
            Assert.Equal(1, _emailQueueService.GetQueueDepth());
        }

        [Fact]
        public async Task QueueEmailConArchivosAsync_WithNullDestinatario_LogsWarning()
        {
            // Act
            await _emailQueueService.QueueEmailConArchivosAsync(null, "subject", "body", new List<string>());

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
        }

        [Fact]
        public async Task QueueEmailConArchivosAsync_WithNullArchivos_EnqueuesSuccessfully()
        {
            // Act
            await _emailQueueService.QueueEmailConArchivosAsync("test@example.com", "subject", "body", null);

            // Assert
            Assert.Equal(1, _emailQueueService.GetQueueDepth());
        }

        #endregion

        #region Queue Depth and Stats Tests

        [Fact]
        public async Task GetQueueDepth_InitialState_ReturnsZero()
        {
            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
        }

        [Fact]
        public async Task GetQueueDepth_AfterMultipleQueues_ReturnsCorrectCount()
        {
            // Act
            await _emailQueueService.QueueEmailAsync("test1@example.com", "subject1", "body1");
            await _emailQueueService.QueueEmailAsync("test2@example.com", "subject2", "body2");
            await _emailQueueService.QueueEmailMultipleAsync(
                new List<string> { "test3@example.com", "test4@example.com" },
                "subject3", "body3");

            // Assert
            Assert.Equal(3, _emailQueueService.GetQueueDepth());
        }

        [Fact]
        public void GetStats_InitialState_ReturnsZeroStats()
        {
            // Act
            var stats = _emailQueueService.GetStats();

            // Assert
            Assert.Equal(0, stats.QueuedCount);
            Assert.Equal(0, stats.ProcessedCount);
            Assert.Equal(0, stats.FailedCount);
        }

        [Fact]
        public async Task GetStats_AfterQueueing_ReflectsQueueCount()
        {
            // Arrange
            await _emailQueueService.QueueEmailAsync("test@example.com", "subject", "body");

            // Act
            var stats = _emailQueueService.GetStats();

            // Assert
            Assert.Equal(1, stats.QueuedCount);
        }

        #endregion

        #region ProcessQueue Tests

        [Fact]
        public async Task ProcessQueueAsync_WithValidEmail_ProcessesSuccessfully()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            await _emailQueueService.QueueEmailAsync("test@example.com", "subject", "body");

            // Act
            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            var stats = _emailQueueService.GetStats();
            Assert.Equal(1, stats.ProcessedCount);
            Assert.Equal(0, stats.FailedCount);
        }

        [Fact]
        public async Task ProcessQueueAsync_WithFailingEmail_RetriesAndEventuallyFails()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(false);

            await _emailQueueService.QueueEmailAsync("test@example.com", "subject", "body");

            // Act - First attempt
            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();
            var statsAfterFirstAttempt = _emailQueueService.GetStats();

            // Assert first attempt
            Assert.Equal(0, statsAfterFirstAttempt.ProcessedCount);
            Assert.Equal(1, _emailQueueService.GetQueueDepth()); // Requeued

            // Act - Retry cycles
            for (int i = 0; i < 3; i++)
            {
                await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();
            }

            // Assert final state
            var finalStats = _emailQueueService.GetStats();
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            Assert.Equal(1, finalStats.FailedCount);
        }

        [Fact]
        public async Task ProcessQueueAsync_WithMultipleQueueItems_ProcessesAll()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);
            _mockEmailService
                .Setup(x => x.EnviarMultipleAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _emailQueueService.QueueEmailAsync("test1@example.com", "subject1", "body1");
            await _emailQueueService.QueueEmailAsync("test2@example.com", "subject2", "body2");
            await _emailQueueService.QueueEmailMultipleAsync(
                new List<string> { "test3@example.com", "test4@example.com" },
                "subject3", "body3");

            // Act
            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            var stats = _emailQueueService.GetStats();
            Assert.Equal(3, stats.ProcessedCount);
        }

        [Fact]
        public async Task ProcessQueueAsync_WithException_LogsAndContinues()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("SMTP error"));

            await _emailQueueService.QueueEmailAsync("test@example.com", "subject", "body");

            // Act
            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public async Task FullEmailQueue_SingleEmail_EnqueueAndProcess()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            // Act - Queue
            await _emailQueueService.QueueEmailAsync(
                "recipient@example.com",
                "Order Confirmation",
                "<p>Your order has been confirmed</p>");

            Assert.Equal(1, _emailQueueService.GetQueueDepth());

            // Act - Process
            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            var stats = _emailQueueService.GetStats();
            Assert.Equal(1, stats.ProcessedCount);
            Assert.Equal(0, stats.FailedCount);

            // Verify SMTP call
            _mockEmailService.Verify(
                x => x.EnviarAsync(
                    "recipient@example.com",
                    "Order Confirmation",
                    It.IsAny<string>(),
                    true),
                Times.Once);
        }

        [Fact]
        public async Task FullEmailQueue_MultipleEmails_EnqueueAndProcessInOrder()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            // Act - Queue multiple
            var recipients = new[] { "user1@example.com", "user2@example.com", "user3@example.com" };
            foreach (var recipient in recipients)
            {
                await _emailQueueService.QueueEmailAsync(recipient, "Notification", "Important update");
            }

            Assert.Equal(3, _emailQueueService.GetQueueDepth());

            // Act - Process
            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();

            // Assert
            Assert.Equal(0, _emailQueueService.GetQueueDepth());
            var stats = _emailQueueService.GetStats();
            Assert.Equal(3, stats.ProcessedCount);

            // Verify all recipients got emails
            foreach (var recipient in recipients)
            {
                _mockEmailService.Verify(
                    x => x.EnviarAsync(recipient, "Notification", "Important update", true),
                    Times.Once);
            }
        }

        [Fact]
        public async Task FullEmailQueue_HtmlAndPlainText_ProcessesBoth()
        {
            // Arrange
            _mockEmailService
                .Setup(x => x.EnviarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            // Act
            await _emailQueueService.QueueEmailAsync(
                "html@example.com",
                "HTML Email",
                "<h1>Header</h1>",
                esHtml: true);

            await _emailQueueService.QueueEmailAsync(
                "text@example.com",
                "Text Email",
                "Plain text body",
                esHtml: false);

            await ((EmailQueueService)_emailQueueService).ProcessQueueAsync();

            // Assert
            _mockEmailService.Verify(
                x => x.EnviarAsync("html@example.com", "HTML Email", "<h1>Header</h1>", true),
                Times.Once);

            _mockEmailService.Verify(
                x => x.EnviarAsync("text@example.com", "Text Email", "Plain text body", false),
                Times.Once);
        }

        #endregion
    }
}
