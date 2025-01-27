# MassTransit Session Key Generator

## Description
This project is a .NET 9 console-hosted service that utilizes MassTransit with an in-memory transport. It includes:
- A service to process session key requests using `Rfc2898DeriveBytes`.
- An event notification service that generates AES keys and IVs from the session key.

## Features
1. **Session Key Request Service**: Generates session keys based on input strings and publishes a notification.
2. **Notification Service**: Listens for session key notifications and generates AES keys and IVs.

## How to Run
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd <repository-folder>
