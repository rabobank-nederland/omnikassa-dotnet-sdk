# Rabo Omnikassa .NET SDK

This repository contains the official .NET SDK for [Rabo OmniKassa](https://www.rabobank.nl/omnikassa).

Rabo Omnikassa offers merchants an all-in-one solution to receive payments on your physical and online locations. It includes a dashboard that puts you in full control of your Rabo OmniKassa and all products included in it: Rabo OnlineKassa, payment terminals, Rabo PinBox, Rabo SmartPin, Retourpinnen, Rabo PinTegoed, Rabo Betaalverzoek Plus and payment brands such as: Maestro, V PAY, iDEAL, MasterCard, Visa, PayPal, AfterPay and Sofort.

The .NET SDK allows .NET developers to integrate their web shop with Rabo OmniKassa to handle online payments. Note that besides using an SDK Rabo OmniKassa also provides other ways to integrate that may require less effort. More information on this topic can be found on the [Developer Portal](https://developer.rabobank.nl/overview/rabo-omnikassa) of Rabobank.

Installation instructions and detailed developer documentation on how to use the .NET SDK as well as contact information can be found in the [SDK manual](https://github.com/rabobank-nederland/omnikassa-sdk-doc/blob/main/README.md).

## Release notes

### Version 1.3.5
* Added support for supplying the RefreshToken, SigningKey, CallbackUrl and BaseUrl values via the configuration files for the sample implementations.

| Samples version | Configuration file                                      |
|-----------------|---------------------------------------------------------|
| DotNet50        | samples/OmniKassa.Samples.DotNet50/appsettings.json     |
| DotNet461       | samples/OmniKassa.Samples.DotNet461/Web.config          |
| DotNetCore31    | samples/OmniKassa.Samples.DotNetCore31/appsettings.json |

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
