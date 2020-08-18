# sendme
sendme is a console application to send dump of CAN logs using CHAI library

## Description
sendme is a console application to send logs using CAN CHAI library.

Supported feature list:
- CAN messages in an extended format
- Proper delay between sent messages based on timestamps
- Logs are suppoted in an TXT format

## Dependencies

- [ CHAI library](http://can.marathon.ru/page/prog/chai) should be installed in your system

## Usage

You can run the application from console

`./sendme.exe -ch 0 -p logs.txt`

### Available options:
- `-ch` Specifies CAN channel to use. The numeration starts from 0.
- `-p` Specifies path to dump file to send over CAN.

### Format of input file
Dump should be provided in a simple `txt` format. Every message should be separated by a new line. Below is an example:

```
DEADBEEF	10	1D	62	F1	91	54	45	41	13:04:19.722
DEADBEEF	45	32	2B	1	1	32	33	32	13:04:19.744
DEADBEEF	12	35	32	39	31	33	50	30	13:04:19.765
```
