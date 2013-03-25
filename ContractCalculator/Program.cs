using System;
using System.Collections.Generic;
using System.Linq;
using ContractStatisticsAnalyser;
using Data;
using Data.Entities;
using Data.Invoice;
using Extensions;
using UserInterface.ContractRelated;
using UserInterface.InvoiceRelated;

namespace UserInterface
{
    class Program
    {
        private static void Main(string[] args)
        {
            var repo = new Repository();

            if (args.ToList().Contains("-weekly"))
            {
                InvoiceCreationUi.RunAutomatedWeeklyInvoice(repo);
            }
            else
            {
                RunInteractive(repo);
            }
        }

        private static void RunInteractive(Repository repo)
        {
            const string version = "v3.0";

            SetupConsole(version);

            ShowIntro(version);

            var runAgain = true;

            while (runAgain)
            {
                var option = InputReceiver.GetOption("Select a task", new List<string>
                    {
                        "View new contract details",
                        "View existing contract details",
                        "Create custom invoice",
                        "Create weekly invoice",
                        "Create invoice from XML",
                        "Add new client"
                    });

                switch (option)
                {
                    case 0: ContractVisualiser.Visualise(); break;

                    case 1: ContractVisualiser.VisualiseExistingContract(repo); break;

                    case 2: InvoiceCreationUi.CreateCustomInvoiceFromInput(repo); break;

                    case 3: InvoiceCreationUi.CreateWeeklyInvoice(repo); break;

                    case 4: InvoiceCreationUi.CreatedInvoiceFromXml(repo); break;

                    case 5: ClientCreatorUi.AddClient(repo); break;
                }

                runAgain = InputReceiver.GetBool("Run again?");

                Console.WriteLine("");
                Console.WriteLine("");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void ShowIntro(string version)
        {
            Console.WriteLine("Hello and welcome to ISec " + version);
            Console.WriteLine("");
            Console.WriteLine("If you are a software contractor - please get in touch for possible");
            Console.WriteLine("business oppurtunites. Visit: www.hardcoresoftware.co.uk");
            Console.WriteLine("");
        }

        private static void SetupConsole(string version)
        {
            Console.BufferHeight = 1000;
            Console.Title = "Contract Analyser " + version;
            Console.WindowHeight = 32;
        }
    }

    public class ClientCreatorUi
    {
        public static void AddClient(Repository repo)
        {
            var client = new ClientEntity
                {
                    PointOfContactName = InputReceiver.GetString(Nameof<ClientEntity>.Property(e => e.PointOfContactName)),
                    PointOfContactEmail = InputReceiver.GetString(Nameof<ClientEntity>.Property(e => e.PointOfContactEmail)),
                    CompanyInformationEntity = new CompanyInformationEntity
                    {
                        Name = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.Name)),
                        Slogan = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.Slogan), true),
                        AddressLine1 = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.AddressLine1)),
                        AddressLine2 = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.AddressLine2), true),
                        Locality = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.Locality)),
                        PostalTown = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.PostalTown)),
                        PostCode = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.PostCode)),
                        Country = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.Country), true),
                        WebsiteUrl = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.WebsiteUrl), true),
                        CellPhone = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.CellPhone), true),
                        OfficePhone = InputReceiver.GetString(Nameof<CompanyInformationEntity>.Property(e => e.OfficePhone), true)
                    }
                };

            repo.ClientsWrapper.Data.Add(client);
            repo.ClientsWrapper.Save();

            Console.WriteLine("\nNew client \"{0}\" added.\n", client.PointOfContactName);
        }
    }
}