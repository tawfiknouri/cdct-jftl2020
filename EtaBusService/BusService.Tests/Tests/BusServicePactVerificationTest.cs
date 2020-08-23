using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using PactNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BusService.Tests.Tests
{
    public class BusServicePactVerificationTest
    {

        private string consumerName { get; } = "BusConsumer";
        private string providerName { get; } = "BusProvider";

        private string pactServiceUri;
        private IWebHost pactServiceHost;
        private string providerServiceUri;
        private IWebHost providerServiceHost;

        [SetUp]
        public async Task Setup()
        {
            // Start the pact service and the provider-states middleware
            pactServiceUri = "http://localhost:5002";
            pactServiceHost = WebHost.CreateDefaultBuilder()
                .UseUrls(pactServiceUri)
                .UseStartup<TestStartup>()
                .Build();
            await pactServiceHost.StartAsync();

            // Start the provider service (app)
            providerServiceUri = "http://localhost:5000";
            providerServiceHost = WebHost.CreateDefaultBuilder()
                .UseUrls(providerServiceUri)
                .UseStartup<Startup>()
                .Build();
            await providerServiceHost.StartAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await pactServiceHost.StopAsync();
            pactServiceHost.Dispose();

            await providerServiceHost.StopAsync();
            providerServiceHost.Dispose();
        }

        [Test]
        public void EnsurePactDemoProviderHobnoursPactWithPactDemoConsumer()
        {
            var config = new PactVerifierConfig
            {
                Verbose = true
            };
            Console.WriteLine($"Current: {Environment.CurrentDirectory}");

            var pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{pactServiceUri}/provider-states")
                .ServiceProvider("BusProvider", $"{providerServiceUri}")
                .HonoursPactWith("BusConsumer")
                .PactUri($"../../../../../pacts/{consumerName.ToLower()}-{providerName.ToLower()}.json")
                .Verify();
        }

        [Test]
        //[Ignore("PACT BROKER")]
        public void EnsurePactDemoProviderHobnoursPactWithPactDemoConsumerUsingPactBroker()
        {

            var wipSinceDate = "2020-01-01";
            var token = Environment.GetEnvironmentVariable("PACT_BROKER_TOKEN");
            var pactBrokerBaseUrl = "https://expandtesting.pact.dius.com.au";
            var pactUriOptions = new PactUriOptions(token);
            
            // fake Git HashCode (only for demo)
            var version = "11bb818";
         
            var config = new PactVerifierConfig
            {
                ProviderVersion = version,
                PublishVerificationResults = true,
                Verbose = true
            };

            var consumerVersionSelectors = new List<VersionTagSelector>
            {
                new VersionTagSelector("master", null, null,false, true),
            };

            var pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{pactServiceUri}/provider-states");
            pactVerifier.ServiceProvider(providerName, $"{providerServiceUri}");

            var providerVersionTags = new List<string> { "master" };

            // Verify
            pactVerifier.PactBroker(pactBrokerBaseUrl,
                            uriOptions: pactUriOptions,
                            enablePending: false,
                            includeWipPactsSince: wipSinceDate,
                            providerVersionTags: providerVersionTags,
                            consumerVersionSelectors: consumerVersionSelectors)
                                .Verify();

        }
    }
}