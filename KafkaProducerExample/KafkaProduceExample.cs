using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Avro;

namespace KafkaProducerExample
{
    public static class KafkaProduceExample
    {
        [FunctionName("KafkaProduceGitHubCommitToKafka")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Kafka("kafka-26b55b43-sifisoshabalala173-36cd.aivencloud.com:27704",
            "EVT_GITHUB_COMMITS",
            Protocol = BrokerProtocol.SaslSsl,
            AuthenticationMode = BrokerAuthenticationMode.Plain,
            Username = "avnadmin",
            SslCaLocation = "ca.pem",
            SslCertificateLocation = "service.cert",
            SslKeyLocation = "service.key",
            Password = "v84gp12ojyz8brnp")] IAsyncCollector<KafkaEventData<string, string>> producer,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var jobj = JsonConvert.DeserializeObject<GitHubPushModel>(requestBody);

            var message = new GitHubMessage(jobj);

            var data = new KafkaEventData<string, string>
            {
                Key = message.Author,
                Value = JsonConvert.SerializeObject(message)
            };

            await producer.AddAsync(data);

            return new OkObjectResult("Message successfully produced");
        }
    }
}
