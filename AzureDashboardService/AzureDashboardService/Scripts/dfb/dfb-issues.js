// dfb written to product HighChart chart for Azure Issues



// build chart
function getChart(data, title, chartname) {

    
//    alert("getChart function");

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
                return this.point.name + ': ' + Math.round(this.percentage) + ' %, q=' + this.point.y; 
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

function getTable(groupSelected, daysSelected, htmlTableObjectName, htmlPageObjectName) {

//    alert("getTable function");

    var urlForJsonCall = '/FeedIssue/DynamicGridData/?majorfilter=' + groupSelected + '&minorfilter=' + daysSelected;

    try {

        $("#dfbHtmlTable").jqGrid({
            url: urlForJsonCall,
            datatype: 'json',
            mtype: 'POST',
            colNames: ['Id', 'Service', 'Location', 'Status', 'Date', 'Title', 'Description'],
            colModel: [
   		            { name: 'Id', index: 'Id' },
   		            { name: 'ServiceName', index: 'ServiceName' },
   		            { name: 'LocationName', index: 'LocationName' },
   		            { name: 'IssueStatus', index: 'IssueStatus' },
   		            { name: 'IssueDate', index: 'IssueDate' },
   		            { name: 'IssueTitle', index: 'IssueTitle' },
                    { name: 'IssueDescription', index: 'IssueDescription' }
   	            ],
            pager: '#dfbHtmlTablePager',
            rowNum: 500,
            rowList: [10, 50, 100, 500],
            sortname: 'Id',
            sortorder: "desc",
            toppager: "true",
            viewrecords: true,
            height: "100%",
            imgpath: '',
            grouping: true,
            groupingView: {
                groupField: [groupSelected],
                groupColumnShow: [false],
                groupText: ['<b>{0} - {1} Items(s)</b>'],
                groupCollapse: false,
                groupOrder: ['asc'],
                groupSummary: [true],
                groupDataSorted: true
            },
            gridview: true,
            caption: 'Issues',
            loadComplete: function () {
                var ret;
                //                alert("This function is executed immediately after\n data is loaded.");
                //                //ret = jQuery("#list15").jqGrid('getRowData', "13");

                $("#dfbHtmlTable").jqGrid('groupingGroupBy', groupSelected);

                if (groupSelected == "clear") { // remove grouping
                    jQuery("#issuelistgroupgrid").jqGrid('groupingRemove', true);
//                    jQuery("#issuelistgroupgrid").jqGrid('sortname', 'Id');
////                    jQuery("#issuelistgroupgrid").jqGrid('sortorder', 'asc');
                } else if (groupSelected == "IssueDate") { // deal with dates differently
                    jQuery("#issuelistgroupgrid").jqGrid('groupingGroupBy', groupSelected);
//                    jQuery("#issuelistgroupgrid").jqGrid('groupingGroupBy', groupSelected);
////                    jQuery("#issuelistgroupgrid").jqGrid('sortname', 'IssueDate');
////                    jQuery("#issuelistgroupgrid").jqGrid('groupingView', { groupOrder: ['desc'] });
                } else { // default sort is issue date
                    jQuery("#issuelistgroupgrid").jqGrid('groupingGroupBy', groupSelected);
//                    jQuery("#issuelistgroupgrid").jqGrid('sortname', 'IssueDate');
////                    jQuery("#issuelistgroupgrid").jqGrid('sortorder', 'desc');
                }
            }

        });
    }
    catch (err) {
//        alert("getTable:: " +err);
    }

    var x = 1;
}

// Either page request or form submission - grab data and build chart
function getGraph(groupSelected, daysSelected, htmlObjectName) {

//    alert("getGraph function");

    var urlForJsonCall = '/FeedIssue/IssueSummary/?majorfilter=' + groupSelected + '&minorfilter=' + daysSelected;
    
    var title = groupSelected + ' for ' + daysSelected + ' days'

    $.getJSON(urlForJsonCall, function (jsondata) {

        getChart(jsondata, title, htmlObjectName);

    });
}

function loadPage(graphHtmlObjectName, tableHtmlObjectName) {

//    alert("loadPage function");

    // get drop down list selection
    daysSelected = $('#ctlFilter2').children("option:selected").val();
    groupSelected = $('#ctlFilter1').children("option:selected").val();


    //getGraph(groupSelected, daysSelected, "dfbHtmlChart");
    getTable(groupSelected, daysSelected, "dfbHtmlTable", "dfbHtmlTablePager");

    // reset UX elements
    if (groupSelected) {
        $("#ctlFilter1").val(groupSelected).attr("selected", "selected");
    }

    if (daysSelected) {
        $("#ctlFilter2").val(daysSelected).attr("selected", "selected");
    }
}

$(document).ready(function () {

//    alert("document ready");

    try {

        $('#filter').submit(function (e) {

//            alert("form submit");

            loadPage();

            return false;
        });

        jQuery("#chngroup").change(function () {
            var vl = $(this).val();
            if (vl) {
                if (vl == "clear") {
                    jQuery("#issuelistgroupgrid").jqGrid('groupingRemove', true);
                } else {
                    jQuery("#issuelistgroupgrid").jqGrid('groupingGroupBy', vl);
                }
            }
        });


        loadPage();
    }
    catch (err) {

//        alert(err);

    }
});
