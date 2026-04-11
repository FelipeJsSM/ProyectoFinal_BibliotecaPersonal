using SQLite;
using ProyectoFinal_BibliotecaPersonal.Models;

namespace ProyectoFinal_BibliotecaPersonal.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "biblioteca.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Book>().Wait();
        }

        public async Task<int> SaveBookAsync(Book book)
        {
            if (book.Id == 0)
                return await _database.InsertAsync(book);
            else
                return await _database.UpdateAsync(book);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _database.Table<Book>().ToListAsync();
        }

        public async Task<List<Book>> GetBooksByGenreAsync(string genre)
        {
            return await _database.Table<Book>()
                .Where(b => b.Genre == genre)
                .ToListAsync();
        }

        public async Task<List<Book>> GetReadBooksAsync()
        {
            return await _database.Table<Book>()
                .Where(b => b.IsRead)
                .ToListAsync();
        }

        public async Task<List<Book>> GetUnreadBooksAsync()
        {
            return await _database.Table<Book>()
                .Where(b => !b.IsRead)
                .ToListAsync();
        }

        public async Task<int> DeleteBookAsync(Book book)
        {
            return await _database.DeleteAsync(book);
        }

        public async Task<int> UpdateReadStatusAsync(Book book, bool isRead)
        {
            book.IsRead = isRead;
            return await _database.UpdateAsync(book);
        }

        public async Task<(int Total, int Read, int Unread)> GetStatisticsAsync()
        {
            var total = await _database.Table<Book>().CountAsync();
            var read = await _database.Table<Book>().CountAsync(b => b.IsRead);
            var unread = total - read;
            return (total, read, unread);
        }
    }
}