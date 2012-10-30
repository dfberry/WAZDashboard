$(document).ready(function () {
    try {

        groupSelected = $('#ctlFilter1').children("option:selected").val();

        // ddl
        jQuery("#ctlFilter1").change(function () {

            groupSelected = $(this).val();
            getPage(groupSelected);

            if (groupSelected) {
                if (groupSelected == "clear") {
                    jQuery("#dfbHtmlTable").jqGrid('groupingRemove', true);
                } else {
                    jQuery("#dfbHtmlTable").jqGrid('groupingGroupBy', groupSelected);
                }
            }
        });

        getPage(groupSelected);
    }
    catch (err) {

        alert(err);

    }

});
function getPage(issueGroup) {

    daysSelected = 30;

    summaryURI = '/Issues/IssueSummary/?majorfilter=' + issueGroup + '&minorfilter=' + daysSelected;
    tableURI = '/Issues/List/?majorfilter=' + issueGroup;
    retrievalDateURI = '/Issues/RetrievalDate/';

    getGraphIssues(summaryURI, groupSelected, daysSelected, 'dfbchart');
    getTableIssues(tableURI);
    //getRetrievalDate(retrievalDateURI)
}