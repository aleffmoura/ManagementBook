namespace ManagementBook.Application.Notifications.Books;

using Amazon.S3.Transfer;
using ManagementBook.Application.Features.Utilities;
using ManagementBook.Domain.Books;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateBookCoverNotificationHandler : INotificationHandler<BookCoverNotification>
{
    private IBookRepository _bookRepository;
    private IUploadService _uploadService;

    public UpdateBookCoverNotificationHandler(IUploadService uploadService, IBookRepository bookRepository)
    {
        _uploadService = uploadService;
        _bookRepository = bookRepository;
    }

    public async Task Handle(BookCoverNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            await _uploadService.Send(new FileSend
            {
                BookId = notification.BookId,
                Data = notification.Data,
            }, async (o, e) =>
            {
                if (e is UploadProgressArgs { PercentDone: 100 } args)
                {
                    var book = await _bookRepository.GetById(notification.BookId);

                    book.IfSucc(async b => await _bookRepository.Update(b with { BookCoverUrl = args.FilePath }));
                }
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
