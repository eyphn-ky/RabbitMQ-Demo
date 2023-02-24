using RabbitMQ.Client;
using System.Text;
ConnectionFactory factory = new ConnectionFactory();
//1-bağlantı oluşturma
factory.Uri = new("amqps://oxystewb:g8ZxUdsUXLX2FY6u0PfNIRftYF7dc2HC@moose.rmq.cloudamqp.com/oxystewb");

//2-bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//3-queue oluşturma
//channel.QueueDeclare(queue:"example-queue",exclusive:false,durable:true);//durable => kuyruktaki mesajların kalıcığıyla alakalı
//                                                                         //exlusive => kuyruk özel mi değil mi? Bu kuyrukta birden fazla bağlantıyla bu kuyrukta işlem yapıp yapamayacağımızı belirler.
//                                                                         //exclusive bu proje için false olmalıdır aynı hem consumer hem publisher bu kuyruğa bağlanabilir olmalıdır.
//                                                                         //autoDelete=> kuyruğun içindeki tüm mesajlar tükedildikten sonra kuyruk silinsin mi silinmesin mi onu belirler.


//4-queue'ya mesaj gönderme. 
//RabbitMQ kuyruğa atacağı verileri byte türünden kabul etmektedir. Haliyle mesajları bizim byte türüne dönüştürmemiz gerekecektir.

//Tek bir mesaj göndermek için deneme
//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange:"",routingKey:"example-queue",body:message);//boş bırakmak veya parametre vermemek defaul exchange olan direct exchange ile çalışmasını sağlayacaktır.


//Biraz da oyun oynayalım
//IBasicProperties properties=channel.CreateBasicProperties();
//properties.Persistent = true;
//for(int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba"+i);
//    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message,basicProperties:properties);//boş bırakmak veya parametre vermemek defaul exchange olan direct exchange ile çalışmasını sağlayacaktır.
//}

//Direct Exchange Deneme Başlangıcı//
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);
while (true)
{
    Console.Write("Mesaj : ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey:"direct-queue-example",
        body:byteMessage);
}
//Direct Exchange Deneme Bitişi//

Console.Read();