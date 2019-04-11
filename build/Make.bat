Echo 1 
..\makecert.exe -n "CN=CARoot" -r -pe -a sha512 -len 4096 -cy authority -sv CARoot.pvk CARoot.cer
 
Echo 2
..\pvk2pfx.exe -pvk CARoot.pvk -spc CARoot.cer -pfx CARoot.pfx

Echo 3
..\makecert.exe -n "CN=test" -iv CARoot.pvk -ic CARoot.cer -pe -a sha512 -len 4096 -b 01/01/2014 -e 01/01/2020 -sky exchange -eku 1.3.6.1.5.5.7.3.1 -sv test.pvk test.cer
 
Echo 4
..\pvk2pfx.exe -pvk test.pvk -spc test.cer -pfx test.pfx  

