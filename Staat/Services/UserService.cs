using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Helpers;
using Staat.Models.Users;

namespace Staat.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        IEnumerable<User> GetAll();
        User Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);
        User GetById(int userId);
    }
    public class UserService : IUserService
    {
        //private ApplicationDbContext _context;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; 

        public UserService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            //ApplicationDbContext context, 
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            //_context = context;
        }

        public User Authenticate(string email, string password)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.User.SingleOrDefault(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }
        public IEnumerable<User> GetAll()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return _context.User;
        }

        public User GetById(int id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return _context.User.Find(id);
        }

        public User Create(User user, string password)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.User.Any(x => x.Email == user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            var user = _context.User.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new username is already taken
                if (_context.User.Any(x => x.Email == userParam.Email))
                    throw new AppException("Email " + userParam.Email + " is already taken");

                user.Email = userParam.Email;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            Console.WriteLine(storedSalt.Length);
            Console.WriteLine(storedHash.Length);
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}