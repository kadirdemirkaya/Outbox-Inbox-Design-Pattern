# Outbox Inbox Pattern

#### Bu projede de bunu baz alan bir proje geliştirilmiştir.

# Outbox Inbox Pattern Nedir ?

#### Outbox, gönderilen ancak henüz alıcıya ulaşmamış iletilerin, inbox ise alıcının aldığı ancak henüz okumamış olduğu iletilerin bulunduğu yerdir.

|      Bölüm      | Açıklama    |
| :-------------: | ----------- |
|   `Publisher`   | Working     |
| `Transactional` | Not Working |

# Publisher Kısmı

#### Bu kısım da veri tabanına order bilgileri kaydedilir. Belirli zaman aralıklarla buradaki order bilgileri çekilir ve message broker ile gerekli servis veya servislere bilgiler eventlar ile gönderilir.
