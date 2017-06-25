var winston = require('winston');
require('winston-daily-rotate-file');

var logLvl = process.env.LOG ? process.env.LOG : 'info';

var fileTr = new winston.transports.DailyRotateFile({
   filename: 'bridge.log',
   datePattern: 'yyyy-MM-dd.',
   prepend: true,
   level: logLvl
});

var consTr = new winston.transports.Console({
   level: logLvl
});

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

var multiAddr = process.env.ADDR ? process.env.ADDR : '192.168.178.255';
var multiPort = process.env.PORT ? process.env.PORT : 4242;
var dgram = require('dgram');
var server = dgram.createSocket('udp4');
server.bind(multiPort, function(){
    server.setBroadcast(true);
    server.setMulticastTTL(128);
});

log.info('I will post to '+multiAddr+'/'+multiPort+' and log is '+logLvl+'!');

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
   var json = JSON.stringify(advertise);
   var message = new Buffer(json);
   server.send(message, 0, message.length, multiPort, multiAddr);
   log.debug('Sent ' + message.length + ' bytes => ' + json);
};

noble.on('discover', onFound);
