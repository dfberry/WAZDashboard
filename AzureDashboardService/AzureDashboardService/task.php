<?php

// dina's cron job
// http://www.bluehostforum.com/archive/index.php/t-5762.html
// /usr/bin/php $HOME/dinafbery/dfbscripts/task.php


// Request Yahoo! REST Web Service using
// HTTP GET with curl. PHP4/PHP5
// Allows retrieval of HTTP status code for error reporting
// Author: Jason Levitt
// February 1, 2006

error_reporting(E_ALL);

$to = "dina@berryintl.com";
$subject = "bluehost cron job";
$message = "top";
$headers = 'From: dina@dinafberry.com' . "\r\n" .
    'Reply-To: dina@dinafberry.com' . "\r\n" .
    'X-Mailer: PHP/' . phpversion();

$from = "dina@dinafberry.com";

mail($to,$subject,$message,$headers,$from);

// The Yahoo! Web Services request
$request =  'http://wazdashboard.apphb.com/Home/Tasks';

// Initialize the session
$session = curl_init($request);

// Set curl options
curl_setopt($session, CURLOPT_HEADER, true);
curl_setopt($session, CURLOPT_RETURNTRANSFER, true);

// Make the request
$response = curl_exec($session);

// Close the curl session
curl_close($session);

// Get HTTP Status code from the response
$status_code = array();
preg_match('/\d\d\d/', $response, $status_code);

// Check the HTTP Status code
switch( $status_code[0] ) {
	case 200:
		// Success
		break;
	case 503:
		die('Your call to Yahoo Web Services failed and returned an HTTP status of 503. That means: Service unavailable. An internal problem prevented us from returning data to you.');
		break;
	case 403:
		die('Your call to Yahoo Web Services failed and returned an HTTP status of 403. That means: Forbidden. You do not have permission to access this resource, or are over your rate limit.');
		break;
	case 400:
		// You may want to fall through here and read the specific XML error
		die('Your call to Yahoo Web Services failed and returned an HTTP status of 400. That means:  Bad request. The parameters passed to the service did not match as expected. The exact error is returned in the XML response.');
		break;
	default:
		die('Your call to Yahoo Web Services returned an unexpected HTTP status of:' . $status_code[0]);
}

// Get the XML from the response, bypassing the header
if (!($xml = strstr($response, '<?xml'))) {
	$xml = null;
}

// Output the XML
echo htmlspecialchars($xml, ENT_QUOTES);

$message = "bottom";
mail($to,$subject,$message,$headers,$from);

?>