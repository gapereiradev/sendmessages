using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SendMessages.InputModels;
using System.Text;
using System.Text.Json;

namespace SendMessages.Controllers
{
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private const string QUEUE_NAME = "messages";
        private readonly ConnectionFactory _factory;
        public MessagesController()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] SendMessageInputModel sendMessageInputModel)
        { 
            using (var connection = _factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    //Declarar a fila para que caso ela não exista, seja criada
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    // Formatar os dados para envio pra fila
                    var stringMessage = JsonSerializer.Serialize(sendMessageInputModel);
                    var byteArray = Encoding.UTF8.GetBytes(stringMessage);

                    channel.BasicPublish(
                            exchange: "",
                            routingKey: QUEUE_NAME,
                            basicProperties: null,
                            body: byteArray
                        );
                }
            }

            return Accepted();
        }
    }
}
