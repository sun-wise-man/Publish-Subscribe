using System;
using System.Text;
using System.Windows.Forms;
using RabbitMQ.Client;

namespace Publisher
{
    public partial class Form1 : Form
    {
        public string exName;
        public IModel channel;
        public IConnection conn;

        public Form1()
        {
            InitializeComponent();
            //Membuat koneksi dengan Host lokal
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";

            conn = factory.CreateConnection();

            //Membuat kanal
            channel = conn.CreateModel();

            //Default exchange adalah "logs".
            //Nantinya pengguna dapat mengganti exchange dengan
            //me-replace "logs" dengan exname.
            exName = "logs";
            textBox3.Text = exName;
            //Membuat exchange
            channel.ExchangeDeclare(exName, ExchangeType.Fanout);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            channel.Close();
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Mengambil tulisan yang diinput
            string pesan = richTextBox1.Text;
            //Encoding pesan dengan UTF8
            var body = Encoding.UTF8.GetBytes(pesan);
            //Pesan akan dikirimkan ke variable "exName"
            channel.BasicPublish(exchange: exName,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
            textBox1.Text = pesan;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            exName = textBox2.Text;
            textBox3.Text = exName;
            Console.WriteLine(exName);
            channel.ExchangeDeclare(exName, ExchangeType.Fanout);
        }
    }
}
