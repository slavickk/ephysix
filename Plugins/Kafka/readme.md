# Sender and Receiver implementations for Kafka

## KafkaSender

The `KafkaSender` class is a Kafka producer that sends messages to a Kafka topic. It is configured with the following parameters
in the YAML configuration file:

* `bootstrapServers`: a comma-separated list of Kafka brokers to connect to
* `topic`: the Kafka topic to send messages to

## KafkaReceiver

The `KafkaReceiver` class is a Kafka consumer that receives messages from a Kafka topic. It is configured with the following parameters
in the YAML configuration file:

* `bootstrapServers`: a comma-separated list of Kafka brokers to connect to
* `topic`: the Kafka topic to receive messages from

## Running locally

To run `KafkaSender` and `KafkaReceiver` locally, you need to have a Kafka broker running locally.
You can use the following docker-compose file, which is based on
https://github.com/bitnami/containers/blob/main/bitnami/kafka/docker-compose.yml

```yaml
# Copyright VMware, Inc.
# SPDX-License-Identifier: APACHE-2.0

version: "2"

services:
  kafka:
    image: docker.io/bitnami/kafka:3.6
    container_name: kafka
    ports:
      - "9092:9092"
    volumes:
      - "kafka_data:/bitnami"
    environment:
      # KRaft settings
      - KAFKA_CFG_NODE_ID=0
      - KAFKA_CFG_PROCESS_ROLES=controller,broker
      - KAFKA_CFG_CONTROLLER_QUORUM_VOTERS=0@kafka:9093
      # Listeners
      - KAFKA_CFG_LISTENERS=PLAINTEXT://:9092,CONTROLLER://:9093
      - KAFKA_CFG_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092
      - KAFKA_CFG_LISTENER_SECURITY_PROTOCOL_MAP=CONTROLLER:PLAINTEXT,PLAINTEXT:PLAINTEXT
      - KAFKA_CFG_CONTROLLER_LISTENER_NAMES=CONTROLLER
      - KAFKA_CFG_INTER_BROKER_LISTENER_NAME=PLAINTEXT
  kafka-ui:
    container_name: kafka-ui
    image: provectuslabs/kafka-ui:latest
    ports:
      - 8080:8080
    environment:
      DYNAMIC_CONFIG_ENABLED: 'true'
volumes:
  kafka_data:
    driver: local
```

Strictly speaking, you only need the Kafka broker, but the Kafka UI is a nice tool to inspect the Kafka topics.

