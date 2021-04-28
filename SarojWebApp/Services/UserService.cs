using Dapper;
using Microsoft.Extensions.Configuration;
using SarojWebApp.Models;
using SarojWebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SarojWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private const string Connectionstring = "DefaultConnection";
        private const string LoginQuery = "SELECT Id,UserName,FirstName,LastName FROM [User] WHERE UserName=@UserName AND Password=@Password";
        private const string InsNote = @"INSERT INTO Notes (UserId,Title,Note)Values(@userId,@title,@note)";
        private const string UpdateNoteQ = @"UPDATE NOTE 
SET
Title=@title,
Note=@note
WHERE Id=@id";
        private const string DeleteNoteQ = "Delete Note Where Id=@id";
        private const string GetNote = "select Id,UserId,Note,Title from Notes Where UserId=@userId";
        public UserService(IConfiguration config)
        {
            _config = config;
        }
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }


        public async Task<User> Login(string username, string password)
        {
            try
            {
                using (var connection = GetDbconnection())
                {
                    var user = (await connection.QueryAsync<User>(LoginQuery, new { username, password })).FirstOrDefault();
                    return user;

                }
            }
            catch(Exception ex)
            {

            }
            return null;
        }


        public async Task<List<UserNote>> GetAllNotes(int userId)
        {
            using (var connection = GetDbconnection())
            {
                return (await connection.QueryAsync<UserNote>(GetNote, new { userId })).ToList();

            }
        }

        public async Task<UserNote> UpdateNote(UserNote note)
        {
            using (var connection = GetDbconnection())
            {
                await connection.ExecuteAsync(UpdateNoteQ, new { note });
                return note;

            }
        }

        public async Task<UserNote> CreateNote(UserNote note)
        {
            using (var connection = GetDbconnection())
            {
                await connection.ExecuteAsync(InsNote, new { note });
                return note;

            }
        }

        public async Task DeleteNote(int noteId)
        {
            using (var connection = GetDbconnection())
            {
                await connection.ExecuteAsync(DeleteNoteQ, new { noteId });

            }
        }
    }
}
