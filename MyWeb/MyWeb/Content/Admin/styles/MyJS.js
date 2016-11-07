if (location.protocol === "file:") {
    document.getElementById("webserver-warning").style.display = "block";
}

var player = bitdash("player");

var conf = {
    key: "6907766c-1663-442c-97e1-15c4363a0137",
    source: {
        dash: "http://123.30.146.150:1935/live/Foscam2.stream/manifest.mpd",
        hls: "http://123.30.146.150:1935/live/Foscam2.stream/playlist.m3u8"
    },
    style: {
        width: "100%",
        aspectratio: "16:9",
        controls: true
    }
};

player.setup(conf).then(function (value) {
    // Success
    console.log("Successfully created bitdash player instance");
}, function (reason) {
    // Error!
    console.log("Error while creating bitdash player instance");
});