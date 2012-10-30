// Either page request or form submission - grab data and build chart
function getRetrievalDate(uri) {

    try {

        $.getJSON(uri, function (jsondata) {

            $("#retrievaldate").text(jsondata);

        });
    }
    catch (err) {
        alert("getRetrievalDate:: " + err);
    }
}