using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.Entities;
using Microsoft.EntityFrameworkCore;

namespace FurnitureBuildingSolution.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
        User GetByEmailAddress(string emailAddress);
        User Add(User user);
        User Update(User user);
        User Delete(User user);
        bool CheckIfEmailAddressTaken(string emailAddress);
        User GetByRecoveryKey(string recoveryKey);
    }

    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        private IMapper _autoMapper;

        public UserRepository(DataContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }

        public User GetById(int id)
        {
            var fetchedDbUser = _context.Users.AsNoTracking().SingleOrDefault(dbUser => dbUser.Id == id);
            return fetchedDbUser == null ? null : _autoMapper.Map<User>(fetchedDbUser);
        }

        public IEnumerable<User> GetAll()
        {
            var dbUsers = _context.Users.AsNoTracking();
            return dbUsers.Select(u => _autoMapper.Map<User>(u)).ToList();
        }

        public User GetByEmailAddress(string emailAddress)
        {
            var fetchedDbUser = _context.Users.AsNoTracking().SingleOrDefault(dbUser => dbUser.EmailAddress == emailAddress);
            return fetchedDbUser == null ? null : _autoMapper.Map<User>(fetchedDbUser);
        }

        public User GetByRecoveryKey(string recoveryKey)
        {
            var fetchedDbUser = _context.Users.AsNoTracking().SingleOrDefault(dbUser => dbUser.PasswordRecoveryKey.ToString() == recoveryKey);
            return fetchedDbUser == null ? null : _autoMapper.Map<User>(fetchedDbUser);
        }

        public User Add(User user)
        {
            var dbUser = _autoMapper.Map<DbUser>(user);
            _context.Users.Add(dbUser);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            var dbUser = _autoMapper.Map<DbUser>(user);
            _context.Users.Update(dbUser);
            _context.SaveChanges();
            return user;
        }

        public User Delete(User user)
        {
            var dbUser = _autoMapper.Map<DbUser>(user);
            _context.Users.Remove(dbUser);
            _context.SaveChanges();
            return user;
        }

        public bool CheckIfEmailAddressTaken(string emailAddress)
        {
            return _context.Users.Any(x => x.EmailAddress == emailAddress);
        }
    }
}