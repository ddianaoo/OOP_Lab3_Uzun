using ConsoleApp1;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Введіть максимальну кількість книг (N > 0): ");
int N;
while (!int.TryParse(Console.ReadLine(), out N) || N <= 0)
{
    Console.Write("Неправильне число. Введіть ще раз (N > 0): ");
}

List<Book> books = new List<Book>();

while (true)
{
    Console.WriteLine("\n=== МЕНЮ ===");
    Console.WriteLine("1 - Додати об'єкт");
    Console.WriteLine("2 - Переглянути всі обʼєкти");
    Console.WriteLine("3 - Знайти обʼєкт");
    Console.WriteLine("4 - Продемонструвати поведінку");
    Console.WriteLine("5 - Видалити обʼєкт");
    Console.WriteLine("0 - Вийти з програми");
    Console.Write("Виберіть опцію: ");

    string choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            if (books.Count >= N)
            {
                Console.WriteLine("Досягнуто максимальної кількості книг.");
                break;
            }

            Console.WriteLine("Оберіть варіант створення книги:");
            Console.WriteLine("1 - Короткий ввід (назва, автор, жанр)");
            Console.WriteLine("2 - Повний ввід (усі поля)");
            Console.Write("Ваш вибір: ");
            string inputMode = Console.ReadLine();

            Book newBook;

            if (inputMode == "1")
            {
                Console.WriteLine("Використано конструктор з 3 параметрами (title, author, genre).");
                newBook = CreateBookFromConsole(false);
            }
            else if (inputMode == "2")
            {
                Console.WriteLine("Використано конструктор з повним набором параметрів.");
                newBook = CreateBookFromConsole();
            }
            else
            {
                Console.WriteLine("Використано конструктор без параметрів.");
                newBook = new Book();
            }


            books.Add(newBook);
            Console.WriteLine("Книгу успішно додано!");
            break;

        case "2":
            Console.WriteLine("=== Усі книги ===");
            printBooks(books);
            break;

        case "3":
            string choiceSearch;
            Console.WriteLine("Оберіть характеристику для пошуку:");
            do
            {
                Console.WriteLine("1 - Назва");
                Console.WriteLine("2 - Автор");
                Console.Write("Ваш вибір: ");
                choiceSearch = Console.ReadLine();
                if (choiceSearch != "1" && choiceSearch != "2")
                {
                    Console.WriteLine("Неправильний вибір.");
                }
            } while (choiceSearch != "1" && choiceSearch != "2");

            Console.Write("Введіть значення для пошуку: ");
            string searchValue = Console.ReadLine();

            List<Book> foundBooks = new List<Book>();

            if (choiceSearch == "1")
            {
                foundBooks = books
                    .Where(b => b.Title.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else if (choiceSearch == "2")
            {
                foundBooks = books
                    .Where(b => b.Author.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Console.WriteLine("\n=== Результати пошуку ===");
            printBooks(foundBooks);
            break;

        case "4":
            Console.Write("Введіть порядковий номер книги: ");
            int indexBook;
            while (!int.TryParse(Console.ReadLine(), out indexBook) || indexBook <= 0 || indexBook > books.Count)
            {
                Console.Write("Неправильний номер. Введіть ще раз: ");
            }
            var bookToModify = books[indexBook - 1];

            Console.WriteLine("\n=== Поточна книга ===");
            printBooks([bookToModify]);

            Console.WriteLine("1 - Змінити ціну");
            Console.WriteLine("2 - Збільшити кількість");
            Console.WriteLine("3 - Зменшити кількість");
            Console.WriteLine("4 - Порівняти з іншою книгою");
            Console.Write("Виберіть опцію: ");
            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    Console.Write("Введіть нову ціну: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                        bookToModify.ChangePrice(newPrice);
                    else
                        Console.WriteLine("Неправильна ціна.");
                    break;
                case "2":
                    bookToModify.IncreaseQuantity();
                    break;
                case "3":
                    bookToModify.DecreaseQuantity();
                    break;
                case "4":
                    Console.Write("Введіть порядковий номер книги: ");
                    int indexBook3;
                    while (!int.TryParse(Console.ReadLine(), out indexBook3) || indexBook3 <= 0 || indexBook3 > books.Count)
                    {
                        Console.Write("Неправильний номер. Введіть ще раз: ");
                    }
                    var bookToCompare = books[indexBook3 - 1];

                    Console.WriteLine("Порівняти за:");
                    Console.WriteLine("1 - Датою створення");
                    Console.WriteLine("2 - Кількістю у наявності");
                    Console.WriteLine("3 - Ціною");
                    Console.Write("Виберіть опцію: ");
                    string action3 = Console.ReadLine();

                    switch (action3)
                    {
                        case "1":
                            {
                                int result = bookToModify.CompareTo(bookToCompare.CreationDate);
                                if (result < 0)
                                    Console.WriteLine($"{bookToModify.Title} вийшла раніше за {bookToCompare.Title}");
                                else if (result > 0)
                                    Console.WriteLine($"{bookToModify.Title} вийшла пізніше за {bookToCompare.Title}");
                                else
                                    Console.WriteLine($"{bookToModify.Title} та {bookToCompare.Title} вийшли в один день");
                                break;
                            }

                        case "2":
                            {
                                int result = bookToModify.CompareTo(bookToCompare.Quantity);
                                if (result < 0)
                                    Console.WriteLine($"{bookToModify.Title} має менше примірників, ніж {bookToCompare.Title}");
                                else if (result > 0)
                                    Console.WriteLine($"{bookToModify.Title} має більше примірників, ніж {bookToCompare.Title}");
                                else
                                    Console.WriteLine($"{bookToModify.Title} та {bookToCompare.Title} мають однакову кількість примірників");
                                break;
                            }

                        case "3":
                            {
                                int result = bookToModify.CompareTo(bookToCompare.Price);
                                if (result < 0)
                                    Console.WriteLine($"{bookToModify.Title} дешевша за {bookToCompare.Title}");
                                else if (result > 0)
                                    Console.WriteLine($"{bookToModify.Title} дорожча за {bookToCompare.Title}");
                                else
                                    Console.WriteLine($"{bookToModify.Title} та {bookToCompare.Title} мають однакову ціну");
                                break;
                            }

                        default:
                            Console.WriteLine("Неправильна опція.");
                            break;
                    }

                    break;
                default:
                    Console.WriteLine("Неправильна опція.");
                    break;
            }
            break;

        case "5":
            string choiceDelete;
            Console.WriteLine("Оберіть характеристику для видалення: ");
            do
            {
                Console.WriteLine("1 - Порядковий номер");
                Console.WriteLine("2 - Назва");
                Console.Write("Ваш вибір: ");
                choiceDelete = Console.ReadLine();
                if (choiceDelete != "1" && choiceDelete != "2")
                {
                    Console.WriteLine("Неправильний вибір.");
                }
            } while (choiceDelete != "1" && choiceDelete != "2");

            if (choiceDelete == "1")
            {
                printBooks(books);
                int index;
                Console.Write("Введіть порядковий номер книги для видалення: ");
                while (!int.TryParse(Console.ReadLine(), out index) || index <= 0 || index > books.Count)
                {
                    Console.Write("Неправильний номер. Введіть ще раз: ");
                }
                books.RemoveAt(index - 1);
                Console.WriteLine("Книгу видалено.");
            }
            else
            {
                Console.Write("Введіть значення для видалення: ");
                string deleteValue = Console.ReadLine();
                int removed = books.RemoveAll(b => b.Title.Equals(deleteValue, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine(removed > 0 ? $"Видалено {removed} книг(и)." : "Книг не знайдено.");
            }
            break;

        case "0":
            Console.WriteLine("Вихід з програми...");
            return;

        default:
            Console.WriteLine("Неправильний вибір.");
            break;
    }
}


Book CreateBookFromConsole(Boolean full=true)
{
    var book = new Book();

    while (true)
    {
        try
        {
            Console.Write("Введіть назву книги: ");
            book.Title = Console.ReadLine();
            break;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    while (true)
    {
        try
        {
            Console.Write("Введіть автора (Прізвище Ім’я): ");
            book.Author = Console.ReadLine();
            break;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    if (full)
    {
        while (true)
        {
            try
            {
                Console.Write("Введіть дату створення (yyyy-mm-dd): ");
                string input = Console.ReadLine();
                if (!DateOnly.TryParse(input, out DateOnly date))
                {
                    Console.WriteLine("Невірний формат дати.");
                    continue;
                }
                book.CreationDate = date;
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        while (true)
        {
            try
            {
                Console.Write("Введіть кількість сторінок: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int pages))
                {
                    Console.WriteLine("Невірне число.");
                    continue;
                }
                book.CountOfPages = pages;
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        while (true)
        {
            try
            {
                Console.Write("Введіть ціну: ");
                string input = Console.ReadLine();
                if (!decimal.TryParse(input, out decimal price))
                {
                    Console.WriteLine("Невірна ціна.");
                    continue;
                }
                book.ChangePrice(price);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        while (true)
        {
            try
            {
                Console.Write("Введіть кількість: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int quantity))
                {
                    Console.WriteLine("Невірне число.");
                    continue;
                }
                book.ChangeQuantity(quantity);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }

    while (true)
    {
        try
        {
            Console.WriteLine("Оберіть жанр:");
            foreach (var g in Enum.GetValues(typeof(BookGenre)))
            {
                Console.WriteLine($"{(int)g} - {g}");
            }

            string input = Console.ReadLine();
            if (!int.TryParse(input, out int genreNumber) || !Enum.IsDefined(typeof(BookGenre), genreNumber))
            {
                Console.WriteLine("Невірний вибір жанру.");
                continue;
            }
            book.Genre = (BookGenre)genreNumber;
            break;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    Console.Write("Введіть опис книги (необов’язково): ");
    book.Description = Console.ReadLine();

    return book;
}

void printBooks(List<Book> books)
{
    if (books == null || books.Count == 0)
    {
        Console.WriteLine("<cписок порожній>");
        return;
    }

    Console.WriteLine("№ | Назва                | Автор               | Дата       | Стор. |    Ціна (USD)  | К-сть | Жанр      | Опис");
    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");

    int index = 1;
    foreach (var book in books)
    {
        Console.WriteLine($"{index,2} | {book}");
        index++;
    }

    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
}