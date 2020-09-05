"use strict";
// Class definition

var KTDatatableHtmlTableListQuest = function() {
    // Private functions
    var datatable;
    // initializer
    var table = function(data,helper) {
        var dataJSONArray = JSON.parse(data);
        console.log(dataJSONArray);
       if (dataJSONArray != null) {
           datatable = $("#kt_datatable").KTDatatable({
               data: {
                   type: 'local',
                   saveState: {cookie: false},
                   source:dataJSONArray,
               },
               search: {
                   input: $('#kt_datatable_search_query'),
                   key: 'generalSearch'
               },
               columns: [
                   {
                       field: 'Img',
                       title:'Img',
                       type: 'image',
                       width: 100
                   },
                   {
                       field: 'City',
                       title:'City',
                       type: 'text',
                       width: 100
                    
                   }, 
                   {
                       field: 'Name',
                       title:'Name',
                       type: 'text',
                       width: 100
                    
                   },
                   {
                       field: 'Description',
                       title: 'Description',
                       type: 'text',
                       width: 500
                   },

                   //}, {
                   //    field: 'Action',
                   //    type: 'action',
                   //    autoHide: false,
                   //    width: 150
					
                   //},
               ],
           });
           console.log(datatable);
       }
		
        //datatable.on('click', '[data-record-id]', function() {

        //    helper.invokeMethodAsync("InvokeMethod", $(this).data('record-id'));
        //});
    };

    var reload = function() {
        datatable.reload();
    }
    var removeRow = function(id) {
        datatable.rows("#" + id).remove();
    }
    return {
        // Public functions
        init: function(data,helper) {
            // init 
            table(data,helper);
        },
        reload:function() {
            reload();
        },
        removeRow:function(id) {
            removeRow(id);
        }

    };
}();


