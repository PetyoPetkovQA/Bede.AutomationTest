using Refit;

namespace Bede.AutomationTest.Tests.Client;

public interface IBookApi
{
    [Get("/api/books")]
    Task<Book[]> GetBooks([Query] string? title = null);

    [Get("/api/books/{id}")]
    Task<Book> GetBook(int id);

    [Post("/api/books")]
    Task<Book> CreateBook([Body] Book book);

    [Put("/api/books/{id}")]
    Task<Book> UpdateBook(int id,[Body] Book book);

    [Delete("/api/books/{id}")]
    Task DeleteBook(int id);

}
