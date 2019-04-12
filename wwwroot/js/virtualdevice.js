if (!navigator.geolocation) {
    alert("no gps location service");
}
$(document).ready(function(){
    setInterval(() => {
        navigator.geolocation.getCurrentPosition((position)=>{
            console.log(position.coords.latitude, position.coords.longitude);
            $("#main-console p").text(`${position.coords.latitude} , ${position.coords.longitude}`);
            /*
                {
                    timestamp: number,
                    coords:{
                        latitude: number, 
                        longitude: number,
                        accuracy: number,
                        heading:
                        speed:
                    }
                }
             
            */
        });
    }, 1000);
});
