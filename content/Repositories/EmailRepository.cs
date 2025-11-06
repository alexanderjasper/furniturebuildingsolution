using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;

namespace FurnitureBuildingSolution.Repositories
{
    public interface IEmailRepository
    {
        Email Enqueue(Email email);
        List<Email> GetEmailsToSend(int maxSendAttempts);
        void RegisterSent(Email email);
        void RegisterSendFailed(Email email);
    }

    public class EmailRepository : IEmailRepository
    {
        private DataContext _context;
        private IMapper _autoMapper;

        public EmailRepository(DataContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }

        public Email Enqueue(Email email)
        {
            try
            {
                var emailToEnqueue = _autoMapper.Map<DbEmail>(email);
                var enqueuedEmail = _context.Emails.Add(emailToEnqueue).Entity;
                _context.SaveChanges();
                return _autoMapper.Map<Email>(enqueuedEmail);
            }
            catch (Exception exception)
            {
                throw new AppException("Could not enqueue email to database", exception);
            }
        }

        public List<Email> GetEmailsToSend(int maxSendAttempts)
        {
            return _context.Emails
                .Where(email => email.Sent == null && email.FailedSendAttempts < maxSendAttempts)
                .Select(email => _autoMapper.Map<Email>(email))
                .ToList();
        }

        public void RegisterSent(Email email)
        {
            var emailFromDb = _context.Emails.Single(dbEmail => dbEmail.Id == email.Id);
            emailFromDb.Sent = DateTime.Now;
            _context.Emails.Update(emailFromDb);
            _context.SaveChanges();
        }

        public void RegisterSendFailed(Email email)
        {
            var emailFromDb = _context.Emails.Single(dbEmail => dbEmail.Id == email.Id);
            emailFromDb.FailedSendAttempts += 1;
            _context.Emails.Update(emailFromDb);
            _context.SaveChanges();
        }
    }
}