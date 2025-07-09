using System.Diagnostics;
using System.Net;
using Bede.AutomationTest.Tests.Client;
using Refit;

namespace Bede.AutomationTest.Tests;

public class BookTests
{
    private static Process serverProcess;

    private IBookApi bookApi = RestService.For<IBookApi>("http://localhost:9000");

    [OneTimeSetUp]
    public static void Init()
    {
        // var testDirectory = TestContext.CurrentContext.TestDirectory;
        serverProcess = Process.Start("../../../../LibraryManager.exe");
    }

    [OneTimeTearDown]
    public static void Cleanup()
    {
        serverProcess.Kill();
        serverProcess.Dispose();
    }

    [TestCase]
    public async Task GetBooks_GetBooksWithTitleExceedingValidLength_ReturnsEmpty()
    {
        //Given
        string longTitle = "Horus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus RisinggggHorus Risingggg";
        //When
        var actualBooks = await this.bookApi.GetBooks(longTitle);
        //Then
        Assert.That(actualBooks, Is.Empty);

    }

    [TestCase]
    public async Task GetBooks_Get2InvalidBooks_ReturnsEmpty()
    {

        //Given
        Book expectedBook1 = new()
        {
            Id = 26,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };

        Book expectedBook2 = new()
        {
            Id = 27,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        //When
        var actualBooks = await this.bookApi.GetBooks("Horus Risingggg");
        //Then
        Assert.That(actualBooks, Is.Empty);
    }

    //Test that creates 3 books and after that gets the 3 created books
    [TestCase]
    public async Task GetBooks_ThreeValidBooks_ReturnsCorrectCount()
    {
        //Given
        Book expectedBook1 = new()
        {
            Id = 1,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising 1"
        };

        Book expectedBook2 = new()
        {
            Id = 2,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising 1"
        };

        Book expectedBook3 = new()
        {
            Id = 3,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising 1"
        };

        await this.bookApi.CreateBook(expectedBook1);
        await this.bookApi.CreateBook(expectedBook2);
        await this.bookApi.CreateBook(expectedBook3);
        //When
        var actualBooks = await this.bookApi.GetBooks("Horus Rising 1");
        //Then
        Assert.That(actualBooks, Is.Not.Null);
        Assert.That(actualBooks, Has.Length.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(actualBooks[0].Title, Is.EqualTo(expectedBook1.Title));
            Assert.That(actualBooks[1].Title, Is.EqualTo(expectedBook2.Title));
            Assert.That(actualBooks[2].Title, Is.EqualTo(expectedBook3.Title));
        });

    }

