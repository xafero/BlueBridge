var winston = require('winston');
require('winston-daily-rotate-file');

var fileTr = new winston.transports.DailyRotateFile({
   filename: 'bridge.log',
   datePattern: 'yyyy-MM-dd.',
   prepend: true,
   level: process.env.ENV === 'development' ? 'debug' : 'info'
});

var consTr = new (winston.transports.Console)();

var log = new (winston.Logger)({ transports: [consTr, fileTr] });

var noble = require('noble');

var onState = function (state) 
{
   log.info('Bluetooth => ' + state);
   if (state === 'poweredOn') {
      log.info('Starting scan...');
      var uuids = [ ];
      var allowDups = true;
      noble.startScanning(uuids, allowDups);
   } else {
      log.info('Stopping scan...');
      noble.stopScanning();
   }
};

noble.on('stateChange', onState);

var onFound = function (peripheral) 
{
   var advertise = { 
      ID : peripheral.id,
      Address : peripheral.address,
      AddressType : peripheral.addressType,
      UUID : peripheral.uuid,
      Name : peripheral.advertisement.localName,
      Services : peripheral.advertisement.serviceUuids,
      Connectable : peripheral.connectable,
      RSSI : peripheral.rssi,
      ServiceData : peripheral.advertisement.serviceData.toString('hex'),
      ManufacturerData : peripheral.advertisement.manufacturerData.toString('hex'),
      PowerLevel : peripheral.advertisement.txPowerLevel
   };

   console.log(JSON.stringify(advertise));
};

noble.on('discover', onFound);
