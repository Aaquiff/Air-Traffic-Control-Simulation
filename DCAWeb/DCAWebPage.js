var airportReq = null;
var airplaneReq = null;
var stepReq = null;

function LoadAirports()
{
    LoadAirportsAsync(fnOn_LoadAirportsAsync_Completion);
}

function LoadAirportsAsync(fnOnCompletion) {
    try {
        airportReq = null;
        if (window.XMLHttpRequest != undefined) {
            airportReq = new XMLHttpRequest();
        }
        else {
            airportReq = new ActiveXObject("Microsoft.XMLHTTP");
        }
        airportReq.onreadystatechange = fnOnCompletion;
        airportReq.open("POST", "DCAService.svc", true);
        airportReq.setRequestHeader("Content-Type", "text/xml;");
        airportReq.setRequestHeader("SOAPAction", "http://tempuri.org/IDCAService/GetAirports");

        var msg =
            '<?xml version="1.0" encoding="utf-8"?> \n\
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/"> \n\
           <soapenv:Body> \n\
                <tem:GetAirports/> \n\
            </soapenv:Body>  \n\
        </soapenv:Envelope> ' ;

        airportReq.send(msg);
    }
    catch (err) {
        alert(err);
    }
}

function fnOn_LoadAirportsAsync_Completion() {

    if (airportReq.readyState == 4) {
        if (airportReq.status == 200) {
            var text = "";
            var ndResult = airportReq.responseXML.documentElement.getElementsByTagName("GetAirportsResult")[0];
            var childCount = ndResult.childElementCount;

            var html = "<h3>Airports</h3>  <div class='list - group'>";

            for (var i = 0; i < childCount; i++) {
                var plane = ndResult.childNodes[i];

                html += "<a class='list-group-item' onclick='LoadPlanes(" + plane.childNodes[0].textContent+")' >" + plane.childNodes[7].textContent+ "</a>";
            }

            html += "</div>";
            divAirports.innerHTML = html;
        }
        else {
            alert("Async call failed - " + airportReq.status + " " + airportReq.statusText);
        }
    }
}

function LoadPlanes(airportId) {
    LoadPlanesAsync(airportId, fnOn_LoadPlanesAsync_Completion);
}

function LoadPlanesAsync(airportId,fnOnCompletion) {
    try {
        airplaneReq = null;
        if (window.XMLHttpRequest != undefined) {
            airplaneReq = new XMLHttpRequest();
        }
        else {
            airplaneReq = new ActiveXObject("Microsoft.XMLHTTP");
        }
        airplaneReq.onreadystatechange = fnOnCompletion;
        airplaneReq.open("POST", "DCAService.svc", true);
        airplaneReq.setRequestHeader("Content-Type", "text/xml;");
        airplaneReq.setRequestHeader("SOAPAction", "http://tempuri.org/IDCAService/GetAirplanes");

        var msg =
            '<?xml version="1.0" encoding="utf-8"?> \n\
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/"> \n\
           <soapenv:Body> \n\
                <tem:GetAirplanes> \n\
                 <tem:airportId>'+ airportId+'</tem:airportId> \n\
                </tem:GetAirplanes> \n\
            </soapenv:Body>  \n\
        </soapenv:Envelope> ' ;

        airplaneReq.send(msg);
        divAirplanes.innerHTML = "<h3>Loading</h3>"
    }
    catch (err) {
        alert(err);
    }
}

function fnOn_LoadPlanesAsync_Completion() {

    if (airplaneReq.readyState == 4) {
        if (airplaneReq.status == 200) {
            var text = "";
            var ndResult = airplaneReq.responseXML.documentElement.getElementsByTagName("GetAirplanesResult")[0];
            var childCount = ndResult.childElementCount;

            var html = "<h3>Planes</h3> <table class='table'> ";
            html += "<thead>";
            html += "<th>Plane Id</th>";
            html += "<th>State</th>";
            html += "<th>Type</th>";
            html += "<th>Remaining Fuel</th>";
            html += "</thead>";
            html += "<tbody>";

            for (var i = 0; i < childCount; i++) {
                var plane = ndResult.childNodes[i];
                html += "<tr>";
                html += "<td>" + plane.childNodes[0].textContent + "</td>"
                html += "<td>" + plane.childNodes[8].textContent + "</td>"
                html += "<td>" + plane.childNodes[7].textContent + "</td>"
                html += "<td>" + plane.childNodes[5].textContent + "</td>"
                html += "</tr>";
                text = text + plane.childNodes[0].textContent + "\n";
                
            }

            html += "</tbody>";
            html += "</table>";
            divAirplanes.innerHTML = html;
        }
        else {
            alert("Async call failed - " + airplaneReq.status + " " + airplaneReq.statusText);
        }
    }
}

function Step()
{
    StepAsync(StepAsyncCompleted);
}

function StepAsync(fnOnCompletion)
{
    try {
        stepReq = null;
        if (window.XMLHttpRequest != undefined) {
            stepReq = new XMLHttpRequest();
        }
        else {
            stepReq = new ActiveXObject("Microsoft.XMLHTTP");
        }
        stepReq.onreadystatechange = fnOnCompletion;
        stepReq.open("POST", "DCAService.svc", true);
        stepReq.setRequestHeader("Content-Type", "text/xml;");
        stepReq.setRequestHeader("SOAPAction", "http://tempuri.org/IDCAService/StepAsync");

        var msg =
            '<?xml version="1.0" encoding="utf-8"?> \n\
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/"> \n\
           <soapenv:Body> \n\
                <tem:StepAsync/> \n\
            </soapenv:Body>  \n\
        </soapenv:Envelope> ' ;

        stepReq.send(msg);
    }
    catch (err) {
        alert(err);
    }
}

function StepAsyncCompleted() {
    if (stepReq.readyState == 4) {
        if (stepReq.status == 200) {

        }
        else {
            alert("Async call failed - " + stepReq.status + " " + stepReq.statusText);
        }
    }
}