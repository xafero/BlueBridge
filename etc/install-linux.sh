#!/bin/sh
sudo apt-get install -y htop mc vim nano iotop iftop
sudo apt-get install -y bluez-tools bluez-firmware bluez-hcidump bluez libbluetooth3-dev libbluetooth3 bluetooth libbluetooth-dev libudev-dev
wget "https://github.com/winterheart/broadcom-bt-firmware/blob/master/brcm/BCM20702A1-0a5c-21e8.hcd?raw=true"
sudo mv "BCM20702A1-0a5c-21e8.hcd?raw=true" /lib/firmware/brcm/BCM20702A1-0a5c-21e8.hcd
sudo apt-get install -y build-essential
curl -sL https://deb.nodesource.com/setup_8.x | sudo -E bash -
sudo apt-get install -y nodejs
sudo apt-get autoremove
sudo npm install -g node-gyp
sudo npm install --unsafe-perm -g noble
sudo setcap cap_net_raw+eip $(eval readlink -f `which node`)
