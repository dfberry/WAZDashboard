// dfb written to product HighChart chart for Azure Issues



// build chart
function getChart(data, title, chartname) {

    var i = 0;

    chart = new Highcharts.Chart({
        chart: {
            renderTo: chartname,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: title
        },
        tooltip: {
            formatter: function () {
                return this.point.name + ': ' + Math.round(this.percentage) + ' %';
            }
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    formatter: function () {
                        return this.point.name + ': ' + Math.round(this.percentage) + ' %';
                    }
                },
                showInLegend: true,
                enableMouseTracking: true
            }
        },
        series: [{
            type: 'pie',
            name: title,
            data: data
        }]
    });
}

function getTable(groupSelected, daysSelected, htmlObjectName) {
    $.getJSON('/FeedIssue/ServiceIssueSummary/', function (data) {

        var plot1 = jQuery.jqplot(htmlObjectName, [data], {
            seriesDefaults: {
                // Make this a pie chart.
                sliceMargin: 1,
                renderer: jQuery.jqplot.PieRenderer,
                rendererOptions: {
                    // Put data labels on the pie slices.
                    // By default, labels show the percentage of the slice.
                    showDataLabels: true
                },
            },
            legend: { show: true, location: 'e' }});
    });
}

// Either page request or form submission - grab data and build chart
function getGraph(groupSelected, daysSelected, htmlObjectName) {

    $.getJSON('/FeedIssue/IssueSummary/?majorfilter=' + groupSelected + '&minorfilter=' + daysSelected, function (jsondata) {

        var title = groupSelected + ' for ' + daysSelected + ' days'

        getChart(jsondata, title, htmlObjectName);

    });
}

function loadPage(graphHtmlObjectName, tableHtmlObjectName) {

    //alert("loadPage function");

    // get drop down list selection
    daysSelected = $('#ctlFilter2').children("option:selected").val();
    groupSelected = $('#ctlFilter1').children("option:selected").val();

            
    getGraph(groupSelected, daysSelected, graphHtmlObjectName);
    getTable(groupSelected, daysSelected, tableHtmlObjectName);

    // reset UX elements
    if (groupSelected) {
        $("#ctlFilter1").val(groupSelected).attr("selected", "selected");
    }

    if (daysSelected) {
        $("#ctlFilter2").val(daysSelected).attr("selected", "selected");
    }
}

$(document).ready(function () {

    //alert("document ready");

    try {

        $('#filter').submit(function (e) {

            //alert("form submit");

            loadPage();

            return false;
        });


        loadPage();
    }
    catch (err) {

        alert(err);

    }
});

