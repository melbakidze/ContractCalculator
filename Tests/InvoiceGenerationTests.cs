using System.Diagnostics;
using System.Linq;
using Data.EntityWrappers;
using Data.EntityWrappers.Clients;
using Data.EntityWrappers.CompanyInformation;
using Data.EntityWrappers.WeeklyInvoice;
using Data.Invoice;
using Invoices;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class InvoiceGenerationTests
    {
        [Test]
        public void CreatePdf()
        {
            var wid = new WeeklyInvoiceDetails
            {
                Number = 1,
                HourlyRate = 8,
                ChargeableHours = 37.5,
                CommentsOrSpecialInstructions = "All good :)"
            };

            var client = new TestClientsWrappers();

            var pdfFielName = new Generator().CreateWeeklyInvoice(new TestInvoiceWrapper().Data, wid, client.Data.First(), new DefaultCompanyInformationWrapper().Data);

            if (true)
            {
                var startInfo = new ProcessStartInfo(pdfFielName)
                    {
                        WindowStyle = ProcessWindowStyle.Normal
                    };
                Process.Start(startInfo);
            }
        }
    }
}