    //Test case that confirms that you cannot get a book without a title
    [TestCase]
    public void GetBook_ValidBook_ReturnsBadRequestWithoutTitle()
    {
        //Given
        Book expectedBook1 = new()
        {
            Id = 4,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",

        };
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.CreateBook(expectedBook1));
        //Then
        Assert.That(apiException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        });

    }

    //THIS IS A BUG
    //It should return the name of the author but instead it returns null.
    [TestCaseAttribute]
    public async Task GetBook_ValidBook_ReturnsCorrectAuthor()//MethodName_StateUnderTest_ExpectedBehavior
    {
        //Given
        Book expectedBook = new()
        {
            Id = 7,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(expectedBook);
        //When
        var actualBook = await this.bookApi.GetBook(7);
        //Then
        Assert.That(actualBook, Is.Not.Null);
        Assert.That(actualBook.Author, Is.EqualTo(expectedBook.Author));

    }
    //The aim of this test is to return a book with any description value
    [TestCase]
    public async Task GetBook_ValidBook_ReturnsNullDescription()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 8,
            Author = "Dan Abnett",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(expectedBook);
        //When
        var actualBook = await this.bookApi.GetBook(8);
        //Then
        Assert.That(actualBook, Is.Not.Null);
        Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));

    }

    [TestCase]
    public async Task GetBook_ValidBook_ReturnsEmptyStringDescription()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 30,
            Author = "Dan Abnett",
            Description = "",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(expectedBook);
        //When
        var actualBook = await this.bookApi.GetBook(30);
        //Then
        Assert.That(actualBook, Is.Not.Null);
        Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));

    }

    [TestCase]
    public async Task GetBook_ValidBook_ReturnsCorrectDescription()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 31,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(expectedBook);
        //When
        var actualBook = await this.bookApi.GetBook(31);
        //Then
        Assert.That(actualBook, Is.Not.Null);
        Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));

    }


    [TestCase]
    public async Task GetBook_ValidBook_ReturnsCorrectTitle()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 9,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(expectedBook);
        //When
        var actualBook = await this.bookApi.GetBook(9);
        //Then
        Assert.That(actualBook, Is.Not.Null);
        Assert.That(actualBook.Title, Is.EqualTo(expectedBook.Title));

    }

    [TestCase(-1)]
    [TestCase(0)]
    public void CreateBook_InvalidBookId_ReturnsBadRequest(int id)
    {
        //Given
        Book expectedBook = new()
        {
            Id = id,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.CreateBook(expectedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book.Id should be a positive integer!"));
        });
    }

    //This test case aims to confirm that a book wil null value cannot be created
    [TestCase]
    public void CreateBook_InvalidNullBook_ReturnsBadRequest()
    {
        //Given
        Book expectedBook = null!;
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.CreateBook(expectedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book should be provided!"));
        });
    }

    //This test case aims to confirm that a book wil null value cannot be created
    [TestCase]
    public void CreateBook_InvalidBookAuthorExceeds30chars_ReturnsBadRequest()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 10,
            Author = "ThisistotestiftheAuthorcanbelongerthan30chars",//45
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.CreateBook(expectedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book.Author should not exceed 30 characters!"));
        });
    }
    [TestCase]

    public void CreateBook_InvalidBookTitleExceeds100chars_ReturnsBadRequest()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 40,
            Author = "Dan Abnett",
            Description = "Space Wolves",
            Title = "Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising Horus Rising "
        };
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.CreateBook(expectedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book.Title should not exceed 100 characters!"));
        });
    }


    [TestCase]
    public async Task CreateBook_DuplicateId_ThrowsException()
    {
        //Given
        Book expectedBook = new()
        {
            Id = 11,
            Author = "ThisIsDuplicatedTest",
            Description = "ThisIsDuplicatedTest",
            Title = "ThisIsDuplicatedTest"
        };
        await this.bookApi.CreateBook(expectedBook);

        Book duplicateBook = new()
        {
            Id = 11,
            Author = "ThisIsDuplicatedTest",
            Description = "ThisIsDuplicatedTest",
            Title = "ThisIsDuplicatedTest"
        };
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.CreateBook(duplicateBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book with id 11 already exists!"));
        });
    }

    [TestCase]
    public async Task UpdateBook_CheckUpdatedAuthor_ReturnsUpdatedAuthor()
    {
        //Given
        Book originalBook = new()
        {
            Id = 12,
            Author = "Original Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = 12,
            Author = "Updated Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };

        //When
        var actualUpdatedBook = await this.bookApi.UpdateBook(12, updatedBook);
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(actualUpdatedBook, Is.Not.Null);
            Assert.That(actualUpdatedBook.Author, Is.EqualTo(updatedBook.Author));
        });
    }

    [TestCase(0)]
    [TestCase(-1)]
    public async Task UpdateBook_CheckUpdatedId_ReturnsBadRequest(int id)
    {
        //Given
        Book originalBook = new()
        {
            Id = id,
            Author = "Original Author",
            Description = "Original Description",
            Title = "Horus Rising"
        };
        //  await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Author = "Original Author",
            Description = "Original Description",
            Title = "Horus Rising"
        };

        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.UpdateBook(id, updatedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book.Id should be a positive integer!"));
        });
    }

    [TestCase]
    public async Task UpdateBook_CheckInvalidId_ReturnsBadRequest()
    {
        //Given
        Book originalBook = new()
        {
            Id = 13,
            Author = "Original Author",
            Description = "Original Description",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = 14,
            Author = "Original Author",
            Description = "Original Description",
            Title = "Horus Rising"
        };

        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.UpdateBook(13, updatedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book.Id cannot be updated"));
        });
    }


    [TestCase]
    public async Task UpdateBook_CheckUpdatedAuthorWithMoreThan30Chars_ReturnsBadRequest()
    {
        //Given
        Book originalBook = new()
        {
            Id = 15,
            Author = "Original Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = 15,
            Author = "Updated AuthorUpdated AuthorUpdated AuthorUpdated AuthorUpdated AuthorUpdated Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };

        //When
        var apiException = Assert.ThrowsAsync<ApiException>(async () => await this.bookApi.UpdateBook(15, updatedBook));
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(apiException.Content, Does.Contain("Book.Author should not exceed 30 characters!"));
        });
    }

    //The aim of this test is to check and confirm that an author with exactly 30 chars is created and updated
    [TestCase]
    public async Task UpdateBook_CheckUpdatedAuthorWithExactly30Chars_ReturnsCorrectAuthor()
    {
        //Given
        Book originalBook = new()
        {
            Id = 36,
            Author = "Original Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = 36,
            Author = "Updated AuthorUpdated AuthorU",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };

        //When
        var actualUpdatedBook = await this.bookApi.UpdateBook(36, updatedBook);
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(actualUpdatedBook, Is.Not.Null);
            Assert.That(actualUpdatedBook.Author, Is.EqualTo(updatedBook.Author));
        });
    }


    [TestCase(18)]
    public async Task UpdateBook_CheckUpdatedTitle_ReturnsCorrectUpdatedTitle(int id)
    {
        //Given
        Book originalBook = new()
        {
            Id = id,
            Author = "Original Author",
            Description = "Original Description",
            Title = "Original Title"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = id,
            Author = "Updated Author",
            Description = "Space Wolves Faction",
            Title = "Updated Title"
        };
        //When
        var actualUpdatedBook = await this.bookApi.UpdateBook(id, updatedBook);
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(actualUpdatedBook, Is.Not.Null);
            Assert.That(actualUpdatedBook.Title, Is.EqualTo(updatedBook.Title));
        });

    }

    //The aim of this test is to check and confirm that an author with exactly 30 chars is created and updated
    [TestCase]
    public async Task UpdateBook_CheckUpdatedTitleWithExactly100Chars_ReturnsCorrectTitle()
    {
        //Given
        Book originalBook = new()
        {
            Id = 16,
            Author = "Original Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = 16,
            Author = "Updated AuthorUpdated AuthorU",
            Description = "Space Wolves Faction",
            Title = "Horus RisingHorus RisingHorus RisingHorus RisingHorus RisingHorus RisingHorus RisingHorus RisingHor"
        };

        //When
        var actualUpdatedBook = await this.bookApi.UpdateBook(16, updatedBook);
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(actualUpdatedBook, Is.Not.Null);
            Assert.That(actualUpdatedBook.Title, Is.EqualTo(updatedBook.Title));
        });
    }




    //The aim of this test is to confirm that the updated description is returned correctly
    [TestCase]
    public async Task UpdateBook_CheckUpdatedDescription_ReturnUpdatedDescription()
    {
        //Given
        Book originalBook = new()
        {
            Id = 19,
            Author = "Original Author",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        Book updatedBook = new()
        {
            Id = 19,
            Author = "Updated Author",
            Description = " UpdatedDescription",
            Title = "Horus Rising"
        };

        //When
        var actualUpdatedBook = await this.bookApi.UpdateBook(19, updatedBook);
        //Then
        Assert.Multiple(() =>
        {
            Assert.That(actualUpdatedBook, Is.Not.Null);
            Assert.That(actualUpdatedBook.Description, Is.EqualTo(updatedBook.Description));
        });
    }

    //The test aims to confirm that when deleting a valid book it returns no content
    [TestCase]
    public async Task DeleteBook_ValidBook_ReturnsNoContent()
    {

        //Given
        Book originalBook = new()
        {
            Id = 23,
            Author = "Dan Abnett",
            Description = "Space Wolves Faction",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);

        //When
        await this.bookApi.DeleteBook(23);
        //Then

        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.GetBook(23));
        Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

    }

    //The aim of this test is to confirm that you get an error message when you try to delete an invalid book
    [TestCase]
    public void DeleteBook_InvalidBook_ReturnsErrorMessage()
    {

        //Given
        int id = 24;

        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.DeleteBook(id));
        //Then
        Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(apiException.Content, Does.Contain("Book with id 24 not found!"));

    }
    //The aim for this test is to create a book without a description, then delete it and then get bad request when trying to request it.
    [TestCase]
    public async Task DeleteBook_WithoutDescription_ReturnValidBook()
    {

        //Given
        Book originalBook = new()
        {
            Id = 25,
            Author = "Dan Abnett",
            Title = "Horus Rising"
        };
        await this.bookApi.CreateBook(originalBook);
        await this.bookApi.DeleteBook(25);
        //When
        var apiException = Assert.ThrowsAsync<ApiException>(() => this.bookApi.GetBook(25));
        //Then
        Assert.That(apiException.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}