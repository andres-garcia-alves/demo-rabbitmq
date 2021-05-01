using System;
using System.Text;
using RabbitMQ.Client;

namespace Sender
{
    class Program
    {       
        static void Main(string[] args)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "MSG", durable: false, exclusive: false, autoDelete: false, arguments: null);

                        Console.WriteLine("");
                        Console.WriteLine("RabbitMQ Demo: Sender");
                        Console.WriteLine("");
                        Console.WriteLine("Valid commands:");
                        Console.WriteLine("MESSAGE <<text to send>>");
                        Console.WriteLine("EXIT");
                        Console.WriteLine("");

                        ProcessCommands(channel: channel);
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        static void ProcessCommands(IModel channel)
        {
            try
            {
                bool bExit = false;

                while (!bExit)
                {
                    string command = Console.ReadLine();
                    
                    if (command.ToUpper().StartsWith("MESSAGE "))
                    {
                        string message = command.Substring(8);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "", routingKey: "MSG", body: body, basicProperties: null);
                        Console.WriteLine($"Message sended: '{message}'");
                        Console.WriteLine("");
                    }
                    else if (command.ToUpper() == "EXIT")
                    {
                        bExit = true;
                    }
                    else
                    {
                        Console.WriteLine($"Unknown command: '{command}'.");
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }
    }
}
