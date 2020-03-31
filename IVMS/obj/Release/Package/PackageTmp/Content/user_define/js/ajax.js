// JavaScript Document

var result;
var xmlhttp;
var theurl;
var setdiv;
function ajax_state(url,setdiv1)
{
	theurl = url;
	setdiv=setdiv1;
	xmlhttp=null;
	
	//alert(theurl);
	// code for IE7+, Firefox, Chrome, Opera, Safari
	if (window.XMLHttpRequest){
		xmlhttp=new XMLHttpRequest();
	}
	// code for IE6, IE5
	else if (window.ActiveXObject){
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
	}	
	if (xmlhttp != null) {
		xmlhttp.onreadystatechange=state_Change;
		//today =  new Date();
		//rnd = today.getTime( );  
		//url = url + "&rnd=" + rnd;
		xmlhttp.open("GET", url, true);
		xmlhttp.send(null);
	}else{
		alert("Your browser does not support XMLHTTP.");
	}
}
function state_Change(){
	// if xmlhttp shows "loaded"
	if (xmlhttp.readyState==4){
		// if "OK"
		if (xmlhttp.status==200){
			result = xmlhttp.responseText;
			document.getElementById(setdiv).innerHTML = result;
		}else{
			alert("Problem retrieving XML data");
		}
	}
}


