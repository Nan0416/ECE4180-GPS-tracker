// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
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

let path_data = []; // [[lat, long]]

let svg_path = svg.append('g').append('path') // setup path
    .datum(path_data)
    .attr('fill', 'none')
    .attr('stroke', 'blue')
    .attr('stroke-width', '2px');
let fun = d3.line()
    .x((d)=>{ return myMap.latLngToLayerPoint(d).x;})
    .y((d)=>{ return myMap.latLngToLayerPoint(d).y;})
    .curve(d3.curveBasis);
function draw(){
    svg_path.attr('d', fun);
}
myMap.on('zoomend', draw);
lat = 33.7762904;
long = -84.4005669;

setInterval(()=>{
    path_data.push([lat, long]);
    lat += 0.0008 * Math.random() - 0.0001;
    long += 0.0008 * Math.random() - 0.0001;
    draw();
},1000);