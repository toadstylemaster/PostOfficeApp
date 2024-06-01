# PostOfficeApp

## Introduction

Post office application. With this application you can add and delete shipments, add bags to existing shipment, and add parcels to existing bags.

## Setup

To setup application:

1. Cloning the repository : Git link: https://github.com/toadstylemaster/PostOfficeApp/tree/main
2. Building solution
3. Starting the server

## API Documentation

### BagWithLetters API

- **GET /api/BagWithLetters**: Returns a list of all BagWithLetters.
- **GET /api/BagWithLetters/{id}**: Returns the BagWithLetters with the specified ID.
- **POST /api/BagWithLetters**: Creates a new BagWithLetters.
- **DELETE /api/BagWithLetters/{id}**: Deletes the BagWithLetters with the specified ID.

### BagWithParcels API

- **GET /api/BagWithParcels**: Returns a list of all BagWithParcels.
- **GET /api/BagWithParcels/{id}**: Returns the BagWithParcels with the specified ID.
- **POST /api/BagWithParcels**: Creates a new BagWithParcels.
- **DELETE /api/BagWithParcels/{id}**: Deletes the BagWithParcels with the specified ID.

### Parcel API

- **GET /api/Parcel**: Returns a list of all Parcels.
- **GET /api/Parcel/{id}**: Returns the Parcel with the specified ID.
- **POST /api/Parcel**: Creates a new Parcel.
- **DELETE /api/Parcel/{id}**: Deletes the Parcel with the specified ID.

### Shipment API

- **GET /api/Shipment**: Returns a list of all Shipments.
- **GET /api/Shipment/{id}**: Returns the Shipment with the specified ID.
- **POST /api/Shipment**: Creates a new Shipment.
- **DELETE /api/Shipment/{id}**: Deletes the Shipment with the specified ID.