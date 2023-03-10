using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//1-Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://oxystewb:g8ZxUdsUXLX2FY6u0PfNIRftYF7dc2HC@moose.rmq.cloudamqp.com/oxystewb");

//2-Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel= connection.CreateModel();

//3-Queue oluşturma(okunacak kuyruk)
//channel.QueueDeclare(queue: "example-queue", exclusive: false);

//4-Queue’dan mesaj okuma
//mesaj okuyabilmek için bir event oluşturulmalı
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue:"example-queue",autoAck:false,consumer);//bildirilen kuyruğa bir mesaj geldiğinde onu almak için bu şekilde kullanırız.
//channel.BasicQos(0,1,false);                                                               //autoack => mesaj başarılı veya başarısız olarak alındığında silinsin mi? 
//consumer.Received += (sender, e) =>
//{
//    //Kuyruğa gelen mesajların işlendiği yerdir.
//    //e.Body:Kuyruktaki mesajın verisini bütünsel olarak getirecektir.
//    //e.Body.Span veya e.Body.ToArray():Kuyruktaki mesajın byte verisini getirecektir. 

//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//    channel.BasicAck(e.DeliveryTag,multiple:false); //message acknowledgement configured
//};


////Direct Exchange Deneme Başlangıcı//
////1-Publisher'da ki channel ile aynı isim ve türde exchange tanımlanmalıdır.
//channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);//publisher'da ki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımlanmalıdır.

////2-Publisher tarafından rounting key'de bulunan değerdeki kuyruğa gönderilen mesajları, kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz gerekmektedir. Bunun için öncelikle bir kuyruk oluşturulmalıdır.
//string queueName = channel.QueueDeclare().QueueName;

////3-publisher hangi kuyruğa attıysa oraya bağlan.
//channel.QueueBind(queue: queueName, exchange: "direct-exchange-example",routingKey:"direct-queue-example");

////4-mesajları tüket
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue:queueName,autoAck:true,consumer:consumer);
//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
////Direct Exchange Deneme Bitişi//



////Fanout Exchange Deneme Başlangıcı
//channel.ExchangeDeclare(
//    exchange: "fanout-exchange-example",
//    type: ExchangeType.Fanout);

//Console.WriteLine("Kuyruk Adını giriniz : ");
//string queueName = Console.ReadLine();
//channel.QueueDeclare(queue: queueName,
//    exclusive:false);
//channel.QueueBind(queue: queueName,
//    exchange: "fanout-exchange-example",
//    routingKey: string.Empty);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName,
//    autoAck:true,
//    consumer:consumer);

//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
////Fanout Exchange Deneme Bitişi


////Topic Exchange Deneme Başlangıcı//
//channel.ExchangeDeclare(
//    exchange:"topic-exchange-example",
//    type:ExchangeType.Topic);

//Console.Write("Dinlenecek topic formatını belirtiniz : ");
//string topic = Console.ReadLine();
//string queueName=channel.QueueDeclare().QueueName;//kuyruk oluştur ve adına ulaş
//channel.QueueBind(
//    queue:queueName,
//    exchange: "topic-exchange-example",
//    routingKey:topic);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//    queue: queueName,
//    autoAck: true,
//    consumer);


//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
////Topic Exchange Deneme Bitişi//

////Header Exchange Deneme Başlangıcı
channel.ExchangeDeclare(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers
    );
Console.Write("Header value girin : ");
string value = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName;//kuyruk oluşturuldu rabbitmq sunucusuna eklendi. Biz ismini aldık.

channel.QueueBind(
    queue:queueName,
    exchange: "header-exchange-example",
    routingKey:string.Empty,
    new Dictionary<string, object>
    {
        ["no"]=value
    }
    );
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
    );
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

////Header Exchange Deneme Bitişi



Console.Read();