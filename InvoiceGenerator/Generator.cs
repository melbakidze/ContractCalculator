using System;
using System.IO;
using Data.Entities;
using Data.Invoice;
using IOInteraction;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Invoices
{
    public class Generator
    {
        public string CreateWeeklyInvoice(InvoiceEntity invoiceEntity, WeeklyInvoiceDetails wid, ClientEntity client, CompanyInformationEntity companyInformationEntity)
        {
            DirectoryCreator.EnsureExistance(wid.InvoiceFolder);

            var pdfFileName = wid.InvoiceFolder + "\\Invoice-" + InvoiceNameGenerator.GetName(wid.Number, DateTime.Now) + ".pdf";

            var pdfDoc = new Document(PageSize.A4, 50, 50, 25, 25);
            var output = new FileStream(pdfFileName, FileMode.OpenOrCreate);
            PdfWriter.GetInstance(pdfDoc, output);

            pdfDoc.Open();

            HeaderFactory.Create(pdfDoc, wid.Number, invoiceEntity, companyInformationEntity);

            ClientInfoFactory.Create(pdfDoc, CompositeAddressCreator.CreateAddress(client.CompanyInformationEntity), wid.CommentsOrSpecialInstructions);

            CostSummaryFactory.CreateWeekly(pdfDoc, wid, invoiceEntity);
            FooterFactory.Create(pdfDoc, invoiceEntity);

            pdfDoc.Close();

            return pdfFileName;
        }

        public string CreateCustomInvoice(InvoiceEntity invoiceEntity, SimpleInvoiceDetails simpleInvoiceDetails, CompanyInformationEntity companyInformationEntity)
        {

            DirectoryCreator.EnsureExistance(simpleInvoiceDetails.InvoiceFolder);

            var pdfFileName = simpleInvoiceDetails.InvoiceFolder + "\\Invoice-" + InvoiceNameGenerator.GetName(simpleInvoiceDetails.Number, DateTime.Now) + ".pdf";

            var pdfDoc = new Document(PageSize.A4, 50, 50, 25, 25);
            var output = new FileStream(pdfFileName, FileMode.OpenOrCreate);
            PdfWriter.GetInstance(pdfDoc, output);

            pdfDoc.Open();

            HeaderFactory.Create(pdfDoc, simpleInvoiceDetails.Number, invoiceEntity, companyInformationEntity);

            ClientInfoFactory.Create(pdfDoc, CompositeAddressCreator.CreateAddress(simpleInvoiceDetails.Client.CompanyInformationEntity), simpleInvoiceDetails.CommentsOrSpecialInstructions);

            CostSummaryFactory.CreateCustom(pdfDoc, simpleInvoiceDetails, invoiceEntity);
            FooterFactory.Create(pdfDoc, invoiceEntity);

            pdfDoc.Close();

            return pdfFileName;
        }
    }
}
