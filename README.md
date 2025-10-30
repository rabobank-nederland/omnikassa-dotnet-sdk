# Rabo Omnikassa .NET SDK

This repository contains the official .NET SDK for [Rabo OmniKassa](https://www.rabobank.nl/omnikassa).

Rabo Omnikassa offers merchants an all-in-one solution to receive payments on your physical and online locations. It includes a dashboard that puts you in full control of your Rabo OmniKassa and all products included in it: Rabo OnlineKassa, payment terminals, Rabo PinBox, Rabo SmartPin, Retourpinnen, Rabo PinTegoed, Rabo Betaalverzoek Plus and payment brands such as: Maestro, V PAY, iDEAL, MasterCard, Visa, PayPal, AfterPay and Sofort.

The .NET SDK allows .NET developers to integrate their web shop with Rabo OmniKassa to handle online payments. Note that besides using an SDK Rabo OmniKassa also provides other ways to integrate that may require less effort. More information on this topic can be found on the [Developer Portal](https://developer.rabobank.nl/overview/rabo-omnikassa) of Rabobank.

Installation instructions and detailed developer documentation on how to use the .NET SDK as well as contact information can be found in the [SDK manual](https://github.com/rabobank-nederland/omnikassa-sdk-doc/blob/main/README.md).

## Release notes

### Version 1.6.0
* Add Docker configuration for development and testing.
* Add GitHub workflow for PR testing.
* Add support for the OrderStatus API.
* Add support for 'Card on File'
* Add support for 'Fast checkout'
* Removed support for older .NET versions (EOL) and updated target frameworks.

### Version 1.5.0
* Added partner reference support
* Added UserAgent support
* Added shopperBankStatementReference support
* Added .NET 7 and 8 to target frameworks, and removed netstandard targets
* Migrated .NET 4.6.2 sample to .NET 6.0

### Version 1.4.0
* Extended SDK to support refunds.
* Added support for .NET 6.
* Removed support for EOL .NET versions.

### Version 1.3.7
* Added support for supplying the RefreshToken, SigningKey, CallbackUrl and BaseUrl values via the configuration files for the sample implementations.

| Samples version | Configuration file                                      |
|-----------------|---------------------------------------------------------|
| DotNet50        | samples/OmniKassa.Samples.DotNet50/appsettings.json     |
| DotNet461       | samples/OmniKassa.Samples.DotNet461/Web.config          |
| DotNetCore31    | samples/OmniKassa.Samples.DotNetCore31/appsettings.json |

### Version 1.3.5
* Fixed an automated pipeline issue (No changes in code)

### Version 1.3.4
* First automated pipeline release for NuGet (No changes in code)

### Version 1.3.2
* Added correct package information for NuGet (No changes in code)

### Version 1.3.0
* Extended SDK to support SOFORT as payment brand.

### Version 1.2.0
* Extended SDK to retrieve a list of iDEAL issuers.
* Extended SDK to allow the iDEAL issuer to be passed in the order announcement.
* Extended SDK to allow the payment result page (also known as the 'thank-you' page) to be skipped in the payment process.
* Extended SDK to allow the name of the customer to be passed in the order announcement.
* Added support for .NET 5.


## Docker configuration

### Dev and test containers

The compose file contains services for dev and test. Dev containers use volume mapping, test containers copy the source into the container.
This makes cross-platform development easier, as the test containers can be run from any platform, as long as the Docker engine uses the 
appropriate platform (Linux or Windows) for the container.

As volume mapping generally doesn't work cross-platform, you should use a 'matching' platform for development. For testing only 
the Docker engine platform is relevant. Linux containers can run on any platform, Windows containers can only run on Windows.

### Running the tests

To run the tests, you can use the following command:

```bash
docker compose up --build TARGET_SERVICE
```

For example:
```bash
docker compose up --build netcore6-windows-test
```

For the target services, see the `compose.yaml` file, any service that ends with `-test` can be used as a target service.

### Using the dev containers

To use the dev containers, you can use the following command:

```bash
docker compose run --rm TARGET_SERVICE shell
```

For example (Linux):
```bash
docker compose run --rm netcore6-linux-dev /bin/bash
```

For example (Windows):
```bash
docker compose run --rm netframework46-windows-dev cmd.exe
```
