var req = null;

function LoadAirports()
{
    LoadAirportsAsync(fnOn_LoadAirportsAsync_Completion);
}

function LoadAirportsAsync(fnOnCompletion) {
    try {
        req = null;
        if (window.XMLHttpRequest != undefined) {
            req = new XMLHttpRequest();
        }
        else {
            req = new ActiveXObject("Microsoft.XMLHTTP");
        }
        req.onreadystatechange = fnOnCompletion;
        req.open("POST", "DCAService.svc", true);
        req.setRequestHeader("Content-Type", "text/xml;");
        req.setRequestHeader("SOAPAction", "http://tempuri.org/IDCAService/GetAirports");

        var msg =
            '<?xml version="1.0" encoding="utf-8"?> \n\
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/"> \n\
           <soapenv:Body> \n\
                <tem:GetAirports/> \n\
            </soapenv:Body>  \n\
        </soapenv:Envelope> ' ;

        req.send(msg);
    }
    catch (err) {
        alert(err);
    }
}

function fnOn_LoadAirportsAsync_Completion() {

    if (req.readyState == 4) {
        if (req.status == 200) {
            var text = "";
            var ndResult = req.responseXML.documentElement.getElementsByTagName("GetAirportsResult")[0];
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
            alert("Async call failed - " + req.status + " " + req.statusText);
        }
    }
}

function LoadPlanes(airportId) {
    LoadPlanesAsync(airportId, fnOn_LoadPlanesAsync_Completion);
}

function LoadPlanesAsync(airportId,fnOnCompletion) {
    try {
        req = null;
        if (window.XMLHttpRequest != undefined) {
            req = new XMLHttpRequest();
        }
        else {
            req = new ActiveXObject("Microsoft.XMLHTTP");
        }
        req.onreadystatechange = fnOnCompletion;
        req.open("POST", "DCAService.svc", true);
        req.setRequestHeader("Content-Type", "text/xml;");
        req.setRequestHeader("SOAPAction", "http://tempuri.org/IDCAService/GetAirplanes");

        var msg =
            '<?xml version="1.0" encoding="utf-8"?> \n\
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/"> \n\
           <soapenv:Body> \n\
                <tem:GetAirplanes> \n\
                 <tem:airportId>'+ airportId+'</tem:airportId> \n\
                </tem:GetAirplanes> \n\
            </soapenv:Body>  \n\
        </soapenv:Envelope> ' ;

        req.send(msg);
        divAirplanes.innerHTML = "<h3>Loading</h3>"
    }
    catch (err) {
        alert(err);
    }
}

function fnOn_LoadPlanesAsync_Completion() {

    if (req.readyState == 4) {
        if (req.status == 200) {
            var text = "";
            var ndResult = req.responseXML.documentElement.getElementsByTagName("GetAirplanesResult")[0];
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
            alert("Async call failed - " + req.status + " " + req.statusText);
        }
    }
}