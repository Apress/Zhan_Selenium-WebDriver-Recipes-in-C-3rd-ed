# kill the hanging browser processes, used in BuildWise prepare step
kill -9 $(ps -x | grep Chrome)
kill -9 $(ps -x | grep Firefox)
killall -9 chromedriver
exit 0
