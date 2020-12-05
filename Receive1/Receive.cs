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
            /*Pertukaran di kanal dinamakan "test", dan bertipe fanout.*/
            /*Tipe fanout akan membroadcast pesan ke semua queue.*/
            channel.ExchangeDeclare(exchange: "test", type: ExchangeType.Fanout);

            /*Membuat queue yang akan menerima pesan*/
            var antrianPesan = channel.QueueDeclare().QueueName;
            /*Melanggan ke "test"*/
            channel.QueueBind(queue: antrianPesan,
                              exchange: "test",
                              routingKey: "");

            /*Indikator queue sudah siap, dan sedang menunggu*/
            Console.WriteLine(" Menunggu pesan...");

            /*Membuat penerima pesan*/
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" Isi pesan: {0}", message); //Pesan ditangkap dan ditampilkan
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
