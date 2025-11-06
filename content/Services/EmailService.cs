using AutoMapper;
using FurnitureBuildingSolution.Dtos;
using FurnitureBuildingSolution.Dtos.Order;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Reflection;

namespace FurnitureBuildingSolution.Services
{
    public interface IEmailService
    {
        Email EnqueueEmail(Email email);
        bool SendEmails(string apiKey);
    }

    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;
        private readonly IMapper _autoMapper;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSender _emailSender;

        public EmailService(IOptions<AppSettings> appSettings, IMapper autoMapper, IEmailRepository emailRepository, IEmailSender emailSender)
        {
            _appSettings = appSettings.Value;
            _autoMapper = autoMapper;
            _emailRepository = emailRepository;
            _emailSender = emailSender;
        }

        public Email EnqueueEmail(Email email)
        {
            return _emailRepository.Enqueue(email);
        }

        public bool SendEmails(string apiKey)
        {
            if (apiKey != _appSettings.EmailSettings.ApiKey)
            {
                throw new AccessViolationException("Api key is not valid for sending emails.");
            }
            var emailsToSend = _emailRepository.GetEmailsToSend(_appSettings.EmailSettings.MaxSendAttempts);
            var success = true;
            foreach (var email in emailsToSend)
            {
                var thisEmailSentSuccessfully = _emailSender.Send(email);
                if (thisEmailSentSuccessfully)
                {
                    _emailRepository.RegisterSent(email);
                }
                else
                {
                    _emailRepository.RegisterSendFailed(email);
                }
                success = success && thisEmailSentSuccessfully;
            }
            return success;
        }
    }
}
