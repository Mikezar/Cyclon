# Cyclon
A small .NET tool for encrypting and decrypting files based on AES algortihm in CBC mode with Pbkdf2 key and iv generator protected by passphrase.

## Installation

```
dotnet tool install --global Cyclon
```

## Usage

To encrypt folder or file provide the path with **--path** argument. 

```
cyclon encrypt --path C:\MyDocuments --passphrase secret
```

To decrypt folder or file provide the path with **--path** argument. 

```
cyclon decrypt --path C:\MyDocuments --passphrase secret
```
