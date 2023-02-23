using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://oxystewb:g8ZxUdsUXLX2FY6u0PfNIRftYF7dc2HC@moose.rmq.cloudamqp.com/oxystewb");

//Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel= connection.CreateModel();

//Queue oluşturma(okunacak kuyruk)
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//Queue’dan mesaj okuma
//mesaj okuyabilmek için bir event oluşturulmalı
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:"example-queue",false,consumer);//bildirilen kuyruğa bir mesaj geldiğinde onu almak için bu şekilde kullanırız.
                                                           //autoack => mesaj alındığında silinsin mi
consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajların işlendiği yerdir.
    //e.Body:Kuyruktaki mesajın verisini bütünsel olarak getirecektir.
    //e.Body.Span veya e.Body.ToArray():Kuyruktaki mesajın byte verisini getirecektir. 

    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
Console.Read();