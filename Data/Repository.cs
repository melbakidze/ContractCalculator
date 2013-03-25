using Data.EntityWrappers;
using Data.EntityWrappers.Clients;
using Data.EntityWrappers.CompanyInformation;
using Data.EntityWrappers.Email;
using Data.EntityWrappers.Smtp;
using Data.EntityWrappers.Terms;
using Data.EntityWrappers.WeeklyInvoice;
using IOInteraction;
using Serialisation;

namespace Data
{
    public class Repository
    {
        public const string Folder = "C:\\Hardcore Software\\ISec\\";

        public readonly ISmtpWrapper SmtpWrapper;
        public readonly IEmailWrapper EmailWrapper;
        public readonly IInvoiceWrapper InvoiceWrapper;
        public readonly IClientsWrapper ClientsWrapper;
        public readonly ICompanyInformationWrapper CompanyInformationWrapper;
        public readonly ITermsWrapper TermsWrapper;

        public Repository()
        {
            SmtpWrapper = new DefaultSmtpWrapper();
            EmailWrapper = new DefaultEmailWrapper();
            InvoiceWrapper = new DefaultInvoiceWrapper();
            ClientsWrapper = new DefaultClientsWrappers();
            CompanyInformationWrapper = new DefaultCompanyInformationWrapper();
            TermsWrapper = new DefaultTermsWrapper();

            EnsureConfigsExist();

            SmtpWrapper.Load();
            EmailWrapper.Load();
            InvoiceWrapper.Load();
            ClientsWrapper.Load();
            CompanyInformationWrapper.Load();
            TermsWrapper.Load();
        }

        public void EnsureConfigsExist()
        {
            if (EmailWrapper.Exists()
                && SmtpWrapper.Exists()
                && InvoiceWrapper.Exists()
                && ClientsWrapper.Exists()
                && CompanyInformationWrapper.Exists()
                && TermsWrapper.Exists())
            {
                return;
            }

            DirectoryCreator.EnsureExistance(Folder);

            if (!SmtpWrapper.Exists()) { SmtpWrapper.Save(); }
            if (!EmailWrapper.Exists()) { EmailWrapper.Save(); }
            if (!InvoiceWrapper.Exists()) { InvoiceWrapper.Save(); }
            if (!ClientsWrapper.Exists()) { ClientsWrapper.Save(); }
            if (!CompanyInformationWrapper.Exists()) { CompanyInformationWrapper.Save(); }
            if (!TermsWrapper.Exists()) { TermsWrapper.Save(); }
        }
    }
}