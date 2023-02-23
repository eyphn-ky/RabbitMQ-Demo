using RabbitMQ.Client;
using System.Text;
ConnectionFactory factory = new ConnectionFactory();
//bağlantı oluşturma
factory.Uri = new("amqps://oxystewb:g8ZxUdsUXLX2FY6u0PfNIRftYF7dc2HC@moose.rmq.cloudamqp.com/oxystewb");

//bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//queue oluşturma
channel.QueueDeclare(queue:"example-queue",exclusive:false);//durable => kuyruktaki mesajların kalıcığıyla alakalı
                                                            //exlusive => kuyruk özel mi değil mi? Bu kuyrukta birden fazla bağlantıyla bu kuyrukta işlem yapıp yapamayacağımızı belirler.
                                                            //exclusive bu proje için false olmalıdır aynı hem consumer hem publisher bu kuyruğa bağlanabilir olmalıdır.
                                                            //autoDelete=> kuyruğun içindeki tüm mesajlar tükedildikten sonra kuyruk silinsin mi silinmesin mi onu belirler.


//queue'ya mesaj gönderme. 
//RabbitMQ kuyruğa atacağı verileri byte türünden kabul etmektedir. Haliyle mesajları bizim byte türüne dönüştürmemiz gerekecektir.

//Tek bir mesaj göndermek için deneme
//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange:"",routingKey:"example-queue",body:message);//boş bırakmak veya parametre vermemek defaul exchange olan direct exchange ile çalışmasını sağlayacaktır.

//Biraz da oyun oynayalım
for(int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba"+i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);//boş bırakmak veya parametre vermemek defaul exchange olan direct exchange ile çalışmasını sağlayacaktır.
}



//git bak bakalım rabbit mq managerde oluşturmuş mu
Console.Read();