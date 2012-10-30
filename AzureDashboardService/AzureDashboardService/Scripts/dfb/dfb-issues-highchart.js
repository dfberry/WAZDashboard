// Either page request or form submission - grab data and build chart
function getGraphIssues(uri, groupSelected, daysSelected, chartname) {

    try{

        $.getJSON(uri, function (jsondata) {


            // set title to default
            if (groupSelected == "clear") {
                var title = 'Issues by Service for ' + daysSelected + ' days'
            }
            else {
                // construct title
                // fiddle with service name 
                if (groupSelected.indexOf("Name") != -1) {
                    var tempTitle = groupSelected.replace("Name", "");
                } else {
                    var tempTitle = groupSelected.replace("Issue", "");
                }

                // finish title
                var title = 'Issues by ' + tempTitle + ' for ' + daysSelected + ' days'
            }

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
                    data: jsondata
                }]
            });

        });
    }
    catch (err) {
        alert("getGraphIssues:: " + err);
    }
}