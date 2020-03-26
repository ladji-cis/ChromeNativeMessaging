
var port = null;

connect();

SendNativeMessage("Hello World!");

function SendNativeMessage(message) 
{	
	port.postMessage({"text": message});
	console.log("SEND: " + message);
}

function onNativeMessage(message) 
{	
	console.log("RECEIVE: " + message);
	var command = message["echo"];		
	if(!command)
	{
		//There's no text
		SendNativeMessage("Empty message received");
		return;
	}
	
	var commandParams = command.split(" ");
	var firstCommand = commandParams[0];
}

function onDisconnected() {
  port = null;
}

function connect() {
  var hostName = "com.google.chrome.example.echo";
  port = chrome.runtime.connectNative(hostName);
  port.onMessage.addListener(onNativeMessage);
  port.onDisconnect.addListener(onDisconnected);
}