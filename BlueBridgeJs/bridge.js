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
   log.info('Found device with local name: ' + peripheral.advertisement.localName);
   log.info('advertising the following service uuid\'s: ' + peripheral.advertisement.serviceUuids);
};

noble.on('discover', onFound);
