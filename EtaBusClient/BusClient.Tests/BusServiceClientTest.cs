using NUnit.Framework;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusClient.Tests
{
    [TestFixture]
    public class BusServiceClientTest : PactTestBase
    {
        [SetUp]
        public void SetUp()
        {
            MockProviderService.ClearInteractions();
        }

        [Test]
        public async Task GetEtaForNextTrip_ReturnsExpectedTripInformation()
        {
            // /eta/routeName/direction/stopName
            
            // Arrange

            string routeName = "20";
            string direction = "Northbound";
            string stopName = "Opera";

            var busId = 99999;
            var eta = 5;

            var busIdRegex = "^[1-9]{1,5}$"; // All positive integers between 1 and 99999
            var etaIdRegex = "^[0-9]{0,2}$"; // All positive integers between 0 and 99
            
            var expectedBusInfo = new
            {
                busID = Match.Type(busId),
                eta = Match.Type(eta),
            };

            MockProviderService
                .Given($"There are buses scheduled for route {routeName} and direction {direction} to arrive at stop {stopName}") // Describe the state the provider needs to setup
                .UponReceiving($"A request for eta for route {routeName} to {stopName} station") // textual description - business case
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = Uri.EscapeUriString($"/eta/{routeName}/{direction}/{stopName}"),
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = expectedBusInfo
                });

            var consumer = new BusServiceClient(MockServerBaseUri);

            // Act
            var result = await consumer.GetBusInfo(routeName, direction, stopName);

            // Assert
            Assert.That(result.BusID.ToString(), Does.Match(busIdRegex));
            Assert.That(result.Eta.ToString(), Does.Match(etaIdRegex));

            // NOTE: Verifies that interactions registered on the mock provider are called at least once
            MockProviderService.VerifyInteractions();
        }

        #region Pact-Broker

        [Test]
        //[Ignore("PACT FLOW")]
        public void PublishToPactFlow()
        {
            // fake Git HashCode (only for demo)
            var version = "725c611";
            // master (only for demo)
            var tags = new[] { "master" };
            var token = Environment.GetEnvironmentVariable("PACT_BROKER_TOKEN");
            var pactPublisher = new PactPublisher("https://expandtesting.pact.dius.com.au", new PactUriOptions(token));
            pactPublisher.PublishToBroker($"../../../../../pacts/{ConsumerName.ToLower()}-{ProviderName.ToLower()}.json", version, tags);
        }

        #endregion
    }
}
