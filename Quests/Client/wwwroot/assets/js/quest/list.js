"use strict";
// Class definition

var KTDatatableHtmlTableListQuest = function() {
    // Private functions

    // initializer
    var table = function() {

		var datatable = $('#kt_datatable').KTDatatable({
			data: {
				saveState: {cookie: false},
			},
			search: {
				input: $('#kt_datatable_search_query'),
				key: 'generalSearch'
			},
			columns: [
				{
					field: 'Изображение',
					type: 'image',
                    width: 100
                },
				{
					field: 'Город',
					type: 'text',
                    width: 100
                    
				}, 
                {
                    field: 'Заголовок',
                    type: 'text',
                    width: 100
                    
                },
                {
					field: 'Описание',
                    type: 'text',
                    width: 500
					
				}, {
					field: 'Действие',
                    type: 'action',
					autoHide: false,
                    width: 150
					
				},
			],
		});


    };

    return {
        // Public functions
        init: function() {
            // init 
            table();
        },
    };
}();


