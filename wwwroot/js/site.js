/* Initialization 
Add the map layer,
Initialize svg path
*/

let atlLatLng = new L.LatLng(33.7762904,-84.4005669); // @Atlanta
let myMap = L.map('map').setView(atlLatLng, 16.32); // create map and set initial point and zoom
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
let svg = d3.select('#map').select('svg');

let path_data = []; // [{timeStamp: 1555798086263, tripId: 7, lat_: 33.7791268, long_: -84.4075186}]

let svg_path = svg.append('g').append('path') // setup path
    .attr('fill', 'none')
    .attr('stroke', 'blue')
    .attr('stroke-width', '2px');
let fun = d3.line()
    .x((d)=>{ return myMap.latLngToLayerPoint([d.lat_, d.long_]).x;})
    .y((d)=>{ return myMap.latLngToLayerPoint([d.lat_, d.long_]).y;})
    .curve(d3.curveBasis);
function draw(){
    svg_path
        .datum(path_data)
        .attr('d', fun);
}
myMap.on('zoomend', draw);


/* functions for obtaining data from the server */
let globalHandle = null;
let currentPresentedTrip = null;


function getAllTrips(callback){
    url = "home/tripstatus";
    jqxhr = $.get(url, null, null, "json");
    jqxhr.done((data, status, jqxhr_)=>{
        // stop current handle if current trip is terminated.
        if(currentPresentedTrip != null && globalHandle != null){
            for(let index = 0; index < data.length; index++){
                if(data[index].tripId == currentPresentedTrip && data[index].status == 2){
                    // current running trip has been terminated.
                    clearInterval(globalHandle);
                }
            }
        }
        callback(data);
    }).fail((jqxhr_, status, data)=>{
        //alert(`Http Get Request Error: ${url}, ${status}, ${data}`)
        callback([]);
    })
}

// obtain a terminated (archived) trip's locations from the server.
function getTerminatedTrips(tripId, callback){
    url = `archive/trip/${tripId}`
    jqxhr = $.get(url, null, null, "json");
    jqxhr.done((data, status, jqxhr_)=>{
        callback(data);
    }).fail((jqxhr_, status, data)=>{
        callback(null);
    });
}
// periodically obtain a realtime trip's locations after a timestamp.

function getRealTimeTrips(tripId, interval, callback){
    let realtimeLocations = [];
    callback(realtimeLocations);
    timeStamp = -1;
    handle = setInterval(()=>{
        url = `realtime/trip/${tripId}${timeStamp < 0? "": "/" + (timeStamp + 1)}`
        console.log(url);
        jqxhr = $.get(url, null, null, "json");
        jqxhr.done((data, status, jqxhr_)=>{
            // add data to realtimeLocations
            if(!Array.isArray(data)){
                alert(`getRealtimeTrips: Unexpected type ${typeof(data)}`)
                clearInterval(handle);
                return;
            }
            if(data.length == 0){
                return;
            }
            for(let index = 0; index < data.length; index++){
                let location = data[index];
                realtimeLocations.push(location);
                // set timeStamp
                timeStamp = location.timeStamp;
                console.log(timeStamp);
            }
            callback(realtimeLocations);
        }).fail((jqxhr_, status, data)=>{
            callback(realtimeLocations);
        });
    },interval);
    return handle;
}




/*UI function*/

/**
 * key: tripId
 * value: trip
 */
let tripstatus = new Map(); 
function showTrips(tripstatus_arr){
    let tempSet = new Set();
    if(!Array.isArray(tripstatus_arr)) return;
    // assembly set
    for(let index = 0; index < tripstatus_arr.length; index++){
        tempSet.add(tripstatus_arr[index].tripId);
    }
    // delete unexisted trips
    for(let [key, _] of tripstatus){
        if(!tempSet.has(key)){
            console.log('delete');
            $(`#select-option-${key}`).remove(); // remove option (html element)
            tripstatus.delete(key); // remove the trip
        }
    }
    // refresh tripstatus
    for(let index = 0; index < tripstatus_arr.length; index++){
        let trip = tripstatus_arr[index];
        let tripId = trip.tripId;
        let option_text = `Trip Id: ${trip.tripId}, Device Id: ${trip.devicenum}, Status: ${trip.status == 1? "Running": "Terminated"}`;
        let option_value = `${trip.tripId}`;
        // console.log(option_text);
        if(tripstatus.has(tripId)){
            // let option = tripstatus.get(tripId)[1];
            let old_text = $(`#select-option-${tripId}`).html();
            if(old_text != option_text){
                $(`#select-option-${tripId}`).text(option_text);
            }
            tripstatus.set(tripId, trip);
        }else{
            // add new option
            let option = new Option(option_text, option_value);
            $(option).attr('id', `select-option-${tripId}`);
            $("#trip-selection").append(option);
            tripstatus.set(tripId, trip);
        }
    }
}

// draw terminated trip
function drawTerminatedTrip(locations){
    if(locations == null){
        console.log("Error......");
        return;
    }
    path_data = locations;
    console.log(locations);
    draw();
}

// draw realtime trip
function drawRealtimeTrip(locations){
    if(locations == null){
        console.log("Error......");
        return;
    }
    path_data = locations;
    draw();
}



/* Helper functions */

function drawTrip(tripId){
    if(!tripstatus.has(tripId)){
        return null;
    }
    if(tripstatus.get(tripId).status == 1){
        // running
        handle = getRealTimeTrips(tripId, 500, drawRealtimeTrip);
        return handle;
    }else if(tripstatus.get(tripId).status == 2){
        // terminated
        getTerminatedTrips(tripId, drawTerminatedTrip);
        return null;
    }else{
        alert(`drawTrip: Unknown trip status (${tripstatus.get(tripId).status})`);
        return null;
    }
}

/* event handler and time drivers */
$('#trip-selection').change(()=>{
    let tripId = parseInt($('#trip-selection').children("option:selected").val());
    if(tripId < 0){
        return;
    }
    if(globalHandle) clearInterval(globalHandle);
    globalHandle = drawTrip(tripId);
    currentPresentedTrip = tripId;
})


$(document).ready(()=>{
    setInterval(()=>{
        getAllTrips(showTrips);
    },1000); 
});

