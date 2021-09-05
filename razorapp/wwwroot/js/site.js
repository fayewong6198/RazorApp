// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var noteStrings = [
    "A0", "A#0", "B0",
    "C1", "C#1", "D1", "D#1", "E1", "F1", "F#1", "G1", "G#1", "A1", "A#1", "B1",
    "C2", "C#2", "D2", "D#2", "E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2",
    "C3", "C#3", "D3", "D#3", "E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3",
    "C4", "C#4", "D4", "D#4", "E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4",
    "C5", "C#5", "D5", "D#5", "E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5",
    "C6", "C#6", "D6", "D#6", "E6", "F6", "F#6", "G6", "G#6", "A6", "A#6", "B6"
];

var prevalue = ""

if (navigator) {
    navigator.mediaDevices.getUserMedia({ audio: true, })
        .then(function(stream) {
            let audioContext = new AudioContext();

            // Reduce noices
            let compressor = audioContext.createDynamicsCompressor();

            // compressor.threshold.value = -50;
            // compressor.knee.value = 40;
            // compressor.ratio.value = 12;
            // compressor.reduction.value = -20;
            // compressor.attack.value = 0;
            // compressor.release.value = 0.25;

            // let filter = audioContext.createBiquadFilter();
            // filter.Q.value = 8.30;
            // filter.frequency.value = 355;
            // filter.gain.value = 3.0;
            // filter.type = 'bandpass';
            // filter.connect(compressor);

            // compressor.connect(audioContext.destination)
            // filter.connect(audioContext.destination)



            /* use the stream */

            var node = audioContext.createGain(4096, 2, 2)

            let analyzer = audioContext.createAnalyser();
            let microphone = audioContext.createMediaStreamSource(stream);


            let javascriptNode = audioContext.createScriptProcessor(2048, 1, 1);

            analyzer.smoothingTimeConstant = 0.8;
            analyzer.fftSize = 1024;

            microphone.connect(analyzer);
            analyzer.connect(javascriptNode);
            javascriptNode.connect(audioContext.destination)

            javascriptNode.onaudioprocess = function() {
                let array = new Uint8Array(analyzer.frequencyBinCount);
                let array2 = new Float32Array(2048);

                analyzer.getByteFrequencyData(array);

                analyzer.getFloatTimeDomainData(array2)

                var ac = autoCorrelate(array2, audioContext.sampleRate);
                // console.log("ac ====>>>>", frequencyToNote(ac * 2))

                var values = 0

                var length = array.length;

                for (let i = 0; i < length; i++) {
                    values += array[i];
                }

                var average = values / length

                if (average > 12) {
                    let value = frequencyToNote(ac * 2);


                    if (true) {
                        if (value != "") {
                            console.log("ac ====>>>>", value, array2)

                        }
                        prevalue = value

                    }

                }

                // console.log("Mic detected ===========>>> ", Math.round(average))
            }

            node.connect(javascriptNode)
        })
        .catch(function(err) {
            console.log(err)
                /* handle the error */
        });
}

function noteFromPitch(frequency) {
    var noteNum = 12 * (Math.log(frequency / 440) / Math.log(2));
    return Math.round(noteNum) + 69;
}


function autoCorrelate(buf, sampleRate) {
    // Implements the ACF2+ algorithm
    var SIZE = buf.length;
    var rms = 0;

    for (var i = 0; i < SIZE; i++) {
        var val = buf[i];
        rms += val * val;
    }
    rms = Math.sqrt(rms / SIZE);
    if (rms < 0.01) // not enough signal
        return -1;

    var r1 = 0,
        r2 = SIZE - 1,
        thres = 0.2;
    for (var i = 0; i < SIZE / 2; i++)
        if (Math.abs(buf[i]) < thres) { r1 = i; break; }
    for (var i = 1; i < SIZE / 2; i++)
        if (Math.abs(buf[SIZE - i]) < thres) { r2 = SIZE - i; break; }

    buf = buf.slice(r1, r2);
    SIZE = buf.length;

    var c = new Array(SIZE).fill(0);
    for (var i = 0; i < SIZE; i++)
        for (var j = 0; j < SIZE - i; j++)
            c[i] = c[i] + buf[j] * buf[j + i];

    var d = 0;
    while (c[d] > c[d + 1]) d++;
    var maxval = -1,
        maxpos = -1;
    for (var i = d; i < SIZE; i++) {
        if (c[i] > maxval) {
            maxval = c[i];
            maxpos = i;
        }
    }
    var T0 = maxpos;

    var x1 = c[T0 - 1],
        x2 = c[T0],
        x3 = c[T0 + 1];
    a = (x1 + x3 - 2 * x2) / 2;
    b = (x3 - x1) / 2;
    if (a) T0 = T0 - b / (2 * a);

    return sampleRate / T0;
}

var errorNumber = 0.98

function frequencyToNote(freqency) {
    if (freqency <= 0) {
        return ""
    }

    var number = Math.log2(freqency / 27.5) * 12
        // console.log("cc", number)
    if (Math.abs(number - Math.round(number)) > 0.2) {
        return ""
    }

    if (number < 0 || number > 60) {
        return ""
    }

    return noteStrings[Math.round(number)]

}