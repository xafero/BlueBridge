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
   log.info('Bluetooth is now ' + state);
}

noble.on('stateChange', onState);
