using System;
using Data.Entities;
using Data.Invoice;
using Invoices;

namespace UserInterface.InvoiceRelated
{
    public class WeeklyInvoiceFactory
    {
        public static string Create(InvoiceEntity invoiceEntity, WeeklyInvoiceDetails weeklyInvoiceDetails, CompanyInformationEntity companyInformationEntity)
        {
            var generator = new Generator();
            var filename = generator.CreateWeeklyInvoice(invoiceEntity, weeklyInvoiceDetails, weeklyInvoiceDetails.Client, companyInformationEntity);

            return filename;
        }
    }
}