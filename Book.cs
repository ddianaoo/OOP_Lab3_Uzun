using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Book
    {
        private string? title;
        private string? author;
        private DateOnly creationDate;
        private decimal price;
        private int quantity;
        private BookGenre genre;
        private int countOfPages;

        public string Description { get; set; } = "nice book";
        public int CountOfPages
        {
            get => countOfPages;
            set
            {
                if (value <= 0 || value > 10000)
                    throw new ArgumentException("Кількість сторінок має бути в межах 1–10000.");
                countOfPages = value;
            }
        }
        public string Title
        {
            get => title;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    title = value;
                else
                    throw new ArgumentException("Назва книги не може бути порожньою.");
            }
        }
        public string Author
        {
            get => author;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Автор не може бути порожнім.");
                if (!Regex.IsMatch(value.Trim(), @"^[A-ZА-ЯЇІЄҐ][a-zа-яїієґ]* [A-ZА-ЯЇІЄҐ][a-zа-яїієґ]*$"))
                    throw new ArgumentException("Автор повинен бути у форматі 'Прізвище Ім’я', лише букви, великі перші літери.");

                author = value.Trim();
            }
        }
        public DateOnly CreationDate
        {
            get => creationDate;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Дата створення не може бути пізніше сьогоднішньої.");

                creationDate = value;
            }
        }
        public decimal Price
        {
            get => price;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Ціна має бути більше 0.");
                price = value;
            }
        }
        public int Quantity
        {
            get => quantity;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Кількість не може бути від’ємною.");

                quantity = value;
            }
        }
        public BookGenre Genre
        {
            get => genre;
            set
            {
                if (!Enum.IsDefined(typeof(BookGenre), value))
                    throw new ArgumentException("Невірне значення жанру книги.");
                genre = value;
            }
        }
        public decimal FullCost => quantity * price;

        public decimal ChangePrice(decimal newPrice)
        {
            Price = newPrice;
            return Price;
        }

        public int IncreaseQuantity()
        {
            Quantity++;
            return Quantity;
        }

        public int ChangeQuantity(int quantity)
        {
            Quantity = quantity;
            return Quantity;
        }

        public int DecreaseQuantity()
        {
            if (Quantity > 0)
                Quantity--;
            return Quantity;
        }

        public int CompareTo(Book? other)
        {
            if (other == null) return 1;
            return this.creationDate.CompareTo(other.creationDate);
        }

        public override string ToString()
        {
            string desc = Description.Length > 20 ? Description.Substring(0, 17) + "..." : Description;

            return $"{title,-20} | {author,-18} | {creationDate:dd.MM.yyyy} | {CountOfPages,5} | {price,8} USD | {quantity,5} | {genre,-10} | {desc,-30}";
        }
    }
}