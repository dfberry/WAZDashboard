function readyTable() {

    // grab query params
    daysSelected = $('#ctlFilter2').children("option:selected").val();
    groupSelected = $('#ctlFilter1').children("option:selected").val();
    grouping = $('#chngroup').attr('checked');
    sort = $('#ctlSort').children("option:selected").val();

    // grab data and table
    getTable('/FeedIssue/DynamicGridData/', '#dfbHtmlTable', '#dfbHtmlTablePager',groupSelected, sort);

    // reset UX elements
    if (groupSelected) {
        $("#ctlFilter1").val(groupSelected).attr("selected", "selected");
    }

    if (daysSelected) {
        $("#ctlFilter2").val(daysSelected).attr("selected", "selected");
    }

    if (sort) {
        $("#ctlSort").val(sort).attr("selected", "selected");
    }

    // changeGrouping
    changeGrouping(grouping, '#dfbHtmlTable', groupSelected);
}

function getTable(urlToCall, htmlTableObject, htmlPagerObject, groupFieldName, sortFieldName) {

    //alert("getTable called");

    var urlForJsonCall = urlToCall + '?majorfilter=' + groupSelected + '&minorfilter=' + daysSelected;

    jQuery(htmlTableObject).jqGrid({
        url: urlToCall,
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
        pager: htmlPagerObject,
        rowNum: 100,
        rowList: [10, 50, 100],
        sortname: sortFieldName,
        sortorder: "desc",
        toppager: "true",
        viewrecords: true,
        height: "100%",
        imgpath: '',
        grouping: true,
        groupingView: {
            groupField: [groupFieldName],
            groupColumnShow: [false],
            groupText: ['<b>{0} - {1} Items(s)</b>'],
            groupCollapse: false,
            groupOrder: ['asc'],
            groupSummary: [true],
            groupDataSorted: true
        },
        gridview: true,
        caption: "Windows Azure Service Dashboard RSS Feed Issues",
        complete: function () {
            var ids = $.jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var id = ids[i];
                var rowData = grid.jqGrid('getRowData', id);
                $('#' + id, grid[0]).attr('title', rowData.IssueStatus + ' (' +
                                        rowData.IssueTitle + ', ' +
                                        rowData.IssueDescription + ')');
            }
        } 
    });
}
function changeGrouping(grouping, htmlTableObject, groupby) {

    //alert("changeGrouping called");

    // grab grouping param
    if (grouping == "checked") {
        jQuery(htmlTableObject).jqGrid('groupingGroupBy', groupby);
    } else {
        jQuery(htmlTableObject).jqGrid('groupingRemove', true);
    }

        
}
// Main Entry Point
$(document).ready(function () {

    //alert("document ready");

    try {
    
        // handle form submission
        $('#filter').submit(function (e) {

            //alert("form submit");

            readyTable();

            return false;
        });
        // initial page response
        readyTable();
    }
    catch (err) {

        alert(err);

    }

});