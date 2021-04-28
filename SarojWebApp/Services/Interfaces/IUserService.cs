using SarojWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarojWebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Login(string username, string password);
        Task<List<UserNote>> GetAllNotes(int userId);
        Task<UserNote> UpdateNote(UserNote note);
        Task<UserNote> CreateNote(UserNote note);
        Task DeleteNote(int noteId);
    }
}
