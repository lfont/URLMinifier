URLMinifier
===========
A simple url minifier.

Hacking
-------
The code has only been tested on [Linux](http://nixos.org) !

1. Install [fsharp](http://fsharp.org/use/linux)  
On a Debian base system the simple steps are:  
`sudo apt install mono-complete fsharp`
2. If you do not use the latest mono, update the certificates:  
`mozroots --import --sync`
3. Install GNU make  
`sudo apt install make`
4. Install npm  
`sudo apt install npm`
5. Build the code  
`make && make test`
6. If all goes well  
`make run`
7. Enjoy :)  
  
On mono the `-d` argument must be used for putting the process in the backgroud:
`nohup mono url-minifier.exe -d &`

About
-----
* License: MIT (see included [LICENSE](https://github.com/lfont/URLMinifier/blob/master/LICENSE) file for full license)
* Original Author: [Loïc Fontaine](http://twitter.com/loic_fontaine)
