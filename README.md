# Outbox Inbox Pattern

#### In this project, a project based on OutboxInbox Pattern was developed.

# Outbox What is Inbox Pattern?

#### Outbox is where the messages are sent but have not yet reached the recipient, and inbox is where the messages the recipient has received but not read yet.

| Section | Description |
| :-------------: | ----------- |
| `Publisher` | Working |
| `Transactional` | Not Working |

# Publisher Section

#### In this section, order information is recorded in the database. The order information here is retrieved at certain intervals and the information is sent to the required service or services via message brokers via events.

![outboxinbox](https://github.com/kadirdemirkaya/OutboxInboxPattern/assets/126807887/1868a39a-4116-4486-b811-98498bb428d5)
