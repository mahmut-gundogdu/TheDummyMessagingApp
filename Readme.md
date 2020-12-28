
# Messaging Rest Api
Bu rest api .net core 5.0 uzerinde calismaktadir. Veritabani olarak MongoDb kullanmaktadir. Projeyi ayaga kaldirmak icin \MessagingService klasoru icerisinde "docker-compose up" demeniz yeterlidir. 


Yeni Üye Kaydı olusturmak için
> POST /api/Users/Register
> Request body; 
> `{
  "userName": "",
  "name": "",
  "surname": "",
  "email": "",
  "password": ""
}
`

 Oturum/Uye tonetimi icin Jwt token kullanmaktadır. Oturum anahtarı almak için 
> GET /api/Users/Authenticate
> Request: `{
    "token": "",
    "expiration": ""
}`
> Response 
> `{
    "UserName":"John.Doe",
    "HashedPassword":"8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
}`

Messaging Endpointindeki tum adresler oturum gerektirmektedir. Jwt tokeninizi headerda asagidaki ornekteki gibi ekleyiniz. 

    Authorization: Bearer <ACCESS_TOKEN>


Bir kullaniciyi engellemek icin jwt token ile birlikte
> POST /api/Users/BlockUser
> Request body:
> `{ "blockedUserName":"" }`

Bir kullanicinin engelini kaldirmak icin icin jwt token ile birlikte
> POST /api/Users/UnBlockUser
> Request body:
> `{ "unblockedUserName":"" }`



Kullanici Adini bildiginiz bir uyeye mesaj gondermek icin:

> POST /api/Messaging
> Request body:
> `{ "receiverUserName":"" , message:""}`


Mevcut sohbetleri getirmek icin:

> GET /api/Messaging

Belirli bir kullaniciya ait  sohbeti getirmek icin:

> GET /api/Messaging/John.Doe


Projedeki birim testlerini calistirmak icin \MessagingService.Tests klasorunde asagidaki komutu calistirmaniz yeterlidir. 
> dotnet test

Soru ve onerileriniz icin Issue acabilir veya herhangi bir kanaldan bana ulasabilirsiniz.