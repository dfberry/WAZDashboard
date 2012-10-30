function getTableIssues(uritable) {

    try {

        $("#dfbHtmlTable").jqGrid({
            url: uritable,
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Date', 'Service', 'Location', 'Status', 'Title', 'Description'],
            colModel: [
   		            { name: 'IssueDate', index: 'IssueDate' },
                    { name: 'ServiceName', index: 'ServiceName' },
   		            { name: 'LocationName', index: 'LocationName' },
   		            { name: 'IssueStatus', index: 'IssueStatus' },
   		            { name: 'IssueTitle', index: 'IssueTitle' },
                    { name: 'IssueDescription', index: 'IssueDescription' }
   	            ],
            pager: '#dfbHtmlTablePager',
            rowNum: 500,
            rowList: [10, 50, 100, 500],
            sortname: 'IssueDate',
            sortorder: "desc",
            toppager: "true",
            viewrecords: true,
            height: "100%",
            imgpath: '',
            grouping: false,
            groupingView: {
                groupField: ['ServiceName'],
                groupColumnShow: [false],
                groupText: ['<b>{0} - {1} Items(s)</b>'],
                groupCollapse: [false],
                groupOrder: ['asc'],
                groupDataSorted:[true],
                groupSummary: [true]
            },
            gridview: true,
            loadonce: true,
            caption: 'Issues',
            loadComplete: function () {

                // dfb-get response header for data build date then display
//                var key = XMLHttpRequest.getResponseHeader('RetrievalDate');
//                $("#retrievaldate").text(jsondata);

            }

        });
    }
    catch (err) {
        alert("getTable:: " + err);
    }
}