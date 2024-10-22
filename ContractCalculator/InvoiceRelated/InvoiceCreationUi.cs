﻿using System.Collections.Generic;
using System.Linq;
using Data;
using Data.EntityWrappers.Smtp;
using EmailProvider;
using Invoices;

namespace UserInterface.InvoiceRelated
{
    public class InvoiceCreationUi
    {
        public static void CreateWeeklyInvoice(Repository repository)
        {
            var weeklyInvoiceDetails = WeeklyInvoiceDataReceiver.Get(repository.ClientsWrapper.Data);

            var pdfFilename = WeeklyInvoiceFactory.Create(repository.InvoiceWrapper.Data, weeklyInvoiceDetails, repository.CompanyInformationWrapper.Data);

            FileSavedNotifier.Notify(pdfFilename);

            FileVisualiser.VisualiseIfRequested(pdfFilename);

            InvoiceEmailer.EmailIfRequested(repository, pdfFilename, weeklyInvoiceDetails);
        }

        public static void CreatedInvoiceFromXml(Repository repository)
        {
            var client = ClientSelector.Get(repository.ClientsWrapper.Data);

            var pdfFilename = new Generator().CreateWeeklyInvoice(repository.InvoiceWrapper.Data, repository.InvoiceWrapper.Data.WeeklyInvoiceDetails, client, repository.CompanyInformationWrapper.Data);

            FileSavedNotifier.Notify(pdfFilename);

            FileVisualiser.VisualiseIfRequested(pdfFilename);

            InvoiceEmailer.EmailIfRequested(repository, pdfFilename, repository.InvoiceWrapper.Data.WeeklyInvoiceDetails);

            repository.InvoiceWrapper.Data.WeeklyInvoiceDetails.Number++;
            repository.InvoiceWrapper.Save();
        }

        public static void CreateCustomInvoiceFromInput(Repository repository)
        {
            var simpleInvoiceDetails = SimpleInvoiceDataReceiver.Get(repository.ClientsWrapper.Data);

            var pdfFilename = new Generator().CreateCustomInvoice(repository.InvoiceWrapper.Data, simpleInvoiceDetails, repository.CompanyInformationWrapper.Data);

            FileSavedNotifier.Notify(pdfFilename);

            FileVisualiser.VisualiseIfRequested(pdfFilename);

            InvoiceEmailer.EmailIfRequested(repository, pdfFilename, simpleInvoiceDetails);
        }

        public static void RunAutomatedWeeklyInvoice(Repository repository)
        {
            var client = repository.ClientsWrapper.Data.FirstOrDefault(x => x.Id == repository.InvoiceWrapper.Data.ClientId);

            var sender = new Sender(new DefaultSmtpWrapper().Data);
            
            if (client == null)
            {
                sender.Send("murados91@gmail.com", "Automated invoice failure", "No client detected with id: " + repository.InvoiceWrapper.Data.ClientId, new List<string>());
            }

            var pdfFilename = new Generator().CreateWeeklyInvoice(repository.InvoiceWrapper.Data, repository.InvoiceWrapper.Data.WeeklyInvoiceDetails, client, repository.CompanyInformationWrapper.Data);

            InvoiceEmailer.SendEmailWithAttachement(client, repository.EmailWrapper.Data, repository.InvoiceWrapper.Data.WeeklyInvoiceDetails, pdfFilename);

            sender.Send("murados91@gmail.com", "Automated invoice success", "Sent invoice with id: " + repository.InvoiceWrapper.Data.WeeklyInvoiceDetails.Number, new List<string>());
            
            repository.InvoiceWrapper.Data.WeeklyInvoiceDetails.Number++;
            repository.InvoiceWrapper.Save();
        }
    }
}