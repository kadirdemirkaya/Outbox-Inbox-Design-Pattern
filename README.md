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

![outboxinbox](https://github.com/kadirdemirkaya/OutboxInboxPattern/assets/126807887/1868a39a-4116-4486-b811-98498bb428d5)
