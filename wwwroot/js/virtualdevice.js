let hasGPS = 1;
if (!navigator.geolocation) {
    alert("no gps location service");
    hasGPS = 0;
}
let myMap;
let location__ = [];
let timestamp = 0;
let circle;
let deviceId = 0x02;
let tripId;
let recording = 0;

function init(){
    navigator.geolocation.getCurrentPosition((position)=>{
        location__ = [position.coords.latitude, position.coords.longitude];
        timestamp = position.timestamp;
        let atlLatLng = new L.LatLng(position.coords.latitude, position.coords.longitude); // @Atlanta
        myMap = L.map('main-map').setView(atlLatLng, 16.32); // create map and set initial point and zoom
        /**
            Add tile layer and points.
            You can use different tile layer.
        */
        L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/terrain/{z}/{x}/{y}{r}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 3,
            maxZoom: 19,
            ext: 'png'
        }).addTo(myMap);
        let _svgPathLayer = L.svg();
        _svgPathLayer.addTo(myMap)
        let svg = d3.select('#main-map').select('svg');
        circle = svg.append('circle') // setup path
            .datum({location: location__})
            .attr('fill', 'green')
            .attr('r', 6)
            .attr('cx', d=>{return myMap.latLngToLayerPoint(d.location).x; })
            .attr('cy', d=>{return myMap.latLngToLayerPoint(d.location).y; });
    });
}
function redraw(){
    circle
        .datum({location: location__})    
        .attr('cx', d=>{return myMap.latLngToLayerPoint(d.location).x; })
        .attr('cy', d=>{return myMap.latLngToLayerPoint(d.location).y; });
}
function append(){
    $("#main-console").append(`<p>${timestamp} ${location__[0]} ${location__[1]}</p>`);
}
function start(){
    setInterval(()=>{
        navigator.geolocation.getCurrentPosition((position)=>{
            location__ = [position.coords.latitude, position.coords.longitude];
            timestamp = position.timestamp;
            // redraw
            redraw();
            if(recording == 1){
                append();
                $.get( `realtime/uploadposition/${tripId}/${location__[0]}/${location__[1]}`, ()=>{
                    
                });
            }
            
        })
    }, 1000);
}
$('#control-button').click(()=>{
    if(recording == 0){
        $.get( `home/startnewtrip/${deviceId}`, function( data ) {
            tripId = data;
            $("#control-status").css('background-color', 'green');
            $('#control-status').text(`tripId: ${data}`);
            $('#control-button').text('Terminate');
            recording = 1;
        });
    }else{
        $.get( `home/endtrip/${tripId}`, function( data ) {
            if(data == "OK"){
                $("#control-status").css('background-color', 'red');
                $('#control-status').text(`${data}`);
                $('#control-button').text('Start')
                recording = 0;
            }
        });
    }
})

$(document).ready(function(){
    
    if(hasGPS == 0){
        return;
    }
    init();
    start();
});
