using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class ReceiveLogs
{
    public static void Main()
    {
        /* Membuat koneksi, dengan host lokal*/
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())

        /*Membuat kanal untuk komunikasi*/
        using (var channel = connection.CreateModel())
        {
            /*Pertukaran di kanal dinamakan "logs", dan bertipe fanout.*/
            /*Tipe fanout akan membroadcast pesan ke semua queue.*/
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            /*Membuat queue yang akan menerima pesan*/
            var antrianPesan = channel.QueueDeclare().QueueName;
            /*Melanggan ke "logs"*/
            channel.QueueBind(queue: antrianPesan,
                              exchange: "logs",
                              routingKey: "");

            /*Indikator queue sudah siap, dan sedang menunggu*/
            Console.WriteLine(" Menunggu pesan...");

            /*Membuat penerima pesan*/
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                //Pesan di decode
                var message = Encoding.UTF8.GetString(body);
                //Pesan ditangkap dan ditampilkan
                Console.WriteLine(" Isi pesan: {0}", message);
                Console.WriteLine(" Pesan diterima!");
                Console.WriteLine(" [enter] untuk keluar.\n");
            };
            channel.BasicConsume(queue: antrianPesan,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" [enter] untuk keluar.\n");
            Console.ReadLine();
        }
    }
}
