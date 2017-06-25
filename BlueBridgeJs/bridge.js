var noble = require('noble');

var onState = function (state) 
{
   console.log(state);
}

noble.on('stateChange', onState);
